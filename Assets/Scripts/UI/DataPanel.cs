
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DataPanel : BasePanel
{
    private Button btnAttitudeData;
    private TMP_Text rolltxt;
    private TMP_Text pitchtxt;
    private TMP_Text yawtxt;
    private TMP_Text height;
    private TMP_Text blackhHeight;
    private TMP_Text speed;
    private TMP_Text blackSpeed;
    private Image leftIamge;
    private Image rightIamge;
    private Image middleIamge;
    private Image middleUpIamge;
    private Image upIamge;
    private Image downIamge;
    private float MoveTime = 0.5f;
    private Coroutine moveCoroutine;

    private Vector2 leftbaseAnchoredPos;
    private Vector2 rightbaseAnchoredPos;
    private Vector2 middlebaseAnchoredPos;
    private Vector2 upbaseAnchoredPos;
    public override void Init()
    {
        btnAttitudeData = GetControl<Button>("姿态数据按钮");

        rolltxt = GetControl<TMP_Text>("横滚角(°)");
        pitchtxt = GetControl<TMP_Text>("俯仰角(°)");
        yawtxt = GetControl<TMP_Text>("航向角(°)");
        height = GetControl<TMP_Text>("高度");
        blackhHeight = GetControl<TMP_Text>("黑条高度");
        speed = GetControl<TMP_Text>("速度");
        blackSpeed = GetControl<TMP_Text>("黑条速度");


        leftIamge = GetControl<Image>("左边刻度");
        rightIamge = GetControl<Image>("右边刻度");
        middleIamge = GetControl<Image>("中间刻度");
        middleUpIamge = GetControl<Image>("红色三角形");
        upIamge = GetControl<Image>("上刻度");
        downIamge = GetControl<Image>("下");
        EventCenter.Instance.AddEventListener<PixhawkSensorData>("数据更新", GetData);

        btnAttitudeData.onClick.AddListener(() =>
        {

            UIManager.Instance.HidePanel<DataPanel>();
            UIManager.Instance.ShowPanel<LineChartPanel>();

        });

        leftbaseAnchoredPos = leftIamge.rectTransform.anchoredPosition;
        rightbaseAnchoredPos = rightIamge.rectTransform.anchoredPosition;
        middlebaseAnchoredPos = middleIamge.rectTransform.anchoredPosition;
        upbaseAnchoredPos = upIamge.rectTransform.anchoredPosition;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float roll = Random.Range(-60f, 60f);
            float pitch = Random.Range(-90f, 90f);
            float yaw = Random.Range(-360f, 360f);
            float h = Random.Range(-30f, 30f);
            float v = Random.Range(-30f, 30f);
            rolltxt.text = "横滚角(°):" + roll.ToString("F2");
            pitchtxt.text = "俯仰角(°):" + pitch.ToString("F2");
            yawtxt.text = "航向角(°):" + yaw.ToString("F2");
            height.text = "高度:" + h.ToString("F2") + " m";
            blackhHeight.text = h.ToString("F2") + " m";
            speed.text = "速度:" + v.ToString("F2") + " m/s";
            blackSpeed.text = v.ToString("F2") + " m/s";
            SetSpeed(v,h,pitch,roll, yaw);
        }
    }


    private void GetData(PixhawkSensorData data)
    {
        rolltxt.text = "横滚角(°):" + data.roll;
        pitchtxt.text = "俯仰角(°):" + data.pitch;
        yawtxt.text = "航向角(°):" + data.yaw;

    }
    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener<PixhawkSensorData>("数据更新", GetData);
    }

    IEnumerator Move(Vector2 leftTargetPos, Vector2 rightTargetPos, Vector2 middleTargetPos, float middleUpTargetAngle, Vector2 upTargetPos)
    {
        Vector2 leftStartPos = leftIamge.rectTransform.anchoredPosition;
        Vector2 rightStartPos = rightIamge.rectTransform.anchoredPosition;
        Vector2 middleStartPos = middleIamge.rectTransform.anchoredPosition;
        Vector2 upStartPos = upIamge.rectTransform.anchoredPosition;
        float startAngle = middleUpIamge.rectTransform.localEulerAngles.z;
        float startAngle1 = downIamge.rectTransform.localEulerAngles.z;
        float elapsed = 0f;//已经经过的时间
        while (elapsed < MoveTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / MoveTime; // 0 → 1
            leftIamge.rectTransform.anchoredPosition = Vector2.Lerp(leftStartPos, leftTargetPos, t);
            rightIamge.rectTransform.anchoredPosition = Vector2.Lerp(rightStartPos, rightTargetPos, t);
            middleIamge.rectTransform.anchoredPosition = Vector2.Lerp(middleStartPos, middleTargetPos, t);
            upIamge.rectTransform.anchoredPosition = Vector2.Lerp(upStartPos, upTargetPos, t);

            // 插值旋转 Z 轴角度
            float angle = Mathf.LerpAngle(startAngle, middleUpTargetAngle, t);
            float angle1 = Mathf.LerpAngle(startAngle1, middleUpTargetAngle/2, t);
            middleUpIamge.rectTransform.localEulerAngles = new Vector3(0, 0, angle);
            downIamge.rectTransform.localEulerAngles = new Vector3(0, 0, angle1);
            yield return null;
        }

        // 最后确保到达目标
        leftIamge.rectTransform.anchoredPosition = leftTargetPos;
        rightIamge.rectTransform.anchoredPosition = rightTargetPos;
        middleIamge.rectTransform.anchoredPosition = middleTargetPos;
        upIamge.rectTransform.anchoredPosition = upTargetPos;
    }

    public void SetSpeed(float speed, float height, float pitch, float angle, float gpsHeading)
    {

        // 计算目标位置（基准位置 + 偏移）
        Vector2 leftTargetPos = leftbaseAnchoredPos + new Vector2(0, -19f * speed);
        Vector2 rightTargetPos = rightbaseAnchoredPos + new Vector2(0, -19f * height);
        Vector2 middleTargetPos = middlebaseAnchoredPos + new Vector2(0, -14.2f * pitch);
        Vector2 upTargetPos = upbaseAnchoredPos + new Vector2(-10.27f * gpsHeading, 0);

        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(Move(leftTargetPos, rightTargetPos, middleTargetPos, angle, upTargetPos));
    }

}

