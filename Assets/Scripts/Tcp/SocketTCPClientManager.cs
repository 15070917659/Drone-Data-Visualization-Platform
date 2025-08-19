using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

/// <summary>
/// TCP 客户端管理器（单例 + 事件中心）
/// </summary>
public class SocketTCPClientManager
{
    private static SocketTCPClientManager instance = new SocketTCPClientManager();
    public static SocketTCPClientManager Instance => instance;
    private Socket clientSocket;
    private Thread receiveThread;
    private bool isConnected = false;
    public bool IsConnected => isConnected;
    private const string EVENT_CONNECT = "连接成功";
    private const string EVENT_DISCONNECT = "断开连接";
    private const string EVENT_RECEIVE = "接收数据";

    /// <summary>
    /// 连接服务器
    /// </summary>
    public void Connect(string ip, int port)
    {
        if (isConnected) return;

        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        clientSocket.Connect(serverEndPoint);

        isConnected = true;
        Debug.Log($"[SocketTCPClient] 已连接服务器 {ip}:{port}");

        // 触发连接成功事件
        EventCenter.Instance.EventTrigger(EVENT_CONNECT);

        // 开启接收线程
        receiveThread = new Thread(ReceiveLoop);
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    /// <summary>
    /// 接收数据
    /// </summary>
    private void ReceiveLoop()
    {
        byte[] buffer = new byte[4096];

        while (isConnected && clientSocket != null && clientSocket.Connected)
        {
            int bytesRead = clientSocket.Receive(buffer);
            if (bytesRead > 0)
            {
                string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                EventCenter.Instance.EventTrigger(EVENT_RECEIVE, msg);
            }
        }
    }

    /// <summary>
    /// 发送数据
    /// </summary>
    public void Send(string message)
    {
        if (!isConnected || clientSocket == null) return;
        byte[] data = Encoding.UTF8.GetBytes(message);
        clientSocket.Send(data);
    }

    /// <summary>
    /// 断开连接
    /// </summary>
    public void Disconnect()
    {
        if (!isConnected) return;

        isConnected = false;
        if (receiveThread != null && receiveThread.IsAlive)
            receiveThread.Abort();

        if (clientSocket != null && clientSocket.Connected)
        {
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }

        clientSocket = null;
        EventCenter.Instance.EventTrigger(EVENT_DISCONNECT);
        Debug.Log("[SocketTCPClient] 已断开连接");
    }
}
