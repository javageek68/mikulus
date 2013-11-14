using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// </summary>
public class scrDashBoard : MonoBehaviour {

    public TextMesh FareText;
    public TextMesh PassengerText;
    public TextMesh DestinationText;
    public TextMesh LivesText;
    public TextMesh EarningText;
    public GameObject Damage;
    public GameObject FuelNeedle;
    public GameObject LandingGearLight;
    public Material matLandingGearLightOn;
    public Material matLandingGearLightOff;

    public scrPassenger psngrScript = null;
    public scrTaxiDriver txiDrvrScript = null;
	public scrTaxiController txiCntlr = null;

	/// <summary>
	/// 
	/// </summary>
	void Start () {
        //set up instances to our taxi and passenger scripts
        psngrScript = scrPassenger.Instance;
        txiDrvrScript = scrTaxiDriver.Instance;	
		txiCntlr = scrTaxiController.Instance;
	}
	
	/// <summary>
	/// 
	/// </summary>
	void Update () {

        FareText.text = "$" + string.Format("{0:00.00}", psngrScript.fare.fare);
        PassengerText.text = psngrScript.GetPassengerMessage();
        DestinationText.text = psngrScript.destination.ToString();
        LivesText.text = string.Format("X {0:00}", txiDrvrScript.intLives);
        EarningText.text = "$" + string.Format("{0:00.00}", txiDrvrScript.fltEarnings);
		if (txiCntlr.blnGearDown == true)
		{
			LandingGearLight.renderer.material = matLandingGearLightOn;
		}
		else
		{
			LandingGearLight.renderer.material = matLandingGearLightOff;
		}

        //GUI.Label(new Rect(0, intLineSize * 1, Screen.width, intLineSize), psngrScript.GetPassengerMessage(), "label");
        //GUI.Label(new Rect(0, intLineSize * 2, Screen.width, intLineSize), "Fare $ " + string.Format("{0:00.00}", psngrScript.fare.fare), "label");
        //GUI.Label(new Rect(0, intLineSize * 3, Screen.width, intLineSize), "Earnings $ " + string.Format("{0:00.00}", txiDrvrScript.fltEarnings), "label");
        //GUI.Label(new Rect(0, intLineSize * 4, Screen.width, intLineSize), "Passenger location    " + psngrScript.location, "label");
        //GUI.Label(new Rect(Screen.width * 0.5f, intLineSize * 1, Screen.width, intLineSize), "Lives  " + string.Format("{0:00}", txiDrvrScript.intLives), "label");
        //GUI.Label(new Rect(Screen.width * 0.5f, intLineSize * 2, Screen.width, intLineSize), "Damage " + string.Format("{0:00.00}", txiDrvrScript.fltDamage), "label");
        //GUI.Label(new Rect(Screen.width * 0.5f, intLineSize * 3, Screen.width, intLineSize), "Fuel   " + string.Format("{0:00.00}", txiDrvrScript.fltFuel), "label");
        //GUI.Label(new Rect(Screen.width * 0.5f, intLineSize * 4, Screen.width, intLineSize), "Passenger destination " + psngrScript.destination, "label");

	}
}
