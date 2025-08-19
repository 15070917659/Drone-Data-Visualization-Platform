using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using XCharts.Runtime;

public class Csv : MonoBehaviour
{
    private LineChart chart;//定义一个线
    [SerializeField] private List<Toggle> toggles = new List<Toggle>();//定义了一个单选框列表

     private int xCount = 0;//定义x轴当前位数
    [SerializeField] private const int splitNumber = 10;//定义了一个常量，x轴的刻度会始终为此值

    private void Start()
    {
        EventCenter.Instance.AddEventListener<PixhawkSensorData>("数据更新", GetData);//监听数据更新事件
        chart = gameObject.GetComponent<LineChart>();//初始化获得LineChart组件
        chart.ClearData();//清楚之前残留的数据
        var title = chart.EnsureChartComponent<Title>();//定义了标题
        title.text = "无人机数据";
   
        var xAxis = chart.EnsureChartComponent<XAxis>();//定义了x轴相关
        xAxis.splitNumber = splitNumber;
        xAxis.boundaryGap = false;
        chart.AddSerie<Line>("高度");
        chart.AddSerie<Line>("速度");
        chart.AddSerie<Line>("电量");
 
        DateTime currentTime = DateTime.Now;//用当前时间预测后续x轴的值
        for (int i = 0; i < splitNumber; i++)
            chart.AddXAxisData(currentTime.AddSeconds(i).ToString("HH:mm:ss"));

    }
   private void GetData(PixhawkSensorData pixhawkSensorData)//得到接收到的无人机数据
    {
        PixhawkSensorData sensorData = pixhawkSensorData;//定义了一个局部变量存储接收到的无人机数据
        if (xCount >= splitNumber)
        {
            chart.ClearData();//清除之前的数据
            DateTime currentTime = DateTime.Now;//用当前时间预测后续x轴的值
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
