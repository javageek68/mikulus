using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
class PCClient : PCClientBase
{
    public GameObject DudeBase;
    public GameObject MainCam;

    public Vector3 vctBaseDir;
    public float fltBaseHorDir;
    public float fltCamHorDir;
    public float fltCamVertDir;

    public float fltBaseMaxSpeed = 3.0F;
    public float flrBaseMaxRotateSpeed = 3.0F;
    public float fltBaseSpeed;

    public CharacterController controller;

    private bool blnSendingScreenShot = false;
    private bool blnClientStarted = false;

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        base.Start();
        //get ref to game objects
        DudeBase = GameObject.Find("Dude");
        MainCam = GameObject.Find("Main Camera");
        controller = DudeBase.GetComponent<CharacterController>();
    }

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        base.Update();
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

        fltCamHorDir = fltBaseHorDir + this.controllerMessages.attDeltaX * ViewerConstants.RAD2DEG;
        //for the vertical direction, we need to get the actual value instead of the offset
        fltCamVertDir = this.controllerMessages.AttMessage.Z * ViewerConstants.RAD2DEG + 90;
        //set the camera's y axis angle
        MainCam.transform.localEulerAngles = new Vector3(fltCamVertDir, fltCamHorDir, 0);
        //if the user presses the R (reset) key, recalibrate the camera 
        //with the control device
        if (Input.GetKeyDown(KeyCode.R) == true) this.controllerMessages.SetInitialAttitude();
    }

    /// <summary>
    /// 
    /// </summary>
    void OnGUI()
    {
        if (blnClientStarted == false)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 20, 200, 40), "Start Client"))
            {
                blnClientStarted = true;
                StartCoroutine(VRUtilities.StartVRClient(this));
            }
        }
    }

}

