using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System;


public class Control : MonoBehaviour
{

    //get broadcast address  bcast_addr = ~netmask | my_addr;
	public const string kServerArgument = "-server";
    public Texture txtBackground;


	const int
		kPort = 42209,
		kHostConnectionBacklog = 10;


	static Control instance;

    //string strLogFile = "LogFile";
	string message = "Awake";
	Socket socket;
	IPAddress ip;
    bool isServer = false;
    bool blnSendingScreenShot = false;

    public Texture2D txtCommTexture = null;

	public static Control Instance
	{
		get
		{
			if (instance == null)
			{
				instance = (Control)FindObjectOfType (typeof (Control));
			}

			return instance;
		}
	}


	public static Socket Socket
	{
		get
		{
			return Instance.socket;
		}
	}


	void Start ()
	{
		Application.RegisterLogCallbackThreaded (OnLog);

		

		foreach (string argument in System.Environment.GetCommandLineArgs ())
		{
			if (argument == kServerArgument)
			{   
				isServer = true;
				break;
			}
		}

		if (isServer)
		{
            //strLogFile = Application.dataPath + "\\" + DateTime.Now.ToString("HHmmss") + ".txt";
            //strLogFile = @"c:\temp\ServerLog.txt";
            Debug.Log("starting up server");
			if (Host (kPort))
			{
				gameObject.SendMessage ("OnServerStarted");
			}
		}
		else
		{
            //strLogFile = Application.dataPath + "\\" + DateTime.Now.ToString("HHmmss") + ".txt";
            //strLogFile = @"c:\temp\ClientLog.txt";
            Debug.Log("starting up client");
			if (Connect (IP, kPort))
			{
				gameObject.SendMessage ("OnClientStarted", socket);
			}
		}
	}

    void Update()
    {
        if (isServer == false) 
        {
            //Send screen image to device
            StartCoroutine(ScreenshotEncode());
        }
    }

	void OnApplicationQuit ()
	{
		Disconnect ();
	}


	public bool Host (int port)
	{
		Debug.Log ("Hosting on port " + port);

		socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

		try
		{
			socket.Bind (new IPEndPoint (IP, port));
			socket.Listen (kHostConnectionBacklog);
			socket.BeginAccept (new System.AsyncCallback (OnClientConnect), socket);
		}
		catch (System.Exception e)
		{
			Debug.LogError ("Exception when attempting to host (" + port + "): " + e);

			socket = null;

			return false;
		}

		return true;
	}


	public bool Connect (IPAddress ip, int port)
	{
		Debug.Log ("Connecting to " + ip + " on port " + port);

		socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		socket.Connect (new IPEndPoint (ip, port));

		if (!socket.Connected)
		{
			Debug.LogError ("Failed to connect to " + ip + " on port " + port);

			socket = null;
			return false;
		}

		return true;
	}


	void Disconnect ()
	{
		if (socket != null)
		{
			socket.BeginDisconnect (false, new System.AsyncCallback (OnEndHostComplete), socket);
		}
	}


	void OnClientConnect (System.IAsyncResult result)
	{
		Debug.Log ("Handling client connecting");

		try
		{
			gameObject.SendMessage ("OnClientConnected", socket.EndAccept (result));
		}
		catch (System.Exception e)
		{
			Debug.LogError ("Exception when accepting incoming connection: " + e);
		}

		try
		{
			socket.BeginAccept (new System.AsyncCallback (OnClientConnect), socket);
		}
		catch (System.Exception e)
		{
			Debug.LogError ("Exception when starting new accept process: " + e);
		}
	}


	void OnEndHostComplete (System.IAsyncResult result)
	{
		socket = null;
	}


	public IPAddress IP
	{
		get
		{
			if (ip == null)
			{
				ip = (
					from entry in Dns.GetHostEntry (Dns.GetHostName ()).AddressList
						where entry.AddressFamily == AddressFamily.InterNetwork
							select entry
				).FirstOrDefault ();
			}

			return ip;
		}
	}


	void OnGUI ()
	{
        if (isServer == false)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), txtBackground);

            if (GUILayout.Button("Send Screenshot"))
            {
                //Send screen image to device
                StartCoroutine(ScreenshotEncode());
            }
        }
        else
        {
            if (txtCommTexture != null)
            {
                GUI.DrawTexture(new Rect(Screen.width / 2, 0, 100, 100), txtCommTexture);
            }

        }
		GUILayout.Label ("");
		GUILayout.Label (message);
	}

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator ScreenshotEncode()
    {
        byte[] bytTexture;

        Debug.Log("Entered ScreenshotEncode");
        //make sure we dont start a new image tx until the last one is complete
        if (blnSendingScreenShot == false)
        {
            blnSendingScreenShot = true;
            // wait for graphics to render
            yield return new WaitForEndOfFrame();

            // create a texture to pass to encoding
            Texture2D txtScreenCapture = TextureUtilities.CaptureScreenShot();

            yield return 0;

            // scale the texture down
            TextureUtilities.TextureScale.Point(txtScreenCapture, 100, 100);

            yield return 0;

            bytTexture = TextureUtilities.TextureToBytes(txtScreenCapture);

            //Debug.Log("about to test BytesToTexture");
            //for (int i = 0; i < 100; i++)
            //{
            //    txtTest = TextureUtilities.BytesToTexture(100, 100, TextureFormat.RGB24, false, bytTexture);
            //}
            //Debug.Log("test BytesToTexture finished");

            //send the texture to the clients
            if (socket != null)
            {
                
                if (socket.Connected) socket.Send(bytTexture);
            }

            yield return 0;

            //System.Threading.Thread.Sleep(5000);

            // Tell unity to delete the texture, by default it seems to keep hold of it and memory crashes will occur after too many screenshots.
            DestroyObject(txtScreenCapture);
            blnSendingScreenShot = false;
        }
        Debug.Log("Leaving ScreenshotEncode");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="callStack"></param>
    /// <param name="type"></param>
	void OnLog (string message, string callStack, LogType type)
	{
		this.message = message + "\n" + this.message;
        //System.IO.File.AppendAllText(strLogFile, DateTime.Now.ToString("HH:mm:ss") + " " + message + "\n");
	}


}
