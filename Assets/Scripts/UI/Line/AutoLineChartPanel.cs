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
       
        btnReturn = GetControl<Button>("返回按钮");
        btnSidebar = GetControl<Button>("侧边栏按钮");
        btnSidebarReturn = GetControl<Button>("侧边栏返回按钮");
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
        chart.AddSerie<Line>("横滚 (Roll)");
        chart.AddSerie<Line>("俯仰 (Pitch)");
        chart.AddSerie<Line>("偏航 (Yaw)");
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
            Debug.LogError($"CSV文件不存在: {filePath}");
            return result;
        }

        using (var reader = new StreamReader(filePath))
        {
            string headerLine = reader.ReadLine(); // 第一行表头
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] values = line.Split(',');

                // 根据CSV列顺序映射到 PixhawkSensorData
                PixhawkSensorData data = new PixhawkSensorData
                {
                    roll = float.Parse(values[1]),
                    pitch = float.Parse(values[2]),
                    yaw = float.Parse(values[3]),

                    // 按照你的CSV列数继续映射
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
