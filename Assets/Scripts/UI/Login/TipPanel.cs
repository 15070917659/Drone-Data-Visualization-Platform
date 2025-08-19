using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TipPanel : BasePanel
{
    //ȷ����ť
    private Button btnSure;
    //��ʾ����
    private TMP_Text txtInfo;

    public override void Init()
    {
        btnSure = GetControl<Button>("ȷ����ť");
        txtInfo = GetControl<TMP_Text>("��ʾ");

        btnSure.onClick.AddListener(() =>
        {
            //�����Լ�
            UIManager.Instance.HidePanel<TipPanel>();
        });
    }
    /// <summary>
    /// ��ʾ���ݸı�
    /// </summary>
    /// <param name="info"></param>
    public void ChangeInfo(string info)
    {
        txtInfo.text = info;
    }
}
