using UnityEngine;
using System.Collections;

public class scrTaxiDriver : MonoBehaviour {
    public static scrTaxiDriver Instance;

	/// <summary>
	/// states.
	/// </summary>
    public enum enStates
    {
        flying,
        landed
    }

    public enStates state;
    public clsGameConstants.enLocations location = clsGameConstants.enLocations.InTransit;

    public int intLives { get; set; }
    public float fltDamage { get; set; }
    public float fltFuel { get; set; }
    public float fltEarnings { get; set; }

    public GameObject LeftDoor;
    public GameObject RightDoor;
    public GameObject WPFrontRight;
    public GameObject WPFrontLeft;
    public GameObject WPBackRight;
    public GameObject WPBackLeft;

    public Material matTaxiTopOn;
    public Material matTaxiTopOff;



    public ParticleEmitter smoke = null;
    public Transform explosionPrefab;

    public scrPassenger psngrScript = null;
    private GameObject objTaxiTop;
	
	/// <summary>
	/// Awake this instance.
	/// </summary>
    void Awake()
    {
        objTaxiTop = GameObject.Find("taxiTop");
        state = enStates.landed;
        scrTaxiDriver.Instance = this;
        smoke = gameObject.GetComponentInChildren<ParticleEmitter>();
        //load the score from the previous level
        fltEarnings = PlayerPrefs.GetFloat(clsGameConstants.strEarningsKey);
        intLives = PlayerPrefs.GetInt(clsGameConstants.strLivesKey);
        reSpawnPlayer();
        //turn the taxi light on
        SetTaxiSign(true);

    }
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {
        psngrScript = scrPassenger.Instance;
	}
	
	// Update is called once per frame
	void Update () {
        if (psngrScript.destination == clsGameConstants.enLocations.Up && gameObject.transform.position.y > 60)
        {
            //store the name of the level the user just completed
            PlayerPrefs.SetString(clsGameConstants.strLastLevelKey, Application.loadedLevelName);
            PlayerPrefs.SetFloat(clsGameConstants.strEarningsKey, fltEarnings);
            PlayerPrefs.SetInt(clsGameConstants.strLivesKey, intLives);

            //load the level complete scene
            Application.LoadLevel(clsGameConstants.strLevelComplete);
        }
	}


    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {

        //print("Taxi contact with " + collision.transform.name);

        
        foreach (ContactPoint contactPoint in collision.contacts)
        {
            //print("contact point on " + contactPoint.thisCollider.name);
            //print("touching " + contactPoint.otherCollider.name);
        }
        
        string strColliderTag = collision.gameObject.tag;

        //find out what hit us
        if ((strColliderTag == clsGameConstants.strPlayerTag))
        {
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contactPoint in collision.contacts)
        {
            Debug.DrawRay(contactPoint.point, contactPoint.normal * 10, Color.white);
           
            //if the car body is being hit, then we need to inflict damage
            if (contactPoint.thisCollider.tag == clsGameConstants.strPlayerTag)
            {
                //the car body is hitting something
                //inflictDamage(Time.deltaTime);
            }
            if (contactPoint.thisCollider.tag == clsGameConstants.strLandingGearTag)
            { 
                //we may have landed.
                //are we moving?
                if (rigidbody.angularVelocity.magnitude < clsGameConstants.fltLandingVelocity && rigidbody.velocity.magnitude < clsGameConstants.fltLandingVelocity )
                { 
                    //the landing gear is touching something and we are not moving.  we have landed
                    state = enStates.landed;
                    //now we need to know where we landed
                    string strLandingSite = contactPoint.otherCollider.tag;

                    //Debug.Log("strlanding site tag = " + strLandingSite);

                    ResolveLocation(strLandingSite);

                    //Debug.Log("resolved taxi location = " + this.location);

                    if (psngrScript.location == clsGameConstants.enLocations.InTheTaxi)
                    {
                        //We're here.  Tell the passenger to get out!!!
                        psngrScript.TellPassengerToGetOut();
                    }
                    else 
                    {
                        //invite the passenger to come aboard
                        psngrScript.InviteToBoard();
                    }
                    
                }
            }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="fare"></param>
    public void PayTaxi(float fare)
    {
        Debug.Log("entered PayTaxi.  about to add " + fare);
        fltEarnings += fare;
    }

    /// <summary>
    /// 
    /// </summary>
    public void SetTaxiSign(bool blnOn)
    {
        if (objTaxiTop != null)
        {
            Debug.Log("setting taxi light" + blnOn);

            if (blnOn == true)
            {
                Debug.Log("setting taxi light to white");
                objTaxiTop.renderer.material = matTaxiTopOn;
            }
            else
            {
                Debug.Log("setting taxi light to gray");
                objTaxiTop.renderer.material = matTaxiTopOff;
            }
            
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    public void inflictDamage(float fltDamageToInflict)
    {
        //TODO: use collision object to determine the part of the taxi that was damaged
        //draw burn marks on the texture.

        fltDamage += fltDamageToInflict;
        setSmoke();
        if (fltDamage >= 100)
        {
            --intLives;
            if (intLives <= 0)
            {
                //Game Over
                PlayerPrefs.SetFloat(clsGameConstants.strPlayerScoreKey, fltEarnings);

                Application.LoadLevel(clsGameConstants.strGameOver);
            }
            else
            {
                Transform temp = Instantiate(explosionPrefab, transform.position, transform.rotation) as Transform;
                reSpawnPlayer();
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fltFuelToBurn"></param>
    public void BurnFuel(float fltFuelToBurn)
    {
        //burn fuel
        fltFuel -= fltFuelToBurn;
        if (fltFuel < 0) fltFuel = 0;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strLandingSite"></param>
    private void ResolveLocation(string strLandingSite)
    {
        if (strLandingSite.StartsWith(clsGameConstants.strPad1Tag)) location = clsGameConstants.enLocations.Pad1;
        else if (strLandingSite.StartsWith(clsGameConstants.strPad2Tag)) location = clsGameConstants.enLocations.Pad2;
        else if (strLandingSite.StartsWith(clsGameConstants.strPad3Tag)) location = clsGameConstants.enLocations.Pad3;
        else if (strLandingSite.StartsWith(clsGameConstants.strPad4Tag)) location = clsGameConstants.enLocations.Pad4;
        else if (strLandingSite.StartsWith(clsGameConstants.strPad5Tag)) location = clsGameConstants.enLocations.Pad5;
        else if (strLandingSite.StartsWith(clsGameConstants.strPad6Tag)) location = clsGameConstants.enLocations.Pad6;
        else if (strLandingSite.StartsWith(clsGameConstants.strPad7Tag)) location = clsGameConstants.enLocations.Pad7;
        else if (strLandingSite.StartsWith(clsGameConstants.strPad8Tag)) location = clsGameConstants.enLocations.Pad8;
        else if (strLandingSite.StartsWith(clsGameConstants.strPad9Tag)) location = clsGameConstants.enLocations.Pad9;
        else location = clsGameConstants.enLocations.InTransit;
    }

    /// <summary>
    /// 
    /// </summary>
    private void setSmoke()
    {
        if (smoke != null)
        {
            smoke.minEmission = fltDamage;
            smoke.maxEmission = fltDamage;
        }
    }

    /// <summary>
    /// When the tank gets reincarnated, it needs to be placed at a random location
    /// </summary>
    public void reSpawnPlayer()
    {
        fltDamage = 0;
        fltFuel = 1000;
        setSmoke();
        //remove any previous momentum before reseting
        gameObject.rigidbody.angularVelocity = Vector3.zero;
        gameObject.rigidbody.velocity = Vector3.zero;

        //reset the orientation - this doesn't work
        //transform.eulerAngles = new Vector3(0, 0, 0);

        //respawn the player in the center of the level
        //float x = Random.Range(-45.0f, 45.0f);
        //float y = 2f;
        //float z = Random.Range(-45.0f, 45.0f);
        //gameObject.transform.position = new Vector3(x, y, z);
    }

}
