using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
public class scrPassenger : MonoBehaviour {

    public static scrPassenger Instance;

    public float fltWaving = 0;
    public float fltWalking = 0;

    public bool blnDebug = true;


    public Vector3 PassengerPosition;
    public clsPassengerStateManager stateManager;
    public scrSoundManager soundManager;

    //locations and destinations
    public clsGameConstants.enLocations location = clsGameConstants.enLocations.Respawning;
    public clsGameConstants.enLocations destination = clsGameConstants.enLocations.Respawning;
    public int intAvailableDestinations = 0;
    private Queue<clsGameConstants.enLocations> destinationQueue = null;

    public clsFare fare;

    private string PassengerMessage = "";

    //public scrTaxiDriver taxiDriverScript;
    private System.Random random = null;

    private Animator anim;				// a reference to the animator on the character


    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        Debug.Log("passenger awake");
        scrPassenger.Instance = this;
        anim = GetComponent<Animator>();

        Debug.Log("init psgr state mgr");
        //set the initial state
        stateManager = new clsPassengerStateManager(clsPassengerStateManager.enState.Spawning);
        
        random = new System.Random();

        fare = new clsFare();

        //load state from previous level
        intAvailableDestinations = 4;

        destinationQueue = new Queue<clsGameConstants.enLocations>();
        RandomizeDestinationList();

        fare.initialFare = 100.00f;
        //the passenger may already be on the taxi
        if (location == clsGameConstants.enLocations.InTheTaxi)
        {
            //the passenger is already in the taxi from the last level
            //we just need to select a destination
            SelectNewDestination();
        }
        else
        {
            //the taxi is empty. we need a new destination and a new spawn point
            SelectNewDestination();
            SelectNewSpawnLocation();
        }
        Debug.Log("passenger awake end");
    }

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        //get a ref to the taxi driver script
        //taxiDriverScript = scrTaxiDriver.Instance;
        soundManager = scrSoundManager.Instance;
    }
	
    //void Update () {
    //    ProcessState();
    //}

    /// <summary>
    /// 
    /// </summary>
    void FixedUpdate()
    {
        ProcessState();
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {

        Debug.Log("Passenger contact with " + other.gameObject.transform.name);
        string strColliderTag = other.gameObject.transform.tag;
        Debug.Log("strColliderTag =  " + strColliderTag);
        //find out what hit us
        if ((strColliderTag == clsGameConstants.strPlayerTag))
        {
            //Debug.Log(" ");
            //if (taxiDriverScript.state == scrTaxiDriver.enStates.flying)
            //{
            //    Debug.Log("taxi is flying. so we got ran over ");
            //    //it looks like the player just ran us over
            //    stateManager.SetState(clsPassengerStateManager.enState.Dying);
            //}
            //else if (taxiDriverScript.state == scrTaxiDriver.enStates.landed)
            //{
            //    Debug.Log("taxi is landed. so we just got on board ");
            //    stateManager.SetState(clsPassengerStateManager.enState.Riding);
            //}

            //we have arrived at the door
            //make the passenger invisible so he appears to have gotten in the taxi
            clsHelper.SetObjectVisiblity(false, gameObject);
            stateManager.SetState(clsPassengerStateManager.enState.Riding);
            location = clsGameConstants.enLocations.InTheTaxi;

        }
    }



    /// <summary>
    /// 
    /// </summary>
    void ProcessState()
    {
        switch (stateManager.state)
        {
            case clsPassengerStateManager.enState.Spawning:
                //select a new spawn location
                SelectNewSpawnLocation();
                MoveToSpawnLocation(location.ToString(), scrTaxiDriver.Instance.transform.position);
                clsHelper.SetObjectVisiblity(true, gameObject);
                //set the fare
                fare.InitFare();
                //gameObject.SetActive(true);
                stateManager.SetState(clsPassengerStateManager.enState.Hailing);
                break;
            case clsPassengerStateManager.enState.Hailing:
                //Hey, Taxi!
                SetPassengerMessage("Hey, Taxi!");
                soundManager.PlayClip(scrSoundManager.Clip.HeyTaxi, false);
                fare.UpdateFare();
                //when the taxi lands on the pad, the state will change to Boarding
                break;
            case clsPassengerStateManager.enState.Boarding:
                fare.UpdateFare();
                //Play animation of passenger getting in taxi
                //When the animation finishes, the state will change to Riding
                //StartCoroutine(PlayBoardTaxiAnimation());
                PlayBoardTaxiAnimation2();
                break;
            case clsPassengerStateManager.enState.Riding:
                //turn passengr invisible to make it look like he got into the taxi
                clsHelper.SetObjectVisiblity(false, gameObject);
                //Pad x, please!
                SetPassengerMessage(destination + " please");
                SpeakDestination(false);
                fare.UpdateFare();
                //turn the taxi light off
                scrTaxiDriver.Instance.SetTaxiSign(false);
                //When the taxi lands on the destination pad, the state will be changed to deboarding
                break;
            case clsPassengerStateManager.enState.Deboarding:
                clsHelper.SetObjectVisiblity(true,gameObject);
                //turn the taxi light back on
                scrTaxiDriver.Instance.SetTaxiSign(true);
                Debug.Log("Deboarding.  about to call PayTaxi");
                //Pay the taxi driver the fare if we havent done it yet
                if (fare.Paid == false) scrTaxiDriver.Instance.PayTaxi(fare.GetFare());

                //Say Thanks!
                SetPassengerMessage("Thanks");
                soundManager.PlayClip(scrSoundManager.Clip.Thanks, false);
                //Play animation of passenger getting out of taxi
                //When the animation finishes, the state will change to Leaving
                //StartCoroutine(PlayDeBoardTaxiAnimation());
                PlayDeBoardTaxiAnimation2();

                break;
            case clsPassengerStateManager.enState.Leaving:
                //we made it out of the taxi alive!
                //Select the next destination
                Debug.Log("about to call SelectNewDestination() state is " + stateManager);
                SelectNewDestination();
                //spawn a new passenger
                stateManager.SetState(clsPassengerStateManager.enState.Spawning);
                Debug.Log("changed state " + stateManager);
                break;
            case clsPassengerStateManager.enState.Dying:
                //Taxi runs the passenger over. 
                //Play animation
                //Say Hey!
                SetPassengerMessage("Hey!");
                soundManager.PlayClip(scrSoundManager.Clip.Hey, false);
                //When the animation changes, the state will be changed to Dead
                StartCoroutine(PlayRunDownByTaxiAnimation());
                break;
            case clsPassengerStateManager.enState.Dead:
                //Passenger is reincarnated and appears at a different pad
                //A pause will happen and the state is changed to Spawning
                StartCoroutine(PauseForReincarnation());
                break;
            default:
                break;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    private void SelectNewDestination()
    {
        Debug.Log("Entered SelectNewDestination.  dest left " + destinationQueue.Count);
        //get the next destination from the queue
        if (destinationQueue.Count > 0)
        {
            destination = destinationQueue.Dequeue();
            Debug.Log("selected " + destination);
        }
        else
        {
            //nowhere left to go but up
            destination = clsGameConstants.enLocations.Up;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void SelectNewSpawnLocation()
    {
        //location = clsGameConstants.enLocations.Pad1;
        //return;

        System.Array values = null;

        //add all of the enumerations to the array
        values = clsGameConstants.enLocations.GetValues(typeof(clsGameConstants.enLocations));

        //randomly select the location to spawn from
        location = (clsGameConstants.enLocations)values.GetValue(random.Next(0, intAvailableDestinations));

        //make sure the location and destination are not the same
        while (location == destination)
        {
            //randomly select the location to spawn from
            location = (clsGameConstants.enLocations)values.GetValue(random.Next(0, intAvailableDestinations));
        }    
    }

    /// <summary>
    /// 
    /// </summary>
    private void MoveToSpawnLocation(string strLocationName, Vector3 vctAvoidLocation)
    {
        gameObject.transform.position = GetPadsBestPassengerLocation(strLocationName,  vctAvoidLocation);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strLocationName"></param>
    /// <param name="vctAvoidLocation"></param>
    /// <returns></returns>
    private Vector3 GetPadsBestPassengerLocation(string strLocationName, Vector3 vctAvoidLocation)
    {
        Vector3 PassengerLocation = Vector3.zero;
        //get a ref to the pad we selected
        GameObject pad = GameObject.Find(strLocationName);
        if (pad == null)
        {
            //Debug.LogError("No pad for location " + strLocationName);
        }
        else
        {
            //get a ref to the pad's script
            scrPad padScript = pad.GetComponent<scrPad>();
            //have the script tell us the best location
            PassengerLocation = padScript.GetBestSpawnLocation(vctAvoidLocation);
        }
        return PassengerLocation;    
    }

    /// <summary>
    /// 
    /// </summary>
    private void RandomizeDestinationList()
    {
        System.Array values = null;
        int i = 0;
        int intSwap1 = 0;
        int intSwap2 = 0;

        //add all of the enumerations to the array
        values = clsGameConstants.enLocations.GetValues(typeof(clsGameConstants.enLocations));

        //shuffle the first (intAvailableDestinations) elements array
        for (i = 0; i <= 100; i++)
        {
            intSwap1 = random.Next(0, intAvailableDestinations);
            intSwap2 = random.Next(0, intAvailableDestinations);

            clsGameConstants.enLocations objSwapValue = (clsGameConstants.enLocations)values.GetValue(intSwap1);
            values.SetValue(values.GetValue(intSwap2), intSwap1);
            values.SetValue(objSwapValue, intSwap2);
        }

        //enqueue the first (intAvailableDestinations) elements into the queue
        for (i = 0; i < intAvailableDestinations; i++)
        {
            Debug.Log("add location to queue " + values.GetValue(i));
            destinationQueue.Enqueue((clsGameConstants.enLocations)values.GetValue(i));
        }

        //enqueue the final destination of the level
        destinationQueue.Enqueue(clsGameConstants.enLocations.Up);
        

    }

    /// <summary>
    /// 
    /// </summary>
    public void InviteToBoard()
    {
        //the taxi has landed and invited us to board
        //if we were hailing a taxi, then we will board at this time
        if (stateManager.state == clsPassengerStateManager.enState.Hailing)
        {
            //make sure the taxi landed on the right pad
            if (location == scrTaxiDriver.Instance.location)
            {
                stateManager.SetState(clsPassengerStateManager.enState.Boarding);
            }
            else
            {
                SetPassengerMessage("Hey, Taxi!");
                soundManager.PlayClip(scrSoundManager.Clip.HeyTaxi, false);
            }
        }    
    }

    /// <summary>
    /// 
    /// </summary>
    public void TellPassengerToGetOut()
    {
        //only try to get out if i am already in the taxi
        if (location == clsGameConstants.enLocations.InTheTaxi)
        {
            if (scrTaxiDriver.Instance.location == destination)
            {
                //the taxi has brought us to our destination
                //its time to deboard
                stateManager.SetState(clsPassengerStateManager.enState.Deboarding);
            }
            else
            {
                //remind the taxi driver of where we want to go
                SetPassengerMessage(destination + " please");
                SpeakDestination(true);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void PlayBoardTaxiAnimation2()
    {
        //float fltWaving = 0;
        //float fltWalking = 0;

        if (blnDebug == true)
        {
            Debug.Log("taxi state " + scrTaxiDriver.Instance.state);
            Debug.Log("taxi location " + scrTaxiDriver.Instance.location);
            Debug.Log("psgr location " + location);
        }

        //if the taxi has landed on the pad we are on, then stop waving and start walking 
        if (scrTaxiDriver.Instance.state == scrTaxiDriver.enStates.landed && scrTaxiDriver.Instance.location == location)
        {
            //point the passenger at the taxi
            gameObject.transform.LookAt(scrTaxiDriver.Instance.gameObject.transform.position - Vector3.up * 0.75f );

            fltWalking = 1.0f;
            fltWaving = 0.0f;
        }
        else
        {
            gameObject.transform.LookAt(new Vector3(scrTaxiDriver.Instance.gameObject.transform.position.x, transform.position.y, scrTaxiDriver.Instance.gameObject.transform.position.z));
            fltWalking = 0.0f;
            fltWaving = 1.0f;
        }

        //send variables to the animation controller
        anim.SetFloat("walking", fltWalking);
        anim.SetFloat("waving", fltWaving);

    }


    private void PlayDeBoardTaxiAnimation2()
    {
        //float fltWaving = 0;
        //float fltWalking = 1.0f;

        fltWaving = 0;

        //please remain seated until we have come to a full stop!  :)
        //if the taxi is still moving, dont start the deboard animation
        //also, keep track of if the animation has started.  if the taxi takes off again, we want to
        //complete the animation
        if (scrTaxiDriver.Instance.location == clsGameConstants.enLocations.InTransit && stateManager.PlayingUnboardAnimation == false) return;

        //once the taxi has landed, start the animation
        stateManager.PlayingUnboardAnimation = true;

        //move the passenger location to the taxi location
        location = scrTaxiDriver.Instance.location;


        //find out where on the pad we should walk to
        Vector3 PassengerWalkToLocation = GetPadsBestPassengerLocation(location.ToString(), scrTaxiDriver.Instance.transform.position);


        if (stateManager.InitUnboardAnimation == false)
        {
            stateManager.InitUnboardAnimation = true;
            //place the passenger next to the door closest to the walk to point
            gameObject.transform.position = scrTaxiDriver.Instance.transform.position - Vector3.up * 0.75f;
            //make the passenger visible so he looks like he got out of the cab
            clsHelper.SetObjectVisiblity(true, gameObject);
        }

        //see how far away we are from the door
        float fltDistToWalkToLocation = (PassengerWalkToLocation - gameObject.transform.position).magnitude;


        if (fltDistToWalkToLocation > 0.5)
        {
            //if we are more than half a unit from there, then keep walking
            //point the passenger at the walkto location
            gameObject.transform.LookAt(PassengerWalkToLocation);
            fltWalking = 1;
        }
        else
        {
            //we have arrived at the walk to point
            //make the passenger invisible so he appears to have gotten in the building
            fltWalking = 0;
            gameObject.transform.LookAt(new Vector3(PassengerWalkToLocation.x, transform.position.y, PassengerWalkToLocation.z));
            clsHelper.SetObjectVisiblity(false, gameObject);
            stateManager.SetState(clsPassengerStateManager.enState.Leaving);
        }



        //send variables to the animation controller
        anim.SetFloat("walking", fltWalking);
        anim.SetFloat("waving", fltWaving);
    
    }


    /// <summary>
    /// 
    /// </summary>
    public void PlayBoardTaxiAnimation()
    {
        //figure out which door is closest
        float fltDistToLeftDoor = (scrTaxiDriver.Instance.LeftDoor.transform.position - gameObject.transform.position).magnitude;
        float fltDistToRightDoor = (scrTaxiDriver.Instance.RightDoor.transform.position - gameObject.transform.position).magnitude;
        //default to the left door
        Vector3 vctDoorPosition = scrTaxiDriver.Instance.LeftDoor.transform.position;
        //if the right door is closer, use it
        if (fltDistToRightDoor < fltDistToLeftDoor) vctDoorPosition = scrTaxiDriver.Instance.RightDoor.transform.position;

        //see how far away we are from the door
        float fltDistToDoor = (vctDoorPosition - gameObject.transform.position).magnitude;

        
        if (fltDistToDoor > 0.5)
        {
            //if we are more than half a unit from there, then keep walking
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, vctDoorPosition, Time.deltaTime * 0.7f);
        }
        else
        {
            //we have arrived at the door
            //make the passenger invisible so he appears to have gotten in the taxi
            clsHelper.SetObjectVisiblity(false, gameObject);
            stateManager.SetState(clsPassengerStateManager.enState.Riding);
            location = clsGameConstants.enLocations.InTheTaxi;
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public void PlayDeBoardTaxiAnimation()
    {

        //please remain seated until we have come to a full stop!  :)
        //if the taxi is still moving, dont start the deboard animation
        //also, keep track of if the animation has started.  if the taxi takes off again, we want to
        //complete the animation
        if (scrTaxiDriver.Instance.location == clsGameConstants.enLocations.InTransit && stateManager.PlayingUnboardAnimation == false) return;

        //once the taxi has landed, start the animation
        stateManager.PlayingUnboardAnimation = true;

        //move the passenger location to the taxi location
        location = scrTaxiDriver.Instance.location;
        //find out from the pad where the passenger should walk to
        Vector3 PassengerWalkToLocation = GetPadsBestPassengerLocation(location.ToString(), scrTaxiDriver.Instance.transform.position);

        if (stateManager.InitUnboardAnimation == false)
        {
            stateManager.InitUnboardAnimation = true;        
            //figure out which door is closest
            float fltDistToLeftDoor = (scrTaxiDriver.Instance.LeftDoor.transform.position - PassengerWalkToLocation).magnitude;
            float fltDistToRightDoor = (scrTaxiDriver.Instance.RightDoor.transform.position - PassengerWalkToLocation).magnitude;
            //default to the left door
            Vector3 vctDoorPosition = scrTaxiDriver.Instance.LeftDoor.transform.position;
            //if the right door is closer, use it
            if (fltDistToRightDoor < fltDistToLeftDoor) vctDoorPosition = scrTaxiDriver.Instance.RightDoor.transform.position;
            //place the passenger next to the door closest to the walk to point
            gameObject.transform.position = vctDoorPosition;
            //make the passenger visible so he looks like he got out of the cab
            clsHelper.SetObjectVisiblity(true, gameObject);
            
        }
        //see how far away we are from the door
        float fltDistToWalkToLocation = (PassengerWalkToLocation - gameObject.transform.position).magnitude;


        if (fltDistToWalkToLocation > 0.5)
        {
            //if we are more than half a unit from there, then keep walking
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, PassengerWalkToLocation, Time.deltaTime * 0.7f);
        }
        else
        {
            //we have arrived at the walk to point
            //make the passenger invisible so he appears to have gotten in the building
            clsHelper.SetObjectVisiblity(false, gameObject);
            stateManager.SetState(clsPassengerStateManager.enState.Leaving);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public System.Collections.IEnumerator PlayRunDownByTaxiAnimation()
    {
        //right now, we will just pause and pretend we played an animation
        yield return new WaitForSeconds(clsGameConstants.fltPassengerPauseTime);

        stateManager.SetState(clsPassengerStateManager.enState.Dead);
        yield return 0;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public System.Collections.IEnumerator PauseForReincarnation()
    {

        //make the passenger invisible so he appears to have died
        //gameObject.SetActive(false);
        yield return new WaitForSeconds(clsGameConstants.fltPassengerPauseTime);
        stateManager.SetState(clsPassengerStateManager.enState.Spawning);
        yield return 0;
    }

    /// <summary>
    /// 
    /// </summary>
    private void GetWalkPath(bool blnBoarding, Vector3 PassengerLocationOnPad)
    {

        Vector3 vctDoorPosition = Vector3.zero;
        //figure out which door we can get to
        RaycastHit hitInfo;

        if (Physics.Linecast(PassengerLocationOnPad, scrTaxiDriver.Instance.LeftDoor.transform.position, out hitInfo))
        {
            //we hit something.
            if (Physics.Linecast(PassengerLocationOnPad, scrTaxiDriver.Instance.RightDoor.transform.position, out hitInfo))
            {
                //we hit something.  we cant get to the right door either
            }
            else
            {
                //we didnt hit anything.  the left door looks good
                vctDoorPosition = scrTaxiDriver.Instance.RightDoor.transform.position;
            }
        }
        else
        { 
            //we didnt hit anything.  the left door looks good
            vctDoorPosition = scrTaxiDriver.Instance.LeftDoor.transform.position;
        }

            //figure out which door is closest
            float fltDistToLeftDoor = (scrTaxiDriver.Instance.LeftDoor.transform.position - PassengerLocationOnPad).magnitude;
            float fltDistToRightDoor = (scrTaxiDriver.Instance.RightDoor.transform.position - PassengerLocationOnPad).magnitude;
            //default to the left door
            vctDoorPosition = scrTaxiDriver.Instance.LeftDoor.transform.position;
            //if the right door is closer, use it
            if (fltDistToRightDoor < fltDistToLeftDoor) vctDoorPosition = scrTaxiDriver.Instance.RightDoor.transform.position;
            //place the passenger next to the door closest to the walk to point
            gameObject.transform.position = vctDoorPosition;


        //see how far away we are from the door
        float fltDistToWalkToLocation = (PassengerLocationOnPad - gameObject.transform.position).magnitude;

    }

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="blnVisible"></param>
    //private void SetPassengerVisiblity(bool blnVisible)
    //{
    //    //loop through all child objects in the passenger
    //    for (int i = 0; i < gameObject.transform.childCount; i++)
    //    {
    //        //check to see if it has a render object
    //        if (gameObject.transform.GetChild(i).renderer != null)
    //        {
    //            //set turn render on/off
    //            gameObject.transform.GetChild(i).renderer.enabled = blnVisible;
    //        }
    //    }
        
    
    //}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMsg"></param>
    private void SetPassengerMessage(string strMsg)
    {
        PassengerMessage = strMsg;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string GetPassengerMessage()
    {
        return PassengerMessage;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="OverrideRepeatDelay"></param>
    private void SpeakDestination(bool OverrideRepeatDelay)
    {
        scrSoundManager.Clip SpokenDestination = scrSoundManager.Clip.Up_Please;

        switch (destination)
        {
            case clsGameConstants.enLocations.Pad1:
                SpokenDestination = scrSoundManager.Clip.Pad_1_Please;
                break;
            case clsGameConstants.enLocations.Pad2:
                SpokenDestination = scrSoundManager.Clip.Pad_2_Please;
                break;
            case clsGameConstants.enLocations.Pad3:
                SpokenDestination = scrSoundManager.Clip.Pad_3_Please;
                break;
            case clsGameConstants.enLocations.Pad4:
                SpokenDestination = scrSoundManager.Clip.Pad_4_Please;
                break;
            case clsGameConstants.enLocations.Pad5:
                SpokenDestination = scrSoundManager.Clip.Pad_5_Please;
                break;
            case clsGameConstants.enLocations.Pad6:
                SpokenDestination = scrSoundManager.Clip.Pad_6_Please;
                break;
            case clsGameConstants.enLocations.Pad7:
                SpokenDestination = scrSoundManager.Clip.Pad_7_Please;
                break;
            case clsGameConstants.enLocations.Pad8:
                SpokenDestination = scrSoundManager.Clip.Pad_8_Please;
                break;
            case clsGameConstants.enLocations.Pad9:
                SpokenDestination = scrSoundManager.Clip.Pad_9_Please;
                break;
            case clsGameConstants.enLocations.Up:
                SpokenDestination = scrSoundManager.Clip.Up_Please;
                break;
            default:
                break;
        }

        soundManager.PlayClip(SpokenDestination, OverrideRepeatDelay);
    }




}
