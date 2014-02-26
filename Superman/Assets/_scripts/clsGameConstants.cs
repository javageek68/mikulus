using UnityEngine;
using System.Collections;

public class clsGameConstants {
    public enum GameStates{
        Playing,
        Dead,
        GameOver
    }

    /// <summary>
    /// 
    /// </summary>
    public enum enLocations
    {
        Pad1,
        Pad2,
        Pad3,
        Pad4,
        Pad5,
        Pad6,
        Pad7,
        Pad8,
        Pad9,
        Up,
        InTheTaxi,
        Respawning,
        InTransit
    }


    public static GameStates gameState = GameStates.GameOver;

    public static string strPlayerTag = "Player";
    public static string strCarBodyTag = "CarBody";
    public static string strLandingGearTag = "LandingGear";
    public static string strPassengerTag = "Passenger";
    public static string strPad1Tag = "Pad1";
    public static string strPad2Tag = "Pad2";
    public static string strPad3Tag = "Pad3";
    public static string strPad4Tag = "Pad4";
    public static string strPad5Tag = "Pad5";
    public static string strPad6Tag = "Pad6";
    public static string strPad7Tag = "Pad7";
    public static string strPad8Tag = "Pad8";
    public static string strPad9Tag = "Pad9";

    //keys
    public static string strPlayerScoreKey = "PlayerScoreKey";
    public static string strLastLevelKey = "LastLevelKey";
    public static string strEarningsKey = "EarningsKey";
    public static string strLivesKey = "LivesKey";


    public static string strSettingsFile = @"c:\temp\SpaceTaxiSettings.xml";

    public const string strLevel001 = "Level001";
    public const string strLevel002 = "Level002";
    public const string strLevel003 = "Level003";
    public const string strLevelRift = "LevelRift";
    public const string strMetropolis = "Metropolis";
    public const string strFinalScene = "FinalScene";
    public const string strLevelComplete = "LevelComplete";
    public const string strGameOver = "GameOver";
    public const string strMainMenu = "MainMenu";

    public const string strHorizontalAxis = "Horizontal";
    public const string strVerticalAxis = "Vertical";
    public const string strUpButton = "Fire1";
    public const string strLandingGearButton = "Fire2";
    public const string strScreenShotButton = "Fire3";
    public const string strHorizontalRightAxis = "HorizontalRight";
    public const string strVerticalRightAxis = "VerticalRight";


    public static float fltPassengerRespawnTime = 5.00f;
    public static float fltPassengerPauseTime = 2.00f;
    public static float fltLandingVelocity = 0.01f; //if the taxi is moving slower than this, we concider it stopped

}
