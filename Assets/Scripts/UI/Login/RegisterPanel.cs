using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPanel : BasePanel
{
    private Button btnCancel;//取消按钮
    private Button btnSure;//确定按钮

    private TMP_InputField inputUN;//账号输入框
    private TMP_InputField inputPW;//密码输入框

    public override void Init()
    {

        btnCancel = GetControl<Button>("取消按钮");
        btnSure = GetControl<Button>("确定按钮");
        inputUN = GetControl<TMP_InputField>("账号输入框");
        inputPW = GetControl<TMP_InputField>("密码输入框");


        btnCancel.onClick.AddListener(() =>
        {
            //隐藏自己
            UIManager.Instance.HidePanel<RegisterPanel>();
            //显示登录面板
            UIManager.Instance.ShowPanel<LoginPanel>();
        });

        btnSure.onClick.AddListener(() =>
        {
            //判断输入的账号密码是否合理
            if(inputUN.text.Length < 6 ||  inputPW.text.Length < 6)
            {
                //提示不合法
                TipPanel panel = UIManager.Instance.ShowPanel<TipPanel>(E_UI_Layer.Top);
                //改变内容
                panel.ChangeInfo("账号和密码都必须大于等于6位");
                return;
            }

            //去注册账号密码
            if (LoginMgr.Instance.RegisterUser(inputUN.text, inputPW.text))
            {
                //注册成功
                //显示登录面板
                LoginPanel loginPanel = UIManager.Instance.ShowPanel<LoginPanel>();
                //显示登录面板上的用户名和密码
                loginPanel.SetInfo(inputUN.text,inputPW.text);

                LoginMgr.Instance.ClearLoginData();
              
                //隐藏自己
                UIManager.Instance.HidePanel<RegisterPanel>();

            }
            else
            {
                TipPanel tipPanel = UIManager.Instance.ShowPanel<TipPanel> (E_UI_Layer.Top);
                //改变提示内容、
                tipPanel.ChangeInfo("用户名已存在");

                inputUN.text = "";
                inputPW.text = "";
            }
        });
    }
}
