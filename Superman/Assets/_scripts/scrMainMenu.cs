using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// </summary>
public class scrMainMenu : MonoBehaviour {

    public Texture txtBackground;

    private int intCenterHor = Screen.width / 2;
    private int intCenterVert = Screen.height / 2;
    private int intButtonWidth = 200;
    private int intButtonHeight = 50;

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        if (Input.GetButton(clsGameConstants.strUpButton))
        {
            clsCommon.StartNewGame();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), txtBackground);

        if (GUI.Button(new Rect(intCenterHor - intButtonWidth / 2, intCenterVert - intButtonHeight / 2, intButtonWidth, intButtonHeight), "Start New Game"))
        {
            clsCommon.StartNewGame();
        }
    }
}
