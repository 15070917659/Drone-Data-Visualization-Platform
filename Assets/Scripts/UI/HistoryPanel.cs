using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HistoryPanel : BasePanel
{
    private Button btnRefresh;
    private string[] csvFiles;
    [SerializeField] private Transform content;

    private void OnEnable()
    {
        csvFiles = CSVFileManager.Instance.GetAllCSVFiles();
        ClearFiles();
        ShowFiles();
    }

    public override void Init()
    {
        csvFiles = CSVFileManager.Instance.GetAllCSVFiles();
        ClearFiles();
        ShowFiles();

        btnRefresh = GetControl<Button>("刷新按钮");

       

        btnRefresh.onClick.AddListener(() =>
        {
            csvFiles = CSVFileManager.Instance.GetAllCSVFiles();
            ClearFiles();
            ShowFiles();

        });

       
    }

   
    private void ShowFiles()
    {
        foreach (string file in csvFiles)
        {
            GameObject CSVFile = Instantiate(Resources.Load<GameObject>("UI/CSVFile"), content,false);

            CSVFile csvFile = CSVFile.GetComponent<CSVFile>();
            string fileName = CSVFileManager.Instance.GetFileName(file);
            csvFile.SetFileName(fileName);

            string path = file;
            csvFile.btnOpen.onClick.AddListener(() =>
            {
                CSVFileManager.Instance.OpenFile(path);
            });

            csvFile.btnShow.onClick.AddListener(() =>
            {

                EventCenter.Instance.EventTrigger("显示折线图", path);
                UIManager.Instance.HidePanel<HistoryPanel>();
                UIManager.Instance.ShowPanel<AutoLineChartPanel>();
                

            });
        }
    }

    private void ClearFiles()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }

}
