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

	}
}
