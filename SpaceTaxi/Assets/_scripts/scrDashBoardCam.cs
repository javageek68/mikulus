using UnityEngine;
using System.Collections;

public class scrDashBoardCam : MonoBehaviour {
    public static scrDashBoardCam Instance = null;

    public scrPassenger psngrScript = null;
    public scrTaxiDriver txiDrvrScript = null;

    public clsUIInfo Fare;
    public clsUIInfo Earnings;
    public clsUIInfo PasngMsg;
    public clsUIInfo Damage;
    public clsUIInfo Taxis;
    public clsUIInfo Fuel;
	
	public GUIText txtPsnger;
		

    private int intLineSize = 21;

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        scrDashBoardCam.Instance = this;
    }

    /// <summary>
    /// 
    /// </summary>
	void Start () {
        //set up instances to our taxi and passenger scripts
        psngrScript = scrPassenger.Instance;
        txiDrvrScript = scrTaxiDriver.Instance;

        Fare = new clsUIInfo();
        Earnings = new clsUIInfo();
        PasngMsg = new clsUIInfo();
        Damage = new clsUIInfo();
        Taxis = new clsUIInfo();
        Fuel = new clsUIInfo();

	}


    /// <summary>
    /// 
    /// </summary>
    void OnGUI()
    {
        if (psngrScript != null)
        {
            PasngMsg.Text = psngrScript.GetPassengerMessage();
			txtPsnger.text = psngrScript.GetPassengerMessage();
            Fare.Text = "Fare $ " + string.Format("{0:00.00}", psngrScript.fare.fare);
        }
        else
        {
            PasngMsg.Text = "Hey, Taxi!";
            Fare.Text = "Fare $100.00";
        }

        if (txiDrvrScript != null)
        {
            Earnings.Text = "Earnings $ " + string.Format("{0:00.00}", txiDrvrScript.fltEarnings);
            Taxis.Text = "Taxis  " + string.Format("{0:00}", txiDrvrScript.intLives);
            Damage.Text = "Damage " + string.Format("{0:00.00}", txiDrvrScript.fltDamage);
            Fuel.Text = "Fuel   " + string.Format("{0:00.00}", txiDrvrScript.fltFuel);
        }
        else
        {
            Earnings.Text = "Earnings $ ";
            Taxis.Text = "Taxis  3";
            Damage.Text = "Damage 0";
            Fuel.Text = "Fuel 45";
        }

        GUI.Label(PasngMsg.GetUIRect(), PasngMsg.Text);
        GUI.Label(Fare.GetUIRect(), Fare.Text);
        GUI.Label(Earnings.GetUIRect() , Earnings.Text);
        GUI.Label(Taxis.GetUIRect(), Taxis.Text);
        GUI.Label(Damage.GetUIRect(), Damage.Text);
        GUI.Label(Fuel.GetUIRect(), Fuel.Text);        


    }
}
