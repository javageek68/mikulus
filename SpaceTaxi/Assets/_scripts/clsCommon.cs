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
        PlayerPrefs.SetString(clsGameConstants.strLastLevelKey, clsGameConstants.strLevel001);
        PlayerPrefs.SetFloat(clsGameConstants.strEarningsKey, 0);
        PlayerPrefs.SetInt(clsGameConstants.strLivesKey, 3);

        Application.LoadLevel(clsGameConstants.strLevel001);
    }

}

