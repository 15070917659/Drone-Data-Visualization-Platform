using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public  class CSVFileManager  
{
    private static CSVFileManager instance = new CSVFileManager();
    public static CSVFileManager Instance => instance;

    public string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\DroneData";//文件夹路径

    public string LinePath;
    public CSVFileManager()
    {
        EventCenter.Instance.AddEventListener<string>("显示折线图", SetPath);
    }

    /// <summary>
    /// 查找文件夹内所有CSV文件
    /// </summary>
    /// <returns></returns>
    public string[] GetAllCSVFiles()
    {
        if (!Directory.Exists(folderPath))
        {
            UnityEngine.Debug.LogWarning("文件夹不存在: " + folderPath);
            return new string[0];
        }

        return Directory.GetFiles(folderPath, "*.csv");
    }

    /// <summary>
    /// 获取文件名
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public string GetFileName(string filePath)
    {
        return Path.GetFileNameWithoutExtension(filePath);
    }

    /// <summary>
    /// 打开指定文件名的文件
    /// </summary>
    /// <param name="filePath"></param>
    public void OpenFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
        }
        else
        {
            UnityEngine.Debug.LogWarning("文件不存在: " + filePath);
        }
    }

    public void SetPath(string path)
    {
        LinePath = path;
        UnityEngine.Debug.Log(LinePath);
    }

    public string GetPath()
    {
        return LinePath;
    }
}
