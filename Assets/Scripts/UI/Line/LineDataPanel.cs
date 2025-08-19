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

    protected int xCount = 0;//����x�ᵱǰλ��
    protected const int splitNumber = 10;//������һ��������x��Ŀ̶Ȼ�ʼ��Ϊ��ֵ
  

    public override void Init()
    {
        EventCenter.Instance.AddEventListener<PixhawkSensorData>("���ݸ���", OnDataReceived);//�������ݸ����¼�
        btnSidebar = GetControl<Button>("�������ť");
        btnSidebarReturn = GetControl<Button>("��������ذ�ť");

        chart = gameObject.GetComponent<LineChart>();//��ʼ�����LineChart���
        chart.ClearData();//���֮ǰ����������

        var title = chart.EnsureChartComponent<Title>();//�����˱���
        title.text = "����ͼʵʱ������";//��������

        var xAxis = chart.EnsureChartComponent<XAxis>();//������x��Ĭ�ϲ���
        xAxis.splitNumber = splitNumber;
        xAxis.boundaryGap = false;

        AddSeries();//�����

        UpdateXAxisLabels(); //�õ�ǰʱ��Ԥ�����x���ֵ

        btnAddListener();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        SetIsShowData();
    }

   

    private void AddSeries()
    {
        chart.AddSerie<Line>("��� (Roll)");
        chart.AddSerie<Line>("���� (Pitch)");
        chart.AddSerie<Line>("ƫ�� (Yaw)");
    }

    private void UpdateXAxisLabels()//�õ�ǰʱ��Ԥ�����x���ֵ
    {
        DateTime currentTime = DateTime.Now;//��ǰʱ��
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
        PixhawkSensorData sensorData = data;//������һ���ֲ������洢���յ������˻�����
        xCount++;
        if (xCount > splitNumber)
        {
            chart.ClearData();//���֮ǰ������
            UpdateXAxisLabels();//�õ�ǰʱ��Ԥ�����x���ֵ
            xCount = 1;
        }


        UpdateChartData(sensorData); //����y������
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
