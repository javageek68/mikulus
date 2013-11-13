using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Net;

/// <summary>
/// 
/// </summary>
public class ServerControl : MonoBehaviour
{
    //The hosting code will create a event handler and pass it 
    //to us here
    public ISocektServer SocketServerHandler = null;

	public List<Socket> clients = new List<Socket> ();
    Socket socket;
    IPAddress ip;
    const int kHostConnectionBacklog = 10;

    public static ServerControl Instance { get; set; }

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
    /// <param name="intPort"></param>
    /// <param name="protocolType"></param>
    /// <param name="socketServerHandler"></param>
    /// <returns></returns>
    public bool Init(int intPort, ProtocolType protocolType, ISocektServer socketServerHandler)
    {
        SocketServerHandler = socketServerHandler;
        return Host(intPort, protocolType);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="intPort"></param>
    /// <returns></returns>
    public bool Host(int intPort, ProtocolType protocolType)
    {
        Debug.Log("Hosting on port " + intPort);

        if (protocolType == ProtocolType.Udp)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, protocolType);
        }
        else
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, protocolType);
        }


        try
        {
            socket.Bind(new IPEndPoint(IPAddress.Parse(ViewerConstants.LocalHost), intPort));
            socket.Listen(kHostConnectionBacklog);
            socket.BeginAccept(new System.AsyncCallback(OnClientConnect), socket);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Exception when attempting to host (" + intPort + "): " + e);

            socket = null;

            return false;
        }

        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="result"></param>
    void OnClientConnect(System.IAsyncResult result)
    {
        Debug.Log("Handling client connecting");
        if (SocketServerHandler != null) SocketServerHandler.OnClientConnected(socket);

        try
        {
            OnClientConnected(socket.EndAccept(result));
        }
        catch (System.Exception e)
        {
            Debug.LogError("Exception when accepting incoming connection: " + e);
        }

        try
        {
            socket.BeginAccept(new System.AsyncCallback(OnClientConnect), socket);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Exception when starting new accept process: " + e);
        }
    }

    /// <summary>
    /// 
    /// </summary>
	void OnServerStarted ()
	{
		Debug.Log ("Server started");
        if (SocketServerHandler != null) SocketServerHandler.OnServerStarted();
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="client"></param>
	void OnClientConnected (Socket client)
	{
		Debug.Log ("Client connected");
		clients.Add (client);
		SocketRead.Begin (client, OnReceive, OnReceiveError);
        if (SocketServerHandler != null) SocketServerHandler.OnClientConnected(client);
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="read"></param>
    /// <param name="data"></param>
	void OnReceive (SocketRead read, byte[] data)
	{
        Debug.Log("message size = " + data.Length);
        if (SocketServerHandler != null) SocketServerHandler.OnReceive(read, data);        
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="read"></param>
    /// <param name="exception"></param>
	void OnReceiveError (SocketRead read, System.Exception exception)
	{
		Debug.LogError ("Receive error: " + exception);
        if (SocketServerHandler != null) SocketServerHandler.OnReceiveError(read, exception);
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    public void SendTxToAll(string data)
    {
        SendTxToAll(Encoding.ASCII.GetBytes(data));
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    public void SendTxToAll(byte data)
    {
        byte[] bytTxData = new byte[1];
        SendTxToAll(bytTxData);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    public void SendTxToAll(byte[] data)
    {
        Debug.Log("message size = " + data.Length);
        foreach (Socket client in clients)
        {
            Debug.Log("sending data to client " + client.Handle.ToString());
            SendTx(client, data);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="client"></param>
    /// <param name="data"></param>
    public void SendTx(Socket client, string data)
    {
        SendTx(client, Encoding.ASCII.GetBytes(data));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="client"></param>
    /// <param name="data"></param>
    public void SendTx(Socket client, byte data)
    {
        byte[] bytTxData = new byte[1];
        SendTx(client, bytTxData);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="client"></param>
    /// <param name="data"></param>
    public void SendTx(Socket client, byte[] data)
    {
        Debug.Log("message size = " + data.Length);
        client.Send(data);
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
        if (SocketServerHandler != null) SocketServerHandler.OnEndHostComplete(result);
        socket = null;
    }

}
