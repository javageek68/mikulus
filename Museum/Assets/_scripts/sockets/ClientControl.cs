using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;

/// <summary>
/// 
/// </summary>
public class ClientControl : MonoBehaviour
    {

    public static ClientControl Instance { get; set; }
    public ISocketClient SocketClientEventHandler = null;
	Socket socket;

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    /// <param name="protocolType"></param>
    /// <param name="socketClientEventHandler"></param>
    /// <returns></returns>
    public bool Init(string ip, int port, ProtocolType protocolType, ISocketClient socketClientEventHandler)
    {
        SocketClientEventHandler = socketClientEventHandler;
        IPAddress objIP = IPAddress.Parse(ip);
        return Connect(objIP, port, protocolType);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool IsConnected()
    {
        bool blnRetVal = false;

        if (socket != null) blnRetVal = socket.Connected;

        return blnRetVal;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    /// <returns></returns>
    public bool Connect(string ip, int port, ProtocolType protocolType)
    {
        IPAddress objIP = IPAddress.Parse(ip);
        return Connect(objIP, port, protocolType);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    /// <returns></returns>
    public bool Connect(IPAddress ip, int port, ProtocolType protocolType)
    {
        bool blnRetVal = true;

        Debug.Log("Connecting to " + ip + " on port " + port);

        try
        {
            if (protocolType == ProtocolType.Udp)
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, protocolType);
            }
            else
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, protocolType);
            }

            socket.Connect(new IPEndPoint(ip, port));

            if (socket.Connected)
            {
                OnClientStarted(socket);
            }
            else
            {
                Debug.LogError("Failed to connect to " + ip + " on port " + port);
                socket = null;
                blnRetVal = false;
            }
        }
        catch (System.Exception e)
        {
            blnRetVal = false;
            Debug.LogError(e.ToString());
        }


        return blnRetVal;
    }

    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="socket"></param>
	void OnClientStarted (Socket socket)
	{
		Debug.Log ("Client started");
        if (SocketClientEventHandler != null) SocketClientEventHandler.OnClientStarted(socket);
		this.socket = socket;
		SocketRead.Begin (socket, OnReceive, OnError);
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="read"></param>
    /// <param name="data"></param>
	void OnReceive (SocketRead read, byte[] data)
	{
		//Debug.Log (Encoding.ASCII.GetString (data, 0, data.Length));
        if (SocketClientEventHandler != null) SocketClientEventHandler.OnReceive(read, data);
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="read"></param>
    /// <param name="exception"></param>
	void OnError (SocketRead read, System.Exception exception)
	{
		Debug.LogError ("Receive error: " + exception);
        if (SocketClientEventHandler != null) SocketClientEventHandler.OnError(read, exception);
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    public void SendTx(string data)
    {
        SendTx(Encoding.ASCII.GetBytes(data));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    public void SendTx(byte data)
    {
        byte[] bytTxData = new byte[1];
        SendTx(bytTxData);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    public void SendTx(byte[] data)
    {
        Debug.Log("SendTx() message size = " + data.Length);
        if (socket == null)
        {
            Debug.LogError("socket is null");
        }
        else
        {
            if (socket.Connected) 
            {
                socket.Send(data);  
            }
            else
            {
                Debug.LogError("socket not connected");
            }
        }
        
    }


    /// <summary>
    /// 
    /// </summary>
    public void Disconnect()
    {
        if (socket != null)
        {
            socket.BeginDisconnect(false, new System.AsyncCallback(OnEndHostComplete), socket);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="result"></param>
    void OnEndHostComplete(System.IAsyncResult result)
    {
        if (SocketClientEventHandler != null) SocketClientEventHandler.OnEndHostComplete(result);
        socket = null;
    }


}
