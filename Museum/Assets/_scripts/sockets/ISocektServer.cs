using System.Net.Sockets;

/// <summary>
/// 
/// </summary>
public	interface ISocektServer
{
    void OnServerStarted();
    void OnClientConnected(Socket client);
    void OnReceive(SocketRead read, byte[] data);
    void OnReceiveError(SocketRead read, System.Exception exception);
    void OnEndHostComplete(System.IAsyncResult result);
}

