using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XCharts.Runtime;

public abstract class BaseLineChartPanel : BasePanel
{
    protected LineChart chart;//定义一个线

    protected int xCount = 0;//定义x轴当前位数
    protected const int splitNumber = 10;//定义了一个常量，x轴的刻度会始终为此值

    public override void Init()
    {
        EventCenter.Instance.AddEventListener<PixhawkSensorData>("数据更新", OnDataReceived);//监听数据更新事件
        chart = gameObject.GetComponent<LineChart>();//初始化获得LineChart组件
        chart.ClearData();//清楚之前残留的数据

        var title = chart.EnsureChartComponent<Title>();//定义了标题
        title.text = GetChartTitle();//让子类去命名标题

        var xAxis = chart.EnsureChartComponent<XAxis>();//定义了x轴默认参数
        xAxis.splitNumber = splitNumber;
        xAxis.boundaryGap = false;

        AddSeries(chart);//让子类去添加线

        UpdateXAxisLabels(); //用当前时间预测后续x轴的值

    }
    /// <summary>
    /// 让子类去添加线
    /// </summary>
    /// <param name="chart"></param>
    protected abstract void AddSeries(LineChart chart);

    /// <summary>
    /// 让子类去命名标题
    /// </summary>
    /// <returns></returns>
    protected abstract string GetChartTitle();

    /// <summary>
    /// 让子类去实现更新y轴数据
    /// </summary>
    /// <param name="data"></param>
    protected abstract void UpdateChartData(PixhawkSensorData data);



    /// <summary>
    /// 接收到数据之后做什么
    /// </summary>
    /// <param name="data"></param>
    private void OnDataReceived(PixhawkSensorData data)
    {
        PixhawkSensorData sensorData = data;//定义了一个局部变量存储接收到的无人机数据
        xCount++;
        if (xCount > splitNumber)
        {
            chart.ClearData();//清除之前的数据
            UpdateXAxisLabels();//用当前时间预测后续x轴的值
            xCount = 1;
        }

        
        UpdateChartData(sensorData); //更新y轴数据
    }

    protected void UpdateXAxisLabels()//用当前时间预测后续x轴的值
    {
        DateTime currentTime = DateTime.Now;//当前时间
        for (int i = 0; i < splitNumber; i++)
            chart.AddXAxisData(currentTime.AddSeconds(i).ToString("HH:mm:ss"));
    }


}


