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
        btnReturn = GetControl<Button>("���ذ�ť");
        


        btnReturn.onClick.AddListener(() =>
        {

            UIManager.Instance.HidePanel<LineChartPanel>();
            UIManager.Instance.ShowPanel<DataPanel>();

        });

     
    }

    protected override void AddSeries(LineChart chart)
    {
        chart.AddSerie<Line>("��� (Roll)");
        chart.AddSerie<Line>("���� (Pitch)");
        chart.AddSerie<Line>("ƫ�� (Yaw)");

    }

    protected override string GetChartTitle()
    {
        return "���˻���̬����";
    }

    protected override void UpdateChartData(PixhawkSensorData data)
    {
        chart.AddData(0, data.roll);
        chart.AddData(1, data.pitch);
        chart.AddData(2, data.yaw);
    }
}
