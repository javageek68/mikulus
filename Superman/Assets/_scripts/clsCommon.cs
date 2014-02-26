using UnityEngine;

/// <summary>
/// 
/// </summary>
public	class clsCommon
{
    /// <summary>
    /// 
    /// </summary>
    public static void StartNewGame()
    {
        //reset progress
        PlayerPrefs.SetString(clsGameConstants.strLastLevelKey, clsGameConstants.strMetropolis);

        Application.LoadLevel(clsGameConstants.strMetropolis);
    }

}

