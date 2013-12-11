using UnityEngine;
using System.Collections;

public class AugReality : MonoBehaviour {

    public Material matWeb;
    private WebCamTexture webcamTexture;

    /// <summary>
    /// 
    /// </summary>
	void Start () {
        webcamTexture = new WebCamTexture();
        
        matWeb.mainTexture = webcamTexture;
        webcamTexture.Play();	
	}
	
	/// <summary>
	/// 
	/// </summary>
	void Update () {
	
	}
}
