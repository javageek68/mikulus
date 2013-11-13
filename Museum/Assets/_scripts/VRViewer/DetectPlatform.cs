using UnityEngine;
using System.Collections;

/// <summary>
/// This script's only job is to detect what platform it is running on and
/// load the correct scene file
/// </summary>
public class DetectPlatform : MonoBehaviour {
    public enum enTestPlatform
    { 
        PC,
        Android
    }

    public enTestPlatform testPlatform = enTestPlatform.PC;
    public bool blnTesting = true;
    public bool blnStart = false;

    private int intGUILabelWidth = 100;
    private int intGUILabelHeight = 20;
    private int intGUIButtonWidth = 100;
    private int intGUIButtonHeight = 20;

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        //determine if we should load the PC interface or the Android interface
        Detect();
    }

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        //if we are running in test mode, then we need to continue to check while
        //the user decides which one to load.
        Detect();
    }

    /// <summary>
    /// 
    /// </summary>
    void OnGUI()
    {
        //if we are in test mode, we should display a prompt to the user that he needs to
        //select a platform from the Inspector
        if (blnTesting == true)
        {
            GUI.Label(new Rect(Screen.width / 2 - intGUILabelWidth / 2, Screen.height / 2 - intGUILabelHeight / 2, intGUILabelWidth, intGUILabelHeight), "Select Platform");

            if (GUI.Button(new Rect(Screen.width / 2 - intGUILabelWidth / 2, Screen.height / 2 + intGUILabelHeight , intGUILabelWidth, intGUILabelHeight), "Android"))
            {
                testPlatform = enTestPlatform.Android;
                blnStart = true;
            }
            if (GUI.Button(new Rect(Screen.width / 2 - intGUILabelWidth / 2, Screen.height / 2 + intGUILabelHeight * 2, intGUILabelWidth, intGUILabelHeight), "PC"))
            {
                testPlatform = enTestPlatform.PC;
                blnStart = true;
            }

        }
    }
	
    /// <summary>
    /// 
    /// </summary
    void Detect()
    {
        //only go into test mode if we are NOT running on the android
        if (blnTesting == true && Application.platform != RuntimePlatform.Android)
        {
            //we are in test mode
            //give the user a chance to use the Unity Inspector to 
            //select a platform and start the application
            if (blnStart == true)
            {
                if (testPlatform == enTestPlatform.PC)
                {
                    //the user select PC from the Inspector
                    Application.LoadLevel("PCMain");
                }
                else
                {
                    //the user select PC from the Inspector
                    Application.LoadLevel("AndroidMain");

                }
            }
        }
        else
        {
            //we are NOT in test mode.  Just detect and move on
            if (Application.platform == RuntimePlatform.Android)
            {
                Application.LoadLevel("AndroidMain");
            }
            else
            {
                Application.LoadLevel("PCMain");
            }            
        }

    }
}
