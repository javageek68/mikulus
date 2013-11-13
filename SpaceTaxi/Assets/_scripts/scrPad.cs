using UnityEngine;
using System.Collections;

public class scrPad : MonoBehaviour {

    public GameObject PassengerLocation1;
    public GameObject PassengerLocation2;

    /// <summary>
    /// We are trying to avoid selecting a passenger location on the pad that is too close to where
    /// the taxi landed.  So, we will return the location that is farthest from it.
    /// </summary>
    /// <param name="vctAvoid"></param>
    /// <returns></returns>
    public Vector3 GetBestSpawnLocation(Vector3 vctAvoid)
    {
        //defualt to location 1
        Vector3 vctRetVal = PassengerLocation1.transform.position;

        //get the distances from where the taxi landed
        Vector3 vctDist1 = vctAvoid - PassengerLocation1.transform.position;
        Vector3 vctDist2 = vctAvoid - PassengerLocation2.transform.position;

        //if location 2 is farther away, then use  it
        if (Mathf.Abs(vctDist1.magnitude) > Mathf.Abs( vctDist2.magnitude)) vctRetVal = PassengerLocation2.transform.position;

        return vctRetVal;
    }

}
