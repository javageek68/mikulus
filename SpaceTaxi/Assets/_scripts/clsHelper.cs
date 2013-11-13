using UnityEngine;

/// <summary>
/// 
/// </summary>
public	class clsHelper
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="blnVisible"></param>
    /// <param name="gameObject"></param>
    public static void SetObjectVisiblity(bool blnVisible, GameObject gameObject)
    {
        //loop through all child objects in the passenger
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            //check to see if it has a render object
            if (gameObject.transform.GetChild(i).renderer != null)
            {
                //set turn render on/off
                gameObject.transform.GetChild(i).renderer.enabled = blnVisible;
            }
        }
    }

}

