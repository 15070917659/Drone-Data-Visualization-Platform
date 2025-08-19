using DG.Tweening.Plugins.Core.PathCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using XCharts.Runtime;

public class LineDataPanel : BasePanel
{
    private Button btnSidebar;
    private Button btnSidebarReturn;

    [SerializeField] private GameObject SideBarPanel;
    [SerializeField] private Toggle[] toggles;

    private LineChart chart;

    private List<PixhawkSensorData> list = new List<PixhawkSensorData>();

    protected int xCount = 0;//定义x轴当前位数
    protected const int splitNumber = 10;//定义了一个常量，x轴的刻度会始终为此值
  

    public override void Init()
    {
        EventCenter.Instance.AddEventListener<PixhawkSensorData>("数据更新", OnDataReceived);//监听数据更新事件
        btnSidebar = GetControl<Button>("侧边栏按钮");
        btnSidebarReturn = GetControl<Button>("侧边栏返回按钮");

        chart = gameObject.GetComponent<LineChart>();//初始化获得LineChart组件
        chart.ClearData();//清楚之前残留的数据

        var title = chart.EnsureChartComponent<Title>();//定义了标题
        title.text = "折线图实时总数据";//命名标题

        var xAxis = chart.EnsureChartComponent<XAxis>();//定义了x轴默认参数
        xAxis.splitNumber = splitNumber;
        xAxis.boundaryGap = false;

        AddSeries();//添加线

        UpdateXAxisLabels(); //用当前时间预测后续x轴的值

        btnAddListener();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        SetIsShowData();
    }

   

    private void AddSeries()
    {
        chart.AddSerie<Line>("横滚 (Roll)");
        chart.AddSerie<Line>("俯仰 (Pitch)");
        chart.AddSerie<Line>("偏航 (Yaw)");
    }

    private void UpdateXAxisLabels()//用当前时间预测后续x轴的值
    {
        DateTime currentTime = DateTime.Now;//当前时间
        for (int i = 0; i < splitNumber; i++)
            chart.AddXAxisData(currentTime.AddSeconds(i).ToString("HH:mm:ss"));
    }

    private  void UpdateChartData(PixhawkSensorData data)
    {
        chart.AddData(0, data.roll);
        chart.AddData(1, data.pitch);
        chart.AddData(2, data.yaw);
    }


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

    private void btnAddListener()
    {

        btnSidebar.onClick.AddListener(() =>
        {

            SideBarPanel.SetActive(true);

        });

        btnSidebarReturn.onClick.AddListener(() =>
        {

            SideBarPanel.SetActive(false);

        });
    }


    private void SetIsShowData()
    {
        for (int i = 0; i < 3; i++)
        {
            chart.series[i].show = toggles[i].isOn;
        }
    }

}
