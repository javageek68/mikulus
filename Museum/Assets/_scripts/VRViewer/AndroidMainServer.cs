using UnityEngine;
using System.Collections;
using System.Net.Sockets;

/// <summary>
/// 
/// </summary>
public class AndroidMainServer : MonoBehaviour, ISocektServer {

    public string strServerIpAddress;
    public Texture2D txtRenderTexture = null;
    public byte[] bytImagedata = null;

    private bool blnReceivingImage = false;
    private bool blnSendingControllerData = false;
    private bool blnGotNewImage = false;
    private bool blnBadImage = false;

    private string strControllerInfo = "";
    private GUIStyle styleAlert = null;

	/// <summary>
	/// 
	/// </summary>
	void Start () {
        Debug.Log("starting AndroidMain");

        styleAlert = new GUIStyle();
        styleAlert.normal.textColor = Color.red;
        styleAlert.fontSize = 25;

        //init the server
        ServerControl.Instance.Init(ViewerConstants.PORT, ProtocolType.Tcp, this);
	}
	
	/// <summary>
	/// 
	/// </summary>
	void Update () {
        Screen.orientation = ScreenOrientation.Landscape;
        if (blnSendingControllerData == false) StartCoroutine(SendControllerInfo());
        if (blnGotNewImage == true) StartCoroutine(ProcessImage(bytImagedata));

        //recalibrate the direction if the user taps the screen
        if (Input.touchCount > 0) Compus.Instance.CallibrateDirection();

	}

    /// <summary>
    /// 
    /// </summary>
    void OnGUI()
    {
        if (txtRenderTexture != null)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), txtRenderTexture);
        }

        //tell the user we are getting bad images
        if (blnBadImage == true)
        {
            GUI.Label(new Rect(0, 0, 100, 20), "Bad Image", styleAlert);
        }

        GUI.Label(new Rect(0, 30, 100, Screen.width), strControllerInfo);

    }

    /// <summary>
    /// 
    /// </summary>
    public void OnServerStarted()
    {
        Debug.Log("OnServerStarted() event fired");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="client"></param>
    public void OnClientConnected(Socket client)
    {
        Debug.Log("OnClientConnected() event fired");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="read"></param>
    /// <param name="data"></param>
    public void OnReceive(SocketRead read, byte[] data)
    {
        //we got a screen shot from the pc
        Debug.Log("OnReceive() event fired.  bytes read " + data.Length);
        bytImagedata = data;
        blnGotNewImage = true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bytView"></param>
    /// <returns></returns>
    IEnumerator ProcessImage(byte[] bytView)
    {
        Texture2D txtCommTexture = null;
        //make sure we only process one image at a time
        if (blnReceivingImage == false)
        {
            blnReceivingImage = true;
    
            yield return 0;

            //create the new texture
            txtCommTexture = TextureUtilities.BytesToTexture(ViewerConstants.ViewerWidth, ViewerConstants.ViewerHeight, TextureFormat.RGB24, false, bytView);
            if (txtCommTexture.width > 8)
            {
                //assume this is a valid image
                blnBadImage = false;
                //destroy the old image
                DestroyObject(txtRenderTexture);
                //copy the new image over to be rendered
                txtRenderTexture = txtCommTexture;
            }
            else
            {
                //this is that question mark thingy
                blnBadImage = true;
                //we'll just keep the image from the last frame
            }
            blnReceivingImage = false; //the coeroutine is no longer running
            blnGotNewImage = false; //we have finished processing the latest image we got from the PC
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="read"></param>
    /// <param name="exception"></param>
    public void OnReceiveError(SocketRead read, System.Exception exception)
    {
        Debug.Log("OnReceiveError() event fired");
        Debug.Log(exception.ToString());
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
    /// <returns></returns>
    IEnumerator SendControllerInfo()
    {
        //string strControllerInfo = "";

        blnSendingControllerData = true;
        //get the mag heading
        strControllerInfo = Compus.Instance.fltHorizontalHeadingDelta.ToString() +
                            Compus.Instance.vctAcc.ToString() + ";";
        //strControllerInfo = Random.Range(0, 360).ToString();
        //send the mag heading to the PC
        ServerControl.Instance.SendTxToAll(strControllerInfo);
        blnSendingControllerData = false;
        yield return 0;
    }
}
