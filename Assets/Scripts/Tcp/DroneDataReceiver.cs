using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Concurrent;

public class DroneDataReceiver : MonoBehaviour
{
    private ConcurrentQueue<PixhawkSensorData> dataQueue = new ConcurrentQueue<PixhawkSensorData>();
    private void Start()
    {
        EventCenter.Instance.AddEventListener<string>("��������", OnMessageReceived);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener<string>("��������", OnMessageReceived);
    }
    private void Update()
    {
        while (dataQueue.TryDequeue(out PixhawkSensorData data))
        {
            // �����¼���֪ͨ�������ݣ�������������
            EventCenter.Instance.EventTrigger("���ݸ���", data);
        }
    }
    private void OnMessageReceived(string message)
    {
        
            PixhawkSensorData data = JsonConvert.DeserializeObject<PixhawkSensorData>(message);
            if (data != null)
            {
                DroneDataManager.Instance.AddData(data);
                dataQueue.Enqueue(data);
                // ������Ը��� UI �򴫸����ݹ�����
            }
        }
       
    }
