using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XCharts.Runtime;

public abstract class BaseLineChartPanel : BasePanel
{
    protected LineChart chart;//����һ����

    protected int xCount = 0;//����x�ᵱǰλ��
    protected const int splitNumber = 10;//������һ��������x��Ŀ̶Ȼ�ʼ��Ϊ��ֵ

    public override void Init()
    {
        EventCenter.Instance.AddEventListener<PixhawkSensorData>("���ݸ���", OnDataReceived);//�������ݸ����¼�
        chart = gameObject.GetComponent<LineChart>();//��ʼ�����LineChart���
        chart.ClearData();//���֮ǰ����������

        var title = chart.EnsureChartComponent<Title>();//�����˱���
        title.text = GetChartTitle();//������ȥ��������

        var xAxis = chart.EnsureChartComponent<XAxis>();//������x��Ĭ�ϲ���
        xAxis.splitNumber = splitNumber;
        xAxis.boundaryGap = false;

        AddSeries(chart);//������ȥ�����

        UpdateXAxisLabels(); //�õ�ǰʱ��Ԥ�����x���ֵ

    }
    /// <summary>
    /// ������ȥ�����
    /// </summary>
    /// <param name="chart"></param>
    protected abstract void AddSeries(LineChart chart);

    /// <summary>
    /// ������ȥ��������
    /// </summary>
    /// <returns></returns>
    protected abstract string GetChartTitle();

    /// <summary>
    /// ������ȥʵ�ָ���y������
    /// </summary>
    /// <param name="data"></param>
    protected abstract void UpdateChartData(PixhawkSensorData data);



    /// <summary>
    /// ���յ�����֮����ʲô
    /// </summary>
    /// <param name="data"></param>
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

    protected void UpdateXAxisLabels()//�õ�ǰʱ��Ԥ�����x���ֵ
    {
        DateTime currentTime = DateTime.Now;//��ǰʱ��
        for (int i = 0; i < splitNumber; i++)
            chart.AddXAxisData(currentTime.AddSeconds(i).ToString("HH:mm:ss"));
    }


}


