using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TipPanel : BasePanel
{
    //确定按钮
    private Button btnSure;
    //提示文字
    private TMP_Text txtInfo;

    public override void Init()
    {
        btnSure = GetControl<Button>("确定按钮");
        txtInfo = GetControl<TMP_Text>("提示");

        btnSure.onClick.AddListener(() =>
        {
            //隐藏自己
            UIManager.Instance.HidePanel<TipPanel>();
        });
    }
    /// <summary>
    /// 提示内容改变
    /// </summary>
    /// <param name="info"></param>
    public void ChangeInfo(string info)
    {
        txtInfo.text = info;
    }
}
