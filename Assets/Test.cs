using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private string ip = "127.0.0.1";
    private float heartbeatInterval = 1f; // 每 1 秒发一次
    private float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.ShowPanel<LoginPanel>();
        Debug.Log(Application.persistentDataPath);
        // UIManager.Instance.ShowPanel<DataPanel>(E_UI_Layer.Mid);
        // UIManager.Instance.ShowPanel<MainPanel>(E_UI_Layer.Bot);
        // UIManager.Instance.ShowPanel<TipsPanel>(E_UI_Layer.System);

         Connect();

    }

    void Update()
    {
        SendHeartBeatInterval();
    }

    private void Connect()
    {
        DroneDataManager.Instance.Init();
        SocketTCPClientManager.Instance.Connect(ip, 8080);
    }


    private void SendHeartbeat()
    {
        SocketTCPClientManager.Instance.Send("HEARTBEAT");
       
    }
    // Update is called once per frame
  

    private void SendHeartBeatInterval()
    {
        if (SocketTCPClientManager.Instance == null) return;
        if (!SocketTCPClientManager.Instance.IsConnected) return;

        timer += Time.deltaTime;
        if (timer >= heartbeatInterval)
        {
            SendHeartbeat();
            timer = 0f;
        }
    }
}
