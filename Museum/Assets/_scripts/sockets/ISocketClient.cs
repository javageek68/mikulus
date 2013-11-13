using System.Net.Sockets;

/// <summary>
/// 
/// </summary>
public interface ISocketClient
{
    void OnClientStarted(Socket socket);
    void OnReceive(SocketRead read, byte[] data);
    void OnError(SocketRead read, System.Exception exception);
    void OnEndHostComplete(System.IAsyncResult result);
}

