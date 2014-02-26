using UnityEngine;
using System.Collections;

public class scrTaxiLoadScene : MonoBehaviour {
    public static scrTaxiLoadScene Instance;

    //public Material matEarth;
    //public Material matMars;
    //public Material matJupiter;
    //public Material matSaturn;


    public GameObject warpEngines;
    public float speed = 1;
    public float fltTime = 0;
    private string strLastLevel = "";
    private string strNextLevel = "";
    //private string strCurrentSkyboxLevel = "";
    private TrailRenderer warpTrail = null;
    private GameObject mainCam = null;
    private GameObject mainCamRelativePosition = null;

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        mainCam = GameObject.Find("Main Camera");
        mainCamRelativePosition = GameObject.Find("mainCamPosition");
        scrTaxiLoadScene.Instance = this;
    }

	/// <summary>
	/// 
	/// </summary>
	void Start () {
        warpTrail = warpEngines.GetComponentInChildren<TrailRenderer>();
        warpTrail.enabled = false;
        speed = 1;
        strLastLevel = PlayerPrefs.GetString(clsGameConstants.strLastLevelKey);
        strNextLevel = GetNextLevel(strLastLevel);
        //set skybox to current planet
        //SetSkyboxForLevel(strLastLevel);
	}
	
	/// <summary>
	/// 
	/// </summary>
	void Update () {

        fltTime = Time.timeSinceLevelLoad;

        if (fltTime < 1) // - taxi moves at impulse
        {
            //start the warp speed clip
            scrSoundManager.Instance.PlayClip(scrSoundManager.Clip.Warp,false);
            //start off slow
            speed = 1;
            gameObject.transform.Translate(Vector3.forward * Time.deltaTime * speed * -1);

        }
        else if (fltTime < 2) //  - taxi flys away at warp
        {
            //jump to warp speed
            speed = 50;
            warpTrail.enabled = true;
            gameObject.transform.Translate(Vector3.forward * Time.deltaTime * speed * -1);
        }
        else if (fltTime < 3) //  - taxi flys away at warp
        {
            //start the warp bang clip
            scrSoundManager.Instance.PlayClip(scrSoundManager.Clip.WarpBang, false);
            //continue at warp speed
            speed = 50;
            warpTrail.enabled = true;
            gameObject.transform.Translate(Vector3.forward * Time.deltaTime * speed * -1);
        }

        else if (fltTime < 20) //  - taxi flys away at warp
        {
            //play the theme music
            scrSoundManager.Instance.PlayClip(scrSoundManager.Clip.ThemeMusic , false);
            speed = 50;
            warpTrail.enabled = true;
            gameObject.transform.Translate(Vector3.forward * Time.deltaTime * speed * -1);
            //place the main camera in relative position to the taxi
            mainCam.transform.position = mainCamRelativePosition.transform.position;
        }
        else if (fltTime >= 21) //     - load next level
        {
            Application.LoadLevel(strNextLevel);
        }

        //point the camera at the taxi
        mainCam.transform.LookAt(gameObject.transform.position);



	}

    //void SetSkyboxForLevel(string strLevel)
    //{
    //    if (strCurrentSkyboxLevel != strLevel)
    //    {
    //        switch (strLevel)
    //        {
    //            case clsGameConstants.strLevel001:
    //                RenderSettings.skybox = matEarth;
    //                break;
    //            case clsGameConstants.strLevel002:
    //                RenderSettings.skybox = matJupiter;
    //                break;
    //            case clsGameConstants.strLevel003:
    //                RenderSettings.skybox = matSaturn;
    //                break;
    //            default:
    //                RenderSettings.skybox = matEarth;
    //                break;
    //        }

    //        strCurrentSkyboxLevel = strLevel;
    //    }
    //}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strLevel"></param>
    /// <returns></returns>
    string GetNextLevel(string strLevel)
    {
        string strNextLevel = "";

        //for now, we will just play the Oculus Rift level over and over
        strNextLevel = clsGameConstants.strLevelRift;

        //switch (strLevel)
        //{
        //    case clsGameConstants.strLevel001:
        //         strNextLevel = clsGameConstants.strLevel002;
        //        break;
        //    case clsGameConstants.strLevel002:
        //        strNextLevel = clsGameConstants.strLevel003;
        //        break;
        //    case clsGameConstants.strLevel003:
        //        strNextLevel = clsGameConstants.strLevel001;
        //        break;
        //    default:
        //        strNextLevel = clsGameConstants.strLevel001;
        //        break;
        //}


        return strNextLevel;
    }
}
