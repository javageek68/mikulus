using UnityEngine;
using System.Collections;

public class CamCntlr : MonoBehaviour {
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


	// Use this for initialization
	void Start () {
        //get ref to game objects
        DudeBase = GameObject.Find("Dude");
        MainCam = GameObject.Find("Main Camera");
        controller = DudeBase.GetComponent<CharacterController>();
	
	}
	
	// Update is called once per frame
	void Update () {
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

	
	}
}
