using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// </summary>
public class scrTaxiMotor : MonoBehaviour {

    public static scrTaxiMotor Instance;
    
    public float fltMoveForce;
    public float fltRotateForce;
    public float fltMaxAngularVelocity;
    public float fltMaxVelocity;

    public ParticleEmitter ThrustBottom = null;
    public ParticleEmitter ThrustRear = null;
    public ParticleEmitter ThrustRight = null;
    public ParticleEmitter ThrustLeft = null;
    public ParticleEmitter ThrustFront = null;

    public AudioSource audThruster;

    private float fltLevelTolerance = 5.0f;
    private scrTaxiDriver taxiDriverScript;
    

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        scrTaxiMotor.Instance = this;
        fltMoveForce = 1000;
        fltRotateForce = 500;
        fltMaxAngularVelocity = 0.5f;
        fltMaxVelocity = 5;

    }

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        //get a reference of the taxi driver script
        taxiDriverScript = scrTaxiDriver.Instance;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="fltMove"></param>
    /// <param name="fltRotate"></param>
    public void movePlayer(float fltMove, float fltRotate, float fltLift, bool blnGearDown)
    {
        //make sure we didnt fall through the floor
        if (this.transform.position.y < 0) this.transform.Translate(Vector3.up * -this.transform.position.y);

        //turn thrusters on and off
        FireThruster(fltMove, fltRotate, fltLift, taxiDriverScript.fltFuel);

        //we can only control the taxi if we have fuel
        if (taxiDriverScript.fltFuel > 0)
        {
            if (audThruster != null)
            {
                if (!audThruster.isPlaying && (fltMove != 0 || fltRotate != 0 || fltLift != 0))
                {
                    //audThruster.Play();
                }
            }

            //the player can only control horizontal movement when the gear is up
            if (blnGearDown == false)
            {
                //rotate the tank around its Y axis
                if (this.rigidbody.angularVelocity.magnitude < fltMaxAngularVelocity)
                    this.rigidbody.AddTorque(0, fltRotate * fltRotateForce * Time.deltaTime, 0);

                if (this.rigidbody.velocity.magnitude < fltMaxVelocity)
                    this.rigidbody.AddForce(this.rigidbody.transform.forward * fltMove * fltMoveForce * Time.deltaTime);
            }

            this.rigidbody.AddForce(this.rigidbody.transform.up * fltLift * fltMoveForce * Time.deltaTime);

            taxiDriverScript.state = scrTaxiDriver.enStates.flying;
            taxiDriverScript.location = clsGameConstants.enLocations.InTransit;

            //account for the fuel we just used
            taxiDriverScript.BurnFuel(fltMove * Time.deltaTime);
            taxiDriverScript.BurnFuel(fltRotate * Time.deltaTime);
            taxiDriverScript.BurnFuel(fltLift * Time.deltaTime);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private bool IsLevel()
    {
        bool blnLevel = true;

        //check if the tank's x axis is level
        bool blnLevelX = false;
        if (this.transform.rotation.eulerAngles.x < fltLevelTolerance || 
            (360 - this.transform.rotation.eulerAngles.x) < fltLevelTolerance)
            blnLevelX = true;

        //check if the tank's y axis is level
        bool blnLevelZ = false;
        if (this.transform.rotation.eulerAngles.z < fltLevelTolerance ||
            (360 - this.transform.rotation.eulerAngles.z) < fltLevelTolerance)
            blnLevelZ = true;

        //if both axis are level, then the whole tank is level
        if (blnLevelX == true && blnLevelZ == true)
            blnLevel = true;
        else
            blnLevel = false;

        return blnLevel;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fltMove"></param>
    /// <param name="fltRotate"></param>
    /// <param name="fltLift"></param>
    /// <param name="fltFuel"></param>
    private void FireThruster(float fltMove, float fltRotate, float fltLift, float fltFuel)
    {
        SetParticle(ThrustBottom, fltLift > 0 && fltFuel > 0);
        SetParticle(ThrustRear, fltMove > 0 && fltFuel > 0);
        SetParticle(ThrustFront, fltMove < 0 && fltFuel > 0);
        SetParticle(ThrustRight, fltRotate < 0 && fltFuel > 0);
        SetParticle(ThrustLeft, fltRotate > 0 && fltFuel > 0);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="part"></param>
    /// <param name="blnEnabled"></param>
    private void SetParticle(ParticleEmitter part, bool blnEnabled)
    {
        if (part != null)
        {
            if (blnEnabled == true)
            {
                //on
                part.minEmission = 79.0f;
                part.maxEmission = 82.0f;
            }
            else
            {
                //off
                part.minEmission = 0;
                part.maxEmission = 0;
            }
        }
    }

}
