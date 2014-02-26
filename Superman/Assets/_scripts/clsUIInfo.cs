using UnityEngine;

/// <summary>
/// 
/// </summary>
public class clsUIInfo
{
    public int X = 0;
    public int Y = 0;
    public int Width = 100;
    public int Height = 100;
    public string Text = "";

    private Rect UIRect;    

    /// <summary>
    /// 
    /// </summary>
    public clsUIInfo()
    { 
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="text"></param>
    public clsUIInfo(int x, int y, int width, int height, string text)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        UIRect = new Rect(X, Y, Width, Height);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Rect GetUIRect()
    {
        UIRect = new Rect(X, Y, Width, Height);
        return UIRect;
    }

    
}
