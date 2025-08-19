using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using XCharts.Runtime;

public class LineChartPanel : BaseLineChartPanel
{
    private Button btnReturn;

    public override void Init()
    {
        base.Init();
        btnReturn = GetControl<Button>("返回按钮");
        


        btnReturn.onClick.AddListener(() =>
        {

            UIManager.Instance.HidePanel<LineChartPanel>();
            UIManager.Instance.ShowPanel<DataPanel>();

        });

     
    }

    protected override void AddSeries(LineChart chart)
    {
        chart.AddSerie<Line>("横滚 (Roll)");
        chart.AddSerie<Line>("俯仰 (Pitch)");
        chart.AddSerie<Line>("偏航 (Yaw)");

    }

    protected override string GetChartTitle()
    {
        return "无人机姿态数据";
    }

    protected override void UpdateChartData(PixhawkSensorData data)
    {
        chart.AddData(0, data.roll);
        chart.AddData(1, data.pitch);
        chart.AddData(2, data.yaw);
    }
}
