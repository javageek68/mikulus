using UnityEngine;
using System.Collections;

public class scrPassengerAnimMotor : MonoBehaviour {

    private Animator anim;				// a reference to the animator on the character
    private scrTaxiDriver taxi;         // ref to the taxi
    private scrPassenger passenger;     // ref to the passenger


	/// <summary>
	/// 
	/// </summary>
	void Start () {
        taxi = scrTaxiDriver.Instance;
        passenger = scrPassenger.Instance;
        anim = GetComponent<Animator>();					  
	}
	
	/// <summary>
	/// 
	/// </summary>
	void FixedUpdate ()
    {
        float fltWaving = 0;
        float fltWalking = 0;

        //point the passenger at the taxi
        gameObject.transform.LookAt(taxi.transform);

        //if the taxi has landed on the pad we are on, then stop waving and start walking 
        if (taxi.state == scrTaxiDriver.enStates.landed && taxi.location == passenger.location)
        {
            fltWalking = 1.0f;
            fltWaving = 0.0f;
        }
        else
        {
            fltWalking = 0.0f;
            fltWaving = 1.0f;
        }
        
        //send variables to the animation controller
        anim.SetFloat("walking", fltWalking);
        anim.SetFloat("waving",fltWaving);
	}
}
