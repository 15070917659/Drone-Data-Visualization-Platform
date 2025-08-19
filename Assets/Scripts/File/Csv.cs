using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using XCharts.Runtime;

public class Csv : MonoBehaviour
{
    private LineChart chart;//����һ����
    [SerializeField] private List<Toggle> toggles = new List<Toggle>();//������һ����ѡ���б�

     private int xCount = 0;//����x�ᵱǰλ��
    [SerializeField] private const int splitNumber = 10;//������һ��������x��Ŀ̶Ȼ�ʼ��Ϊ��ֵ

    private void Start()
    {
        EventCenter.Instance.AddEventListener<PixhawkSensorData>("���ݸ���", GetData);//�������ݸ����¼�
        chart = gameObject.GetComponent<LineChart>();//��ʼ�����LineChart���
        chart.ClearData();//���֮ǰ����������
        var title = chart.EnsureChartComponent<Title>();//�����˱���
        title.text = "���˻�����";
   
        var xAxis = chart.EnsureChartComponent<XAxis>();//������x�����
        xAxis.splitNumber = splitNumber;
        xAxis.boundaryGap = false;
        chart.AddSerie<Line>("�߶�");
        chart.AddSerie<Line>("�ٶ�");
        chart.AddSerie<Line>("����");
 
        DateTime currentTime = DateTime.Now;//�õ�ǰʱ��Ԥ�����x���ֵ
        for (int i = 0; i < splitNumber; i++)
            chart.AddXAxisData(currentTime.AddSeconds(i).ToString("HH:mm:ss"));

    }
   private void GetData(PixhawkSensorData pixhawkSensorData)//�õ����յ������˻�����
    {
        PixhawkSensorData sensorData = pixhawkSensorData;//������һ���ֲ������洢���յ������˻�����
        if (xCount >= splitNumber)
        {
            chart.ClearData();//���֮ǰ������
            DateTime currentTime = DateTime.Now;//�õ�ǰʱ��Ԥ�����x���ֵ
            for (int i = 0; i < splitNumber; i++)
                chart.AddXAxisData(currentTime.AddSeconds(i).ToString("HH:mm:ss"));
            xCount = 0;
        }

        xCount++;

        chart.AddData(0, sensorData.roll);
        chart.AddData(1, sensorData.pitch);
        chart.AddData(2, sensorData.yaw);

    }




}
