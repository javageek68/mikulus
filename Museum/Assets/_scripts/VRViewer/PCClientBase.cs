using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// </summary>
public class PCClientBase : MonoBehaviour, ISocketClient{
    private int intDeviceScreenWidth;
    private int intDeviceScreenHeight;
    private bool blnGotDeviceResolution = false;
    protected ControllerMessages controllerMessages = null;

	/// <summary>
	/// 
	/// </summary>
	public void Start () {
        //set the default screen resolution
        intDeviceScreenWidth = ViewerConstants.ViewerWidth;
        intDeviceScreenHeight = ViewerConstants.ViewerHeight;

        controllerMessages = new ControllerMessages();
	
	}
	
	/// <summary>
	/// 
	/// </summary>
	public void Update () {
        if (blnGotDeviceResolution == true && (Screen.width != intDeviceScreenWidth ||
                                               Screen.height != intDeviceScreenHeight))
        {
            //set the game resolution on the PC to be the same as the device
            Screen.SetResolution(intDeviceScreenWidth, intDeviceScreenWidth, false);
        }

        //send the video to the phone
        StartCoroutine(VRUtilities.SendVideoStream(intDeviceScreenHeight, intDeviceScreenWidth));
	
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="socket"></param>
    public void OnClientStarted(System.Net.Sockets.Socket socket)
    {
        Debug.Log("OnClientStarted() event fired");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="read"></param>
    /// <param name="data"></param>
    public void OnReceive(SocketRead read, byte[] data)
    {
        //Debug.Log("OnReceived() fired bytes " + data.Length);
        VRUtilities.DecodeControllerData(data, ref controllerMessages);

        //make sure we only set the resolution once
        if (blnGotDeviceResolution == false)
        {
            //check to see if the device gave us its dimensions. if so, use them instead of the default
            if (controllerMessages.OptionMessage.ScreenHeight > 0 && controllerMessages.OptionMessage.ScreenWidth > 0)
            {
                intDeviceScreenWidth = controllerMessages.OptionMessage.ScreenWidth;
                intDeviceScreenHeight = controllerMessages.OptionMessage.ScreenHeight;

                intDeviceScreenWidth = TextureUtilities.getSquare(intDeviceScreenWidth, TextureUtilities.sqSquareType.lowest);
                intDeviceScreenHeight = TextureUtilities.getSquare(intDeviceScreenHeight, TextureUtilities.sqSquareType.lowest);

                blnGotDeviceResolution = true;
            }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="read"></param>
    /// <param name="exception"></param>
    public void OnError(SocketRead read, System.Exception exception)
    {
        Debug.Log("OnError() event fired");
        Debug.LogError(exception.ToString());        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="result"></param>
    public void OnEndHostComplete(System.IAsyncResult result)
    {
        Debug.Log("OnEndHostComplete() event fired");
    }
}
