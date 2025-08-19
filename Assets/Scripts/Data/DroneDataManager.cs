using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// 管理无人机数据缓存与CSV导出
/// </summary>
public class DroneDataManager
{
    private static DroneDataManager instance = new DroneDataManager();
    public static DroneDataManager Instance => instance;
    private List<PixhawkSensorData> temp = new List<PixhawkSensorData>();
    private string folderPath;
    private  int autoSaveNum;  // 自动导出CSV缓存个数

    public List<string> exportedFiles = new List<string>();

    public void Init(string saveFolder = "DroneData")
    {
        autoSaveNum = PlayerPrefs.GetInt("autoSaveNum",50);
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        folderPath = Path.Combine(desktopPath, saveFolder);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        Debug.Log($"[DroneDataManager] 数据保存目录: {folderPath}");
    }

    /// <summary>
    /// 添加一条数据到缓存
    /// </summary>
    public void AddData(PixhawkSensorData data)
    {
        temp.Add(data);
        if (temp.Count >= autoSaveNum)
        {
            ExportToCsv();
           
           
        }
    }

    /// <summary>
    /// 导出 CSV 文件（包含 PixhawkSensorData 的所有字段，中文表头）
    /// </summary>
    public void ExportToCsv()
    {
        if (temp.Count == 0) return;

        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string filePath = Path.Combine(folderPath, $"{timestamp}.csv");

        try
        {
            using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                // === 1. 写中文表头 ===
                sw.WriteLine(
                    "时间戳," +
                    "横滚角(°),俯仰角(°),航向角(°)," 
                    //"加速度X(m/s²),加速度Y(m/s²),加速度Z(m/s²)," +
                    //"角速度X(rad/s),角速度Y(rad/s),角速度Z(rad/s)," +
                    //"地磁X(μT),地磁Y(μT),地磁Z(μT)," +
                    //"纬度(°),经度(°),GPS高度(米),GPS地速(m/s),GPS航向角(°),GPS卫星数," +
                    //"气压高度(米),气压(hPa),气温(℃)," +
                    //"测距传感器(米)," +
                    //"电压(V),电流(A),剩余电量(%)" +
                    //",油门(0-100),空速(m/s),地速(m/s),爬升率(m/s),当前方向(°)," +
                    //"是否解锁,飞行模式"
                );

                // === 2. 写每一条数据 ===
                foreach (PixhawkSensorData d in temp)
                {
                    sw.WriteLine(
                        $"=\"{timestamp}\"," +
                        $"{d.roll},{d.pitch},{d.yaw}," //+
                        //$"{d.accelX},{d.accelY},{d.accelZ}," +
                        //$"{d.gyroX},{d.gyroY},{d.gyroZ}," +
                        //$"{d.magX},{d.magY},{d.magZ}," +
                        //$"{d.latitude},{d.longitude},{d.gpsAltitude},{d.gpsSpeed},{d.gpsHeading},{d.gpsSatelliteCount}," +
                        //$"{d.baroAltitude},{d.baroPressure},{d.baroTemperature}," +
                        //$"{d.distanceSensor}," +
                        //$"{d.voltage},{d.current},{d.remaining}," +
                        //$"{d.throttle},{d.airspeed},{d.groundspeed},{d.climbRate},{d.heading}," +
                        //$"{d.isArmed},{d.flightMode}"
                    );
                }
            }
            
            Debug.Log($"[DroneDataManager] CSV 导出成功：{filePath}");
            exportedFiles.Add(filePath);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[DroneDataManager] CSV 导出失败: {ex.Message}");
        }
        finally
        {
            temp.Clear();//清空缓存
        }
    }

    /// <summary>
    /// 手动保存并清空缓存
    /// </summary>
    public void ForceSave()
    {
        ExportToCsv();
        Debug.Log("[DroneDataManager] 手动导出CSV");
    }

    public int GetTempNum()
    {
        return temp.Count;
    }

    public int GetAutoSaveNum()
    {
        return PlayerPrefs.GetInt("autoSaveNum", 50);
    }

    public void SetAutoSaveNum(int autoSaveNum)
    {
        this.autoSaveNum = autoSaveNum;
        PlayerPrefs.SetInt("autoSaveNum", this.autoSaveNum);
        PlayerPrefs.Save();
    }
}
