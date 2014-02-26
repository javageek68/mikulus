using UnityEngine;
using System.Collections;

/// <summary>
/// Scr debug slate.
/// </summary>
public class scrDebugSlate : MonoBehaviour {
	
	public TextMesh DebugText;
	
	public scrPassenger psngrScript = null;
    public scrTaxiDriver txiDrvrScript = null;
	public scrTaxiController txiCntlr = null;
	
	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Start () {
        psngrScript = scrPassenger.Instance;
        txiDrvrScript = scrTaxiDriver.Instance;	
		txiCntlr = scrTaxiController.Instance;	
	}
	

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () {
		DebugText.text = "taxi state : " + txiDrvrScript.state.ToString() + "\n" +
			             "     loc   : " + txiDrvrScript.location.ToString() + "\n" +
				         "psgr walk  : " + psngrScript.fltWalking.ToString() + "\n" +
				         "     wave  : " + psngrScript.fltWaving.ToString() + "\n" +
				         "     state : " + psngrScript.stateManager.state.ToString() + "\n" +
				         "     loc   : " + psngrScript.location.ToString();
	}
}
