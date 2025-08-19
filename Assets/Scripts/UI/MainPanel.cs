using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : BasePanel
{
    private Button btnDataPanel; 
    private Button btnHistoryDataPanel; 
    private Button btnLineDataPanel;
    private Button brnSeeting;

    public override void Init()
    {
        btnDataPanel = GetControl<Button>("数据面板按钮");
        btnHistoryDataPanel = GetControl<Button>("历史数据面板按钮");
        btnLineDataPanel = GetControl<Button>("折线图实时数据面板按钮");
        brnSeeting = GetControl<Button>("设置按钮");


        btnDataPanel.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<SettingPanel>();
            UIManager.Instance.HidePanel<LineDataPanel>();
            UIManager.Instance.HidePanel<HistoryPanel>();
            UIManager.Instance.ShowPanel<DataPanel>();
        });

        btnHistoryDataPanel.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<SettingPanel>();
            UIManager.Instance.HidePanel<DataPanel>();
            UIManager.Instance.HidePanel<LineDataPanel>();
            UIManager.Instance.ShowPanel<HistoryPanel>();
        });

        btnLineDataPanel.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<SettingPanel>();
            UIManager.Instance.HidePanel<HistoryPanel>();
            UIManager.Instance.HidePanel<DataPanel>();
            UIManager.Instance.ShowPanel<LineDataPanel>();
        });

        brnSeeting.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<LineDataPanel>();
            UIManager.Instance.HidePanel<HistoryPanel>();
            UIManager.Instance.HidePanel<DataPanel>();
            UIManager.Instance.ShowPanel<SettingPanel>();
        });
    }

   
}
