using DG.Tweening.Plugins.Core.PathCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using XCharts.Runtime;

public class AutoLineChartPanel : BasePanel
{
    private Button btnReturn;
    private Button btnSidebar;
    private Button btnSidebarReturn;

    [SerializeField] private GameObject SideBarPanel;
    [SerializeField] private Toggle[] toggles;

    private LineChart chart;

    private List<PixhawkSensorData> list = new List<PixhawkSensorData>();


    private void OnEnable()
    {
        LineDataInit();
    }

    public override void Init()
    {
       
        btnReturn = GetControl<Button>("���ذ�ť");
        btnSidebar = GetControl<Button>("�������ť");
        btnSidebarReturn = GetControl<Button>("��������ذ�ť");
        btnAddListener();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        SetIsShowData();
    }

    private void LineDataInit()
    {
        string path = CSVFileManager.Instance.GetPath();
        list = ReadCsvToPixhawk(path);

        chart = gameObject.GetComponent<LineChart>();
        chart.ClearData();

        var title = chart.EnsureChartComponent<Title>();
        title.text = CSVFileManager.Instance.GetFileName(path);

        var xAxis = chart.EnsureChartComponent<XAxis>();
        xAxis.boundaryGap = false;

        AddSeries();

        UpdateChartData();

    }

    private void AddSeries()
    {
        chart.AddSerie<Line>("��� (Roll)");
        chart.AddSerie<Line>("���� (Pitch)");
        chart.AddSerie<Line>("ƫ�� (Yaw)");
    }



    private void UpdateChartData()
    {

        for (int i = 0; i < list.Count; i++)
        {
            chart.AddXAxisData((i+1).ToString());
            chart.AddData(0, list[i].roll);
            chart.AddData(1, list[i].pitch);
            chart.AddData(2, list[i].yaw);
        }
            
        
    }


    private List<PixhawkSensorData> ReadCsvToPixhawk(string filePath)
    {
        var result = new List<PixhawkSensorData>();

        if (!File.Exists(filePath))
        {
            Debug.LogError($"CSV�ļ�������: {filePath}");
            return result;
        }

        using (var reader = new StreamReader(filePath))
        {
            string headerLine = reader.ReadLine(); // ��һ�б�ͷ
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] values = line.Split(',');

                // ����CSV��˳��ӳ�䵽 PixhawkSensorData
                PixhawkSensorData data = new PixhawkSensorData
                {
                    roll = float.Parse(values[1]),
                    pitch = float.Parse(values[2]),
                    yaw = float.Parse(values[3]),

                    // �������CSV��������ӳ��
                };
                result.Add(data);
            }
        }
        return result;
    }


    private void btnAddListener()
    {
        btnReturn.onClick.AddListener(() =>
        {

            UIManager.Instance.HidePanel<AutoLineChartPanel>();
            UIManager.Instance.ShowPanel<HistoryPanel>();

        });

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
