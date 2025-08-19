
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
        btnAttitudeData = GetControl<Button>("��̬���ݰ�ť");

        rolltxt = GetControl<TMP_Text>("�����(��)");
        pitchtxt = GetControl<TMP_Text>("������(��)");
        yawtxt = GetControl<TMP_Text>("�����(��)");
        height = GetControl<TMP_Text>("�߶�");
        blackhHeight = GetControl<TMP_Text>("�����߶�");
        speed = GetControl<TMP_Text>("�ٶ�");
        blackSpeed = GetControl<TMP_Text>("�����ٶ�");


        leftIamge = GetControl<Image>("��߿̶�");
        rightIamge = GetControl<Image>("�ұ߿̶�");
        middleIamge = GetControl<Image>("�м�̶�");
        middleUpIamge = GetControl<Image>("��ɫ������");
        upIamge = GetControl<Image>("�Ͽ̶�");
        downIamge = GetControl<Image>("��");
        EventCenter.Instance.AddEventListener<PixhawkSensorData>("���ݸ���", GetData);

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
            rolltxt.text = "�����(��):" + roll.ToString("F2");
            pitchtxt.text = "������(��):" + pitch.ToString("F2");
            yawtxt.text = "�����(��):" + yaw.ToString("F2");
            height.text = "�߶�:" + h.ToString("F2") + " m";
            blackhHeight.text = h.ToString("F2") + " m";
            speed.text = "�ٶ�:" + v.ToString("F2") + " m/s";
            blackSpeed.text = v.ToString("F2") + " m/s";
            SetSpeed(v,h,pitch,roll, yaw);
        }
    }


    private void GetData(PixhawkSensorData data)
    {
        rolltxt.text = "�����(��):" + data.roll;
        pitchtxt.text = "������(��):" + data.pitch;
        yawtxt.text = "�����(��):" + data.yaw;

    }
    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener<PixhawkSensorData>("���ݸ���", GetData);
    }

    IEnumerator Move(Vector2 leftTargetPos, Vector2 rightTargetPos, Vector2 middleTargetPos, float middleUpTargetAngle, Vector2 upTargetPos)
    {
        Vector2 leftStartPos = leftIamge.rectTransform.anchoredPosition;
        Vector2 rightStartPos = rightIamge.rectTransform.anchoredPosition;
        Vector2 middleStartPos = middleIamge.rectTransform.anchoredPosition;
        Vector2 upStartPos = upIamge.rectTransform.anchoredPosition;
        float startAngle = middleUpIamge.rectTransform.localEulerAngles.z;
        float startAngle1 = downIamge.rectTransform.localEulerAngles.z;
        float elapsed = 0f;//�Ѿ�������ʱ��
        while (elapsed < MoveTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / MoveTime; // 0 �� 1
            leftIamge.rectTransform.anchoredPosition = Vector2.Lerp(leftStartPos, leftTargetPos, t);
            rightIamge.rectTransform.anchoredPosition = Vector2.Lerp(rightStartPos, rightTargetPos, t);
            middleIamge.rectTransform.anchoredPosition = Vector2.Lerp(middleStartPos, middleTargetPos, t);
            upIamge.rectTransform.anchoredPosition = Vector2.Lerp(upStartPos, upTargetPos, t);

            // ��ֵ��ת Z ��Ƕ�
            float angle = Mathf.LerpAngle(startAngle, middleUpTargetAngle, t);
            float angle1 = Mathf.LerpAngle(startAngle1, middleUpTargetAngle/2, t);
            middleUpIamge.rectTransform.localEulerAngles = new Vector3(0, 0, angle);
            downIamge.rectTransform.localEulerAngles = new Vector3(0, 0, angle1);
            yield return null;
        }

        // ���ȷ������Ŀ��
        leftIamge.rectTransform.anchoredPosition = leftTargetPos;
        rightIamge.rectTransform.anchoredPosition = rightTargetPos;
        middleIamge.rectTransform.anchoredPosition = middleTargetPos;
        upIamge.rectTransform.anchoredPosition = upTargetPos;
    }

    public void SetSpeed(float speed, float height, float pitch, float angle, float gpsHeading)
    {

        // ����Ŀ��λ�ã���׼λ�� + ƫ�ƣ�
        Vector2 leftTargetPos = leftbaseAnchoredPos + new Vector2(0, -19f * speed);
        Vector2 rightTargetPos = rightbaseAnchoredPos + new Vector2(0, -19f * height);
        Vector2 middleTargetPos = middlebaseAnchoredPos + new Vector2(0, -14.2f * pitch);
        Vector2 upTargetPos = upbaseAnchoredPos + new Vector2(-10.27f * gpsHeading, 0);

        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(Move(leftTargetPos, rightTargetPos, middleTargetPos, angle, upTargetPos));
    }

}

