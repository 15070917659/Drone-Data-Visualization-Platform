using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Concurrent;

public class DroneDataReceiver : MonoBehaviour
{
    private ConcurrentQueue<PixhawkSensorData> dataQueue = new ConcurrentQueue<PixhawkSensorData>();
    private void Start()
    {
        EventCenter.Instance.AddEventListener<string>("接收数据", OnMessageReceived);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener<string>("接收数据", OnMessageReceived);
    }
    private void Update()
    {
        while (dataQueue.TryDequeue(out PixhawkSensorData data))
        {
            // 触发事件，通知有新数据，传递最新数据
            EventCenter.Instance.EventTrigger("数据更新", data);
        }
    }
    private void OnMessageReceived(string message)
    {
        
            PixhawkSensorData data = JsonConvert.DeserializeObject<PixhawkSensorData>(message);
            if (data != null)
            {
                DroneDataManager.Instance.AddData(data);
                dataQueue.Enqueue(data);
                // 这里可以更新 UI 或传给数据管理器
            }
        }
       
    }
