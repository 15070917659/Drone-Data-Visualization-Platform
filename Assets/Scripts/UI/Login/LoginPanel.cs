using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class LoginPanel : BasePanel
{
    private Button btnRegister;//注册按钮
    private Button btnSure;//确定按钮

    private TMP_InputField inputUN;//账号输入框
    private TMP_InputField inputPW;//密码输入框

    private Toggle togPw;//记住密码多选框
    private Toggle togAuto;//自动登录多选框
    public override void Init()
    {
        btnRegister = GetControl<Button>("注册按钮");
        btnSure = GetControl<Button>("确定按钮");
        inputUN = GetControl<TMP_InputField>("账号输入框");
        inputPW = GetControl<TMP_InputField>("密码输入框");
        togPw = GetControl<Toggle>("记住密码");
        togAuto = GetControl<Toggle>("自动登录");
        //点击注册做什么
        btnRegister.onClick.AddListener(() =>
        {
            //显示注册面板
            UIManager.Instance.ShowPanel<RegisterPanel>();
            //隐藏自己
            UIManager.Instance.HidePanel<LoginPanel>(false);
        });
        //点击登录做什么
        btnSure.onClick.AddListener(() =>
        {
            //点击登录后 验证密码是否正确
            //判断输入的账号密码是否合理
            if (inputUN.text.Length <= 6 || inputPW.text.Length <= 6)
            {
                //提示不合法
                TipPanel panel = UIManager.Instance.ShowPanel<TipPanel>(E_UI_Layer.Top);
                //改变内容
                panel.ChangeInfo("账号和密码都必须大于6位");
                return;
            }

                if (LoginMgr.Instance.CheckInfo(inputUN.text, inputPW.text))
                {
                //登录成功
               
                //记录数据
                    LoginMgr.Instance.LoginData.username = inputUN.text;
                    LoginMgr.Instance.LoginData.password = inputPW.text;
                    LoginMgr.Instance.LoginData.rememberpw = togPw.isOn;
                    LoginMgr.Instance.LoginData.autoLogin = togAuto.isOn;
                    
                    LoginMgr.Instance.SaveLoginData();
     
                    //隐藏自己
                    UIManager.Instance.HidePanel<LoginPanel>();

                UIManager.Instance.ShowPanel<DataPanel>(E_UI_Layer.Mid);
                UIManager.Instance.ShowPanel<MainPanel>(E_UI_Layer.Bot);
                // UIManager.Instance.ShowPanel<TipsPanel>(E_UI_Layer.System);

            }
            else
                {
                    //登录失败
                    UIManager.Instance.ShowPanel<TipPanel>(E_UI_Layer.Top).ChangeInfo("账号或密码错误");
                }
            
        });
    
        //点击记住密码 逻辑
        togPw.onValueChanged.AddListener((ison) =>
        {
            //当记住密码取消选中，自动登录也要取消选中
            if (!ison)
            {
                togAuto.isOn = false;
            }


        });
        //点击自动登录逻辑

        togAuto.onValueChanged.AddListener((ison) =>
        {
            //选中自动登录，记住密码没选中应该让他选中
            if (ison)
            {
                togPw.isOn = true;
            }
        });
    }

    public override void ShowMe()
    {
        base.ShowMe();//显示自己时 根据数据 更新面板上的内容

        //得到数据
        LoginData loginData = LoginMgr.Instance.LoginData;
        //初始化面板设置
        //更新两个多选框
        togPw.isOn = loginData.rememberpw;
        togAuto.isOn = loginData.autoLogin;

        //更新账号密码
        inputUN.text = loginData.username;
        //看看是不是选了记住密码来决定是否更新密码
        if(togPw.isOn) 
            inputPW.text = loginData.password;

        if(togAuto.isOn)
        {
            //自动取验证账号密码
            if (LoginMgr.Instance.CheckInfo(inputUN.text, inputPW.text))
            {

                //隐藏自己
                UIManager.Instance.HidePanel<LoginPanel>();
                UIManager.Instance.ShowPanel<DataPanel>(E_UI_Layer.Mid);
                UIManager.Instance.ShowPanel<MainPanel>(E_UI_Layer.Bot);
            }
            else
            {
                //登录失败
                UIManager.Instance.ShowPanel<TipPanel>(E_UI_Layer.Top).ChangeInfo("账号或密码错误");
            }
        }

        }
    
    public void SetInfo(string userName,string password)
    {
        inputPW.text = password;
        inputUN.text = userName;
    }
    
}
