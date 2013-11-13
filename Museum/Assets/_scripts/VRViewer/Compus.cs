using UnityEngine;
using System.Collections;

public class Compus : MonoBehaviour {
    public Vector3 vctRaw;
    public Vector3 vctAcc;
    public float fltMagHeading;
    public float fltTrueHeading;
    public bool blnCompEnabled;
    public float fltHorizontalHeadingDelta = 0;

    private float fltInitialMagneticHeading = 0;
    
    private int intGUILabelLeft = 10;
    private int intGUIRow = 10;
    private int intGUILabelWidth = 100;
    private int intGUILineHeight = 20;
    private int intGUILineSpace = 5;
    private int intGUILineLeft = 100;

    //private int intWindowWidth = (int)(Screen.width * 0.8f);
    //private int intWindowHeight = (int)(Screen.height * 0.3f);

    private Camera camMainCam;

    public static Compus Instance { get; set; }

    // Use this for initialization
    void Start()
    {
        Instance = this;
        Input.compass.enabled = true;

        CallibrateDirection();
    }

    // Update is called once per frame
    void Update()
    {

        vctRaw = Input.compass.rawVector;
        fltMagHeading = Input.compass.magneticHeading;
        fltTrueHeading = Input.compass.trueHeading;
        vctAcc = Input.acceleration;
        blnCompEnabled = Input.compass.enabled;

        fltHorizontalHeadingDelta = fltMagHeading - fltInitialMagneticHeading;

    }

    /// <summary>
    /// 
    /// </summary>
    void OnGUI()
    {
        //GUI.Window(0, new Rect(Screen.width / 2 - intWindowWidth / 2, Screen.height * 0.45f, intWindowWidth, intWindowHeight), renderWindow, "Compus Info");
    }

    public void CallibrateDirection()
    {
        fltInitialMagneticHeading = Input.compass.magneticHeading;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="intWinId"></param>
    void renderWindow(int intWinId)
    {
        intGUILabelLeft = 10;
        intGUILineLeft = 100;

        intGUIRow = 10;
        drawGUILine("mag  Heading ", fltMagHeading.ToString());
        drawGUILine("true Heading ", fltTrueHeading.ToString());
        drawGUILine("acceleration ", vctAcc.ToString());

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strPrompt"></param>
    /// <param name="strText"></param>
    /// <returns></returns>
    void drawGUILine(string strPrompt, string strText)
    {
        intGUIRow += intGUILineHeight + intGUILineSpace;

        GUI.Label(new Rect(intGUILabelLeft, intGUIRow, intGUILabelWidth, intGUILineHeight), strPrompt);
        GUI.Label(new Rect(intGUILineLeft, intGUIRow, intGUILabelWidth, intGUILineHeight), strText);
    }

}
