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

    //ע������
    private RegisterData registerData;
    public RegisterData RegisterData => registerData;

    private LoginMgr()
    {
        //��ȡ��¼����
        loginData = JsonMgr.Instance.LoadData<LoginData>("LoginData");

        //��ȡע������
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
    //��ע������
    public void SaveRegisterData()
    {
        JsonMgr.Instance.SaveData(RegisterData, "RegisterData");
    }
    //ע�᷽��
    public bool RegisterUser(string username, string password)
    {
        if(registerData.registerInfo.ContainsKey(username))
        {
            return false;
        }
        //�������ע��
        registerData.registerInfo.Add(username, password);
        //���ش��
        SaveRegisterData();
        return true;
    }
    //��֤�Ƿ�Ϸ�
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
