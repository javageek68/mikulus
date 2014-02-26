using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// </summary>
public class scrMainCamPointAtPlayer : MonoBehaviour {

    public static scrMainCamPointAtPlayer Instance = null;
    public scrTaxiLoadScene taxi = null;
    public Camera mainCam = null;

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        scrMainCamPointAtPlayer.Instance = this;
    }

	/// <summary>
	/// 
	/// </summary>
	void Start () {
        taxi = scrTaxiLoadScene.Instance;
        mainCam = Camera.mainCamera;
	}
	
	/// <summary>
	/// 
	/// </summary>
	void Update () {
        mainCam.transform.LookAt(taxi.gameObject.transform.position);
	}
}
