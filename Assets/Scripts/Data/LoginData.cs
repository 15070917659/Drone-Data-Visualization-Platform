using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginData 
{
    public string username;//用户名
    public string password;//密码
    public bool rememberpw = false;//是否记住密码
    public bool autoLogin = false;//是否自动登录

    public int frontServerID = 0;//服务器
}
