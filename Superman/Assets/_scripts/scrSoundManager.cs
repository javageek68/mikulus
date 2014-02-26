using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// </summary>
public class scrSoundManager : MonoBehaviour {

    public static scrSoundManager Instance;

    public AudioSource audHeyTaxi;
    public AudioSource audHey;
    public AudioSource audThanks;
    public AudioSource aud1;
    public AudioSource aud2;
    public AudioSource aud3;
    public AudioSource aud4;
    public AudioSource aud5;
    public AudioSource aud6;
    public AudioSource aud7;
    public AudioSource aud8;
    public AudioSource aud9;
    public AudioSource audUp;
    public AudioSource audWarp;
    public AudioSource audWarpBang;
    public AudioSource audThemeMusic;

    /// <summary>
    /// 
    /// </summary>
    public enum Clip
    { 
        HeyTaxi,
        Thanks,
        Hey,
        Pad_1_Please,
        Pad_2_Please,
        Pad_3_Please,
        Pad_4_Please,
        Pad_5_Please,
        Pad_6_Please,
        Pad_7_Please,
        Pad_8_Please,
        Pad_9_Please,
        Up_Please,
        Warp,
        WarpBang,
        ThemeMusic
    }

    private Clip lastClip = Clip.Up_Please;
    //private float fltLastTimeSpoken = 0;
    //private float fltMinRepeatTime = 100;

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        scrSoundManager.Instance = this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="OverrideRepeatDelay"></param>
    public void PlayClip(Clip clip, bool OverrideRepeatDelay)
    {
        //Debug.Log("requested phrase " + phrase);

        if (clip == lastClip)
        { 
            //we already said that!
        }
        else
        {
            PlayClip(clip);
            lastClip = clip;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="phrase"></param>
    private void PlayClip(Clip phrase)
    {
        switch (phrase)
        {
            case Clip.HeyTaxi:
                audHeyTaxi.Play();
                break;
            case Clip.Thanks:
                audThanks.Play();
                break;
            case Clip.Hey:
                audHey.Play();
                break;
            case Clip.Pad_1_Please:
                aud1.Play();
                break;
            case Clip.Pad_2_Please:
                aud2.Play();
                break;
            case Clip.Pad_3_Please:                
                aud3.Play();
                break;
            case Clip.Pad_4_Please:
                aud4.Play();
                break;
            case Clip.Pad_5_Please:
                aud5.Play();
                break;
            case Clip.Pad_6_Please:
                aud6.Play();
                break;
            case Clip.Pad_7_Please:
                aud7.Play();
                break;
            case Clip.Pad_8_Please:
                aud8.Play();
                break;
            case Clip.Pad_9_Please:
                aud9.Play();
                break;
            case Clip.Up_Please:
                audUp.Play();
                break;
            case Clip.Warp:
                audWarp.Play();
                break;
            case Clip.WarpBang:
                audWarpBang.Play();
                break;
            case Clip.ThemeMusic:
                audThemeMusic.Play();
                break;

            default:
                break;
        }

        //record the last phrase we spoke and when we spoke it
        lastClip = phrase;
        //fltLastTimeSpoken = Time.realtimeSinceStartup;
    }
}
