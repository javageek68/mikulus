using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Text;

/// <summary>
/// 
/// </summary>
public class PCMainClient : MonoBehaviour, ISocketClient {

    public int intDeviceScreenWidth;
    public int intDeviceScreenHeight;

    public ControllerMessages cntlMsgs = null;
    public GameObject DudeBase;
    public GameObject MainCam;

    public string strCamControllerData = "";
    public Vector3 vctBaseDir;
    public float fltBaseHorDir;
    public float fltCamHorDir;
    public float fltCamVertDir;

    public float fltBaseMaxSpeed = 3.0F;
    public float flrBaseMaxRotateSpeed = 3.0F;
    public float fltBaseSpeed;

    public CharacterController controller;

    private bool blnSendingScreenShot = false;
    //public Vector3 vctCamControllerAccel = new Vector3(0, 0, 0);
    private GUIStyle styleAlert = null;
    private bool blnGotDeviceResolution = false;
    private bool blnClientStarted = false;

    void Awake()
    {
        //Tell VRClient to open a connectin to the phone through the USB 
        //StartCoroutine(VRUtilities.StartVRClient(this));       
    }

	/// <summary>
	/// 
	/// </summary>
	void Start () {        
        styleAlert = new GUIStyle();
        styleAlert.normal.textColor = Color.red;
        styleAlert.fontSize = 15;

        //set the default screen resolution
        intDeviceScreenWidth = ViewerConstants.ViewerWidth;
        intDeviceScreenHeight = ViewerConstants.ViewerHeight;

        cntlMsgs = new ControllerMessages();

        DudeBase = GameObject.Find("Dude");
        MainCam = GameObject.Find("Main Camera");
        controller = DudeBase.GetComponent<CharacterController>();
	}
	
    /// <summary>
    /// 
    /// </summary>
	void Update () {

        if (blnGotDeviceResolution == true && (Screen.width != intDeviceScreenWidth || 
                                               Screen.height != intDeviceScreenHeight))
        {
            //set the game resolution on the PC to be the same as the device
            Screen.SetResolution(intDeviceScreenWidth, intDeviceScreenWidth, false);
        }
        
        //send the video to the phone
        StartCoroutine(VRUtilities.SendVideoStream(intDeviceScreenHeight, intDeviceScreenWidth));

        //the camera is parented to the Character Controller game object.  
        //Dude
        //  +-Capsule
        //  +-MainCamera

        //This code come directly from the Unity script manual
        DudeBase.transform.Rotate(0, Input.GetAxis("Horizontal") * flrBaseMaxRotateSpeed, 0);
        //get the vector the base in pointing toward
        vctBaseDir = DudeBase.transform.TransformDirection(Vector3.forward);
        //get the speed of the base
        fltBaseSpeed = fltBaseMaxSpeed * Input.GetAxis("Vertical");
        //apply speed and direction to the character controller
        controller.SimpleMove(vctBaseDir * fltBaseSpeed);

        //for some crazy reason, the Character Controller is incrementing Y even we are not moving along that direction
        //so we need to move back to the floor
        DudeBase.transform.position = new Vector3(DudeBase.transform.position.x, 1, DudeBase.transform.position.z);

        //get the heading of the base
        fltBaseHorDir = Mathf.Atan2(vctBaseDir.x, vctBaseDir.z);
        //we want to point the camera the angle as the controller.
        //since the camera is sitting on top of and is parented to the base, it needs to offset by the bases heading
        //we are also using the controller's delta instead of the controller's actual angle.  the actual angle is the
        //number of degrees from due north. if the player isnt actually facing due north, then this would cause the 
        //3D camera to look crooked.
        fltCamHorDir = fltBaseHorDir + cntlMsgs.attDeltaX * ViewerConstants.RAD2DEG;
        //for the vertical direction, we need to get the actual value instead of the offset
        fltCamVertDir = cntlMsgs.AttMessage.Z * ViewerConstants.RAD2DEG + 90;
        //set the camera's y axis angle
        MainCam.transform.localEulerAngles = new Vector3(fltCamVertDir, fltCamHorDir, 0);
        //if the user presses the R (reset) key, recalibrate the camera 
        //with the control device
        if (Input.GetKeyDown(KeyCode.R) == true) cntlMsgs.SetInitialAttitude();

        if (Input.GetKeyDown(KeyCode.P) == true) ScreenShotRenderTexture(@"C:\temp\UnityScreenshot\cap1.png");

	}

    /// <summary>
    /// 
    /// </summary>
    void OnGUI()
    {
        //GUI.Label(new Rect(10, 10, Screen.width, 20), "Controller Data :" + strCamControllerData);
        //GUI.Label(new Rect(10, 10, Screen.width, 20), "Controller Data :" + strCamControllerData, styleAlert);
        //if (cntlMsgs != null)
        //{
        //    GUI.Label(new Rect(10, 50, Screen.width, 20), "Controller Data :" + cntlMsgs.ToString(), styleAlert);
        //}
        if (blnClientStarted == false)
        {
            if (GUI.Button(new Rect(Screen.width/2 - 100, Screen.height/2 -20, 200,40),"Start Client"))
            {
                blnClientStarted = true;
                StartCoroutine(VRUtilities.StartVRClient(this));    
            }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="socket"></param>
    public void OnClientStarted(System.Net.Sockets.Socket socket)
    {
        Debug.Log("OnClientStarted() event fired");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="read"></param>
    /// <param name="data"></param>
    public void OnReceive(SocketRead read, byte[] data)
    {
        //Debug.Log("OnReceive() event fired. data size " + data.Length);
        string strHex = ByteArrayUtilities.ByteArrayToString(data);
        //Debug.Log("Hex : " + strHex);

        //Debug.Log("process Java controller data ");
        VRUtilities.DecodeControllerData(data, ref cntlMsgs);
        //Debug.Log("Controller Messages " + cntlMsgs.ToString());

        //make sure we only set the resolution once
        if (blnGotDeviceResolution == false)
        {
            //check to see if the device gave us its dimensions. if so, use them instead of the default
            if (cntlMsgs.OptionMessage.ScreenHeight > 0 && cntlMsgs.OptionMessage.ScreenWidth > 0)
            {
                intDeviceScreenWidth = cntlMsgs.OptionMessage.ScreenWidth;
                intDeviceScreenHeight = cntlMsgs.OptionMessage.ScreenHeight;

                intDeviceScreenWidth = TextureUtilities.getSquare(intDeviceScreenWidth, TextureUtilities.sqSquareType.lowest);
                intDeviceScreenHeight = TextureUtilities.getSquare(intDeviceScreenHeight, TextureUtilities.sqSquareType.lowest);

                blnGotDeviceResolution = true;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="read"></param>
    /// <param name="exception"></param>
    public void OnError(SocketRead read, System.Exception exception)
    {
        Debug.Log("OnError() event fired");
        Debug.LogError(exception.ToString());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="result"></param>
    public void OnEndHostComplete(System.IAsyncResult result)
    {
        Debug.Log("OnEndHostComplete() event fired");
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="strFileName"></param>
    void ScreenShotRenderTexture(string strFileName)
    {
        int resWidth = Screen.width;
        int resHeight = Screen.height;

        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        Camera.main.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        Camera.main.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        Camera.main.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        GameObject.Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        System.IO.File.WriteAllBytes(strFileName, bytes);

    }

}
