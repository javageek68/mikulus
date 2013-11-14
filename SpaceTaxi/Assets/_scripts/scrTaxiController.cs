using UnityEngine;
using System.Collections;

public class scrTaxiController : MonoBehaviour {
	
	public static scrTaxiController Instance;
    public float fltRotate = 0;
    public float fltMove = 0;
    public float fltLift = 0;
    public float fltCamHor = 0;
    public float fltCamVert = 0;

    public bool blnGearDown = true;
    public Vector3 moveVector;
    public float fltMoveForce;
    public float fltRotateForce;
    public float fltMaxAngularVelocity;
    public float fltMaxVelocity;
    public scrTaxiMotor objTaxiMotorScript;

    public scrMainCam objMainCam;
    private GameObject objLandingGear;
    GUIStyle objUIStyle;

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
		scrTaxiController.Instance = this;
        objLandingGear = GameObject.Find("LandingGear");

        fltMoveForce = 1000;
        fltRotateForce = 1000;
        fltMaxAngularVelocity = 1;
        fltMaxVelocity = 5;

        //Get ref to motor script
        objTaxiMotorScript = gameObject.GetComponent<scrTaxiMotor>();
        
        //Set motor values
        objTaxiMotorScript.fltMaxAngularVelocity = fltMaxAngularVelocity;
        objTaxiMotorScript.fltMaxVelocity = fltMaxVelocity;
        objTaxiMotorScript.fltMoveForce = fltMoveForce;
        objTaxiMotorScript.fltRotateForce = fltRotateForce;

        objUIStyle = new GUIStyle();
        objUIStyle.normal.textColor = Color.black;
        objUIStyle.fontSize = 25;


    }

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        //objMainCam = scrMainCam.Instance;
    }
	
	/// <summary>
	/// 
	/// </summary>
	void Update () {
        getPlayerInput();
	}

    /// <summary>
    /// 
    /// </summary>
    void FixedUpdate()
    {
        //Apply player input to tank motor
        objTaxiMotorScript.movePlayer(fltMove, fltRotate, fltLift, blnGearDown);
    }


    /// <summary>
    /// 
    /// </summary>
    void getPlayerInput()
    {
        if (blnGearDown == false)
        {
            fltRotate = Input.GetAxis(clsGameConstants.strHorizontalAxis);
            fltMove = Input.GetAxis(clsGameConstants.strVerticalAxis);
        }

        if (Input.GetButton(clsGameConstants.strUpButton))
        {
            fltLift = 1;
        }
        else
        {
            fltLift = 0;
        }

        if (Input.GetKeyDown(KeyCode.G) || Input.GetButtonDown(clsGameConstants.strLandingGearButton))
        {
            blnGearDown = !blnGearDown;
        }

        objLandingGear.SetActive(blnGearDown);
		
		
		//This is for the non-Oculus version.
		//Get user imput to rotate the camera arount the outside of the taxi
        //float fltCamHor = Input.GetAxis(clsGameConstants.strHorizontalRightAxis);
        //float fltCamVert = Input.GetAxis(clsGameConstants.strVerticalRightAxis);

        //objMainCam.fltLong += fltCamHor * Time.deltaTime;
        //objMainCam.fltLat += fltCamVert * Time.deltaTime;

        if (Input.GetButton(clsGameConstants.strScreenShotButton))
        {
            Application.CaptureScreenshot("ScreenShot" + Time.realtimeSinceStartup + ".png");
        }
    }

}
