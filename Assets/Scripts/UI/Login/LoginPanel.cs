using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class LoginPanel : BasePanel
{
    private Button btnRegister;//ע�ᰴť
    private Button btnSure;//ȷ����ť

    private TMP_InputField inputUN;//�˺������
    private TMP_InputField inputPW;//���������

    private Toggle togPw;//��ס�����ѡ��
    private Toggle togAuto;//�Զ���¼��ѡ��
    public override void Init()
    {
        btnRegister = GetControl<Button>("ע�ᰴť");
        btnSure = GetControl<Button>("ȷ����ť");
        inputUN = GetControl<TMP_InputField>("�˺������");
        inputPW = GetControl<TMP_InputField>("���������");
        togPw = GetControl<Toggle>("��ס����");
        togAuto = GetControl<Toggle>("�Զ���¼");
        //���ע����ʲô
        btnRegister.onClick.AddListener(() =>
        {
            //��ʾע�����
            UIManager.Instance.ShowPanel<RegisterPanel>();
            //�����Լ�
            UIManager.Instance.HidePanel<LoginPanel>(false);
        });
        //�����¼��ʲô
        btnSure.onClick.AddListener(() =>
        {
            //�����¼�� ��֤�����Ƿ���ȷ
            //�ж�������˺������Ƿ����
            if (inputUN.text.Length <= 6 || inputPW.text.Length <= 6)
            {
                //��ʾ���Ϸ�
                TipPanel panel = UIManager.Instance.ShowPanel<TipPanel>(E_UI_Layer.Top);
                //�ı�����
                panel.ChangeInfo("�˺ź����붼�������6λ");
                return;
            }

                if (LoginMgr.Instance.CheckInfo(inputUN.text, inputPW.text))
                {
                //��¼�ɹ�
               
                //��¼����
                    LoginMgr.Instance.LoginData.username = inputUN.text;
                    LoginMgr.Instance.LoginData.password = inputPW.text;
                    LoginMgr.Instance.LoginData.rememberpw = togPw.isOn;
                    LoginMgr.Instance.LoginData.autoLogin = togAuto.isOn;
                    
                    LoginMgr.Instance.SaveLoginData();
     
                    //�����Լ�
                    UIManager.Instance.HidePanel<LoginPanel>();

                UIManager.Instance.ShowPanel<DataPanel>(E_UI_Layer.Mid);
                UIManager.Instance.ShowPanel<MainPanel>(E_UI_Layer.Bot);
                // UIManager.Instance.ShowPanel<TipsPanel>(E_UI_Layer.System);

            }
            else
                {
                    //��¼ʧ��
                    UIManager.Instance.ShowPanel<TipPanel>(E_UI_Layer.Top).ChangeInfo("�˺Ż��������");
                }
            
        });
    
        //�����ס���� �߼�
        togPw.onValueChanged.AddListener((ison) =>
        {
            //����ס����ȡ��ѡ�У��Զ���¼ҲҪȡ��ѡ��
            if (!ison)
            {
                togAuto.isOn = false;
            }


        });
        //����Զ���¼�߼�

        togAuto.onValueChanged.AddListener((ison) =>
        {
            //ѡ���Զ���¼����ס����ûѡ��Ӧ������ѡ��
            if (ison)
            {
                togPw.isOn = true;
            }
        });
    }

    public override void ShowMe()
    {
        base.ShowMe();//��ʾ�Լ�ʱ �������� ��������ϵ�����

        //�õ�����
        LoginData loginData = LoginMgr.Instance.LoginData;
        //��ʼ���������
        //����������ѡ��
        togPw.isOn = loginData.rememberpw;
        togAuto.isOn = loginData.autoLogin;

        //�����˺�����
        inputUN.text = loginData.username;
        //�����ǲ���ѡ�˼�ס�����������Ƿ��������
        if(togPw.isOn) 
            inputPW.text = loginData.password;

        if(togAuto.isOn)
        {
            //�Զ�ȡ��֤�˺�����
            if (LoginMgr.Instance.CheckInfo(inputUN.text, inputPW.text))
            {

                //�����Լ�
                UIManager.Instance.HidePanel<LoginPanel>();
                UIManager.Instance.ShowPanel<DataPanel>(E_UI_Layer.Mid);
                UIManager.Instance.ShowPanel<MainPanel>(E_UI_Layer.Bot);
            }
            else
            {
                //��¼ʧ��
                UIManager.Instance.ShowPanel<TipPanel>(E_UI_Layer.Top).ChangeInfo("�˺Ż��������");
            }
        }

        }
    
    public void SetInfo(string userName,string password)
    {
        inputPW.text = password;
        inputUN.text = userName;
    }
    
}
