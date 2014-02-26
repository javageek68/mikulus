using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// </summary>
public class scrMainCam : MonoBehaviour {

    public static scrMainCam Instance = null;

    public scrPassenger psngrScript = null;
    public scrTaxiDriver txiDrvrScript = null;

    public GUIStyle customStyle;
    public GUISkin customSkin;

    public float fltLat = 0;
    public float fltLong = 0;
    public float fltPlayerRotation = 0;
    public float fltCamLong = 0;
    public float fltDistFromPlayer = 0;

    private bool blnDebug = true;
    private int intLineSize = 21;

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        scrMainCam.Instance = this;
        fltLong = 1.5f;
    }

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        //set up instances to our taxi and passenger scripts
        psngrScript = scrPassenger.Instance;
        txiDrvrScript = scrTaxiDriver.Instance;
    
    }

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        //compute the camera's longitude relative to the players
        fltPlayerRotation = txiDrvrScript.gameObject.transform.rotation.eulerAngles.y * Mathf.PI / 180;
        fltCamLong = fltLong - fltPlayerRotation ;

        //compute the camera's location relative to the player
        float fltX = txiDrvrScript.gameObject.transform.position.x - fltDistFromPlayer * Mathf.Cos(fltCamLong);
        float fltY = txiDrvrScript.gameObject.transform.position.y + fltDistFromPlayer * Mathf.Sin(fltLat);
        float fltZ = txiDrvrScript.gameObject.transform.position.z - fltDistFromPlayer * Mathf.Sin(fltCamLong);
        Vector3 vctCamPos = new Vector3(fltX, fltY, fltZ);
        transform.position = vctCamPos;
        transform.LookAt(txiDrvrScript.gameObject.transform.position);
    }

    /// <summary>
    /// 
    /// </summary>
    void OnGUI()
    {
        GUI.skin = customSkin;

        int intDashWinHeight = 0;
        int intDashWinWidth = 0;
        intDashWinHeight = (int) (Screen.height * 0.25f);
        intDashWinWidth = Screen.width;
        GUI.Window(0, new Rect(0, Screen.height - intDashWinHeight, intDashWinWidth, intDashWinHeight), renderDashboardWindow, "Dashboard");
        //debug window
        if (blnDebug == true) GUI.Window(1, new Rect(0, 0, Screen.width * 0.25f, Screen.height * 0.25f), renderDebugWindow, "Debug Info");
    }

    void renderDashboardWindow(int intWinId)
    {
        GUI.Label(new Rect(0, intLineSize * 1, Screen.width, intLineSize), psngrScript.GetPassengerMessage(), "label");
        GUI.Label(new Rect(0, intLineSize * 2, Screen.width, intLineSize), "Fare $ " + string.Format("{0:00.00}", psngrScript.fare.fare), "label");
        GUI.Label(new Rect(0, intLineSize * 3, Screen.width, intLineSize), "Earnings $ " + string.Format("{0:00.00}", txiDrvrScript.fltEarnings), "label");
        GUI.Label(new Rect(0, intLineSize * 4, Screen.width, intLineSize), "Passenger location    " + psngrScript.location, "label");
        GUI.Label(new Rect(Screen.width * 0.5f, intLineSize * 1, Screen.width, intLineSize), "Lives  " + string.Format("{0:00}", txiDrvrScript.intLives), "label");
        GUI.Label(new Rect(Screen.width * 0.5f, intLineSize * 2, Screen.width, intLineSize), "Damage " + string.Format("{0:00.00}", txiDrvrScript.fltDamage), "label");
        GUI.Label(new Rect(Screen.width * 0.5f, intLineSize * 3, Screen.width, intLineSize), "Fuel   " + string.Format("{0:00.00}", txiDrvrScript.fltFuel), "label");
        GUI.Label(new Rect(Screen.width * 0.5f, intLineSize * 4, Screen.width, intLineSize), "Passenger destination " + psngrScript.destination, "label");
    }


    void renderDebugWindow(int intWinId)
    {
        //debug info
        GUI.Label(new Rect(0, intLineSize * 1, Screen.width, intLineSize), "Taxi state             " + txiDrvrScript.state);
        GUI.Label(new Rect(0, intLineSize * 2, Screen.width, intLineSize), "Taxi location          " + txiDrvrScript.location);
        GUI.Label(new Rect(0, intLineSize * 3, Screen.width, intLineSize), "Passenger state       " + psngrScript.stateManager.state);
        GUI.Label(new Rect(0, intLineSize * 4, Screen.width, intLineSize), "Taxi velocity         " + string.Format("{0:00.00000}", txiDrvrScript.gameObject.rigidbody.velocity.magnitude));
        GUI.Label(new Rect(0, intLineSize * 5, Screen.width, intLineSize), "Taxi radial velocity  " + string.Format("{0:00.00000}", txiDrvrScript.gameObject.rigidbody.angularVelocity.magnitude));
    
    }
}
