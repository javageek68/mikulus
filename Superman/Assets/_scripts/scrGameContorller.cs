using UnityEngine;
using System.Collections;

public class scrGameContorller : MonoBehaviour
{

    private bool blnDebug = false;


    public GUIStyle customStyle;
    public GUISkin customSkin;
    public DeviceOrEmulator devOrEmu;
    public Transform trnSuperMan;
    public float fltForwardSpeed = 10f;
    public float fltTurnSpeed = 5f;
    public float fltDiveSpeed = 5f;
    public AudioSource audThemeMusic;

    private float fltTotalGameTime = 60;  //a game last for 60 seconds
    private float fltTime = 0;
    private float fltGameStart = 0;
    private Kinect.KinectInterface kinect;
    private int intLineSize = 21;
    private KinectModelControllerV2 kinectCtlr = null;
    private Vector3 vCtlDir;
    private float fltVertical = 0;
    private float fltHorizontal = 0;
    private float fltSpeed = 0;
    private bool blnTrackingOn = false;
    private Quaternion qOrientation;

	// Use this for initialization
	void Start () {
        //get an instance of the Kinect device
        kinect = devOrEmu.getKinect();
        //get an instance of the Kinect mesh controller script
        kinectCtlr = KinectModelControllerV2.instance;
        
       
	}
	
	// Update is called once per frame
	void Update () {
        //determine if the Kinect is tracking
        blnTrackingOn = IsTrackingOn();

        vCtlDir = kinectCtlr.Hand_Right.transform.position - kinectCtlr.Head.transform.position ;
        fltSpeed = vCtlDir.magnitude;
        //square the speed
        fltSpeed = fltSpeed * fltSpeed;

        OVRDevice.GetOrientation(ref qOrientation);

        fltVertical = qOrientation.x;
        fltHorizontal = -qOrientation.z;

        //kinect.getSkeleton().SkeletonData

        if (blnTrackingOn)
        {
            //start the theme music if it hasnt started yet.
            if (audThemeMusic.isPlaying == false)
            {
                fltGameStart = Time.timeSinceLevelLoad;
                audThemeMusic.Play();
            }

            //Calculate the time the player has left
            fltTime = fltTotalGameTime - Time.timeSinceLevelLoad + fltGameStart;

            if (fltTime < 0) Application.LoadLevel(clsGameConstants.strMainMenu);

            //rotate
            trnSuperMan.Rotate(Vector3.up, fltHorizontal * fltTurnSpeed * Time.deltaTime);
            trnSuperMan.Rotate(Vector3.right, fltVertical * fltDiveSpeed * Time.deltaTime);
            //move forward
            trnSuperMan.position += trnSuperMan.forward * fltSpeed * fltForwardSpeed * Time.deltaTime;
        }
	}

    /// <summary>
    /// 
    /// </summary>
    void OnGUI()
    {
        GUI.skin = customSkin;

        //debug window
        if (blnDebug == true) GUI.Window(1, new Rect(0, 0, Screen.width * 0.25f, Screen.height * 1f), renderDebugWindow, "Debug Info");
    }


    void renderDebugWindow(int intWinId)
    {

        //debug info
        GUI.Label(new Rect(0, intLineSize * 1, Screen.width, intLineSize), "head " + kinectCtlr.Head.transform.position.ToString());
        GUI.Label(new Rect(0, intLineSize * 2, Screen.width, intLineSize), "left   hand     " + kinectCtlr.Hand_Left.transform.position.ToString());
        GUI.Label(new Rect(0, intLineSize * 3, Screen.width, intLineSize), "right  hand     " + kinectCtlr.Hand_Right.transform.position.ToString());

        GUI.Label(new Rect(0, intLineSize * 4, Screen.width, intLineSize), "ctrl vct   " + vCtlDir.ToString());
        GUI.Label(new Rect(0, intLineSize * 5, Screen.width, intLineSize), "ctrl mag   " + vCtlDir.magnitude.ToString());
        GUI.Label(new Rect(0, intLineSize * 6, Screen.width, intLineSize), "horz " + fltHorizontal.ToString("###.##"));
        GUI.Label(new Rect(0, intLineSize * 7, Screen.width, intLineSize), "vert " + fltVertical.ToString("###.##"));
        GUI.Label(new Rect(0, intLineSize * 8, Screen.width, intLineSize), "speed " + fltSpeed.ToString("###.##"));

        GUI.Label(new Rect(0, intLineSize * 10, Screen.width, intLineSize), "tracked     " + blnTrackingOn);

        GUI.Label(new Rect(0, intLineSize * 12, Screen.width, intLineSize), "device " + qOrientation.ToString());
    }

    bool IsTrackingOn()
    {
        bool blnTracked = false;
        for (int i = 0; i < kinectCtlr.sw.players.Length; i++)
        {
            if (kinectCtlr.sw.players[i] == Kinect.NuiSkeletonTrackingState.SkeletonTracked)
            {
                blnTracked = true;
                break;
            }
        }
        return blnTracked;
    }

}
