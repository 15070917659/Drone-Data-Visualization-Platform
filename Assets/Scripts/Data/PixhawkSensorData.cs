using System;
using UnityEngine; // 如果不是在 Unity 中，可移除

[Serializable]
public class PixhawkSensorData
{
    // 时间戳
    public string timestamp; // 格式：yyyy-MM-dd HH:mm:ss.fff

    // === 1. 姿态（Attitude） ===
    public float roll;     // 横滚角（度）
    public float pitch;    // 俯仰角（度）
    public float yaw;      // 航向角（度）

    // === 2. 加速度（m/s²） ===
    public float accelX;
    public float accelY;
    public float accelZ;

    // === 3. 角速度（rad/s） ===
    public float gyroX;
    public float gyroY;
    public float gyroZ;

    // === 4. 地磁（μT） ===
    public float magX;
    public float magY;
    public float magZ;

    // === 5. GPS ===
    public double latitude;     // 纬度（°）
    public double longitude;    // 经度（°）
    public float gpsAltitude;   // GPS 高度（米）
    public float gpsSpeed;      // 地速（m/s）
    public float gpsHeading;    // 航向角（°）
    public int gpsSatelliteCount;

    // === 6. 气压高度 / 温度 ===
    public float baroAltitude;   // 由气压估算的高度（米）
    public float baroPressure;   // 压强（hPa）
    public float baroTemperature; // 温度（℃）

    // === 7. 激光 / 超声波测距 ===
    public float distanceSensor; // 距离（米）

    // === 8. 电池信息 ===
    public float voltage;   // 电压（V）
    public float current;   // 电流（A）
    public float remaining; // 剩余百分比（0~100）

    // === 9. 飞行控制状态 ===
    public float throttle; // 油门输出（0~100）
    public float airspeed; // 空速（m/s）
    public float groundspeed; // 地速（m/s）
    public float climbRate; // 上升速度（m/s）
    public float heading;   // 当前方向（°）

    // === 10. 状态辅助 ===
    public bool isArmed;        // 是否解锁
    public string flightMode;   // 当前飞行模式

    // 构造函数（可选）
    //public PixhawkSensorData()
    //{
    //    timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    //}
}
