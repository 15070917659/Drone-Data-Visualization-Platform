using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    private TMP_InputField inputField;
    private Button btnSure;
    private Button btnQuit;
    private TMP_Text tempNumText;
    private TMP_Text autoSaveText;
    public override void Init()
    {
        inputField = GetControl<TMP_InputField>("输入框");
        btnSure = GetControl<Button>("确定");
        btnQuit = GetControl<Button>("退出登录");
        tempNumText = GetControl<TMP_Text>("缓存数据数量");
        autoSaveText = GetControl<TMP_Text>("自动保存数量");

        autoSaveText.text = "缓存" + DroneDataManager.Instance.GetAutoSaveNum().ToString() + "条数据后将自动保存\r\n";


        EventCenter.Instance.AddEventListener<PixhawkSensorData>("数据更新", (d) =>
        {
            tempNumText.text = "当前已经缓存" + DroneDataManager.Instance.GetTempNum().ToString() + "条数据";

        });//监听数据更新事件



        EventCenter.Instance.AddEventListener("自动保存", () =>
        {
            autoSaveText.text = "缓存" + DroneDataManager.Instance.GetAutoSaveNum().ToString() + "条数据后将自动保存\r\n";

        });//监听自动保存事件

        Debug.Log(inputField.name);
        btnSure.onClick.AddListener(() =>
        {
            int inputFieldText;
            if (inputField.text != "")
            {
                inputFieldText = int.Parse(inputField.text);
            
              
            if (inputFieldText >= 10 && inputFieldText <= 500)
            {
                UIManager.Instance.ShowPanel<TipPanel>(E_UI_Layer.Top).ChangeInfo($"每 {DroneDataManager.Instance.GetAutoSaveNum()} 条数据将自动保存");
                DroneDataManager.Instance.SetAutoSaveNum(inputFieldText);
                EventCenter.Instance.EventTrigger("自动保存");
            }
            else
            {
                UIManager.Instance.ShowPanel<TipPanel>(E_UI_Layer.Top).ChangeInfo("请输入介于 10 到 500 之间的数字");
            } 
            }
            else
            {
                UIManager.Instance.ShowPanel<TipPanel>(E_UI_Layer.Top).ChangeInfo("请输入介于 10 到 500 之间的数字");
            }
        });

        btnQuit.onClick.AddListener(() =>
        {
            LoginMgr.Instance.LoginData.autoLogin = false;
            UIManager.Instance.HidePanel<MainPanel>();
            UIManager.Instance.HidePanel<SettingPanel>();
            UIManager.Instance.ShowPanel<LoginPanel>();
        });





    }

    
}
