using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class LoginMgr
{
    private static LoginMgr instance = new LoginMgr();
    public static LoginMgr Instance => instance;
    private LoginData loginData;
    public LoginData LoginData => loginData;

    //注册数据
    private RegisterData registerData;
    public RegisterData RegisterData => registerData;

    private LoginMgr()
    {
        //读取登录数据
        loginData = JsonMgr.Instance.LoadData<LoginData>("LoginData");

        //读取注册数据
        registerData = JsonMgr.Instance.LoadData<RegisterData>("RegisterData");


    }

    public void SaveLoginData()
    {
        JsonMgr.Instance.SaveData(LoginData, "LoginData");
    }
    public void ClearLoginData()
    {
        LoginData.frontServerID = 0;
        LoginData.autoLogin = false;
        LoginData.rememberpw = false;
        SaveLoginData();
    }
    //存注册数据
    public void SaveRegisterData()
    {
        JsonMgr.Instance.SaveData(RegisterData, "RegisterData");
    }
    //注册方法
    public bool RegisterUser(string username, string password)
    {
        if(registerData.registerInfo.ContainsKey(username))
        {
            return false;
        }
        //不存可以注册
        registerData.registerInfo.Add(username, password);
        //本地存粗
        SaveRegisterData();
        return true;
    }
    //验证是否合法
    public bool CheckInfo(string username, string password)
    {
        if (registerData.registerInfo.ContainsKey(username))
        {
            if (registerData.registerInfo[username] == password)
            {
                return true;         
            }
        }
        return false;
    }

}
