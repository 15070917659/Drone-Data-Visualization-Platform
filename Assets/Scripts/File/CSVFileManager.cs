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

    public string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\DroneData";//�ļ���·��

    public string LinePath;
    public CSVFileManager()
    {
        EventCenter.Instance.AddEventListener<string>("��ʾ����ͼ", SetPath);
    }

    /// <summary>
    /// �����ļ���������CSV�ļ�
    /// </summary>
    /// <returns></returns>
    public string[] GetAllCSVFiles()
    {
        if (!Directory.Exists(folderPath))
        {
            UnityEngine.Debug.LogWarning("�ļ��в�����: " + folderPath);
            return new string[0];
        }

        return Directory.GetFiles(folderPath, "*.csv");
    }

    /// <summary>
    /// ��ȡ�ļ���
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public string GetFileName(string filePath)
    {
        return Path.GetFileNameWithoutExtension(filePath);
    }

    /// <summary>
    /// ��ָ���ļ������ļ�
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
            UnityEngine.Debug.LogWarning("�ļ�������: " + filePath);
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
