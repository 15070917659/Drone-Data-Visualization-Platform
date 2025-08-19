using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPanel : BasePanel
{
    private Button btnCancel;//ȡ����ť
    private Button btnSure;//ȷ����ť

    private TMP_InputField inputUN;//�˺������
    private TMP_InputField inputPW;//���������

    public override void Init()
    {

        btnCancel = GetControl<Button>("ȡ����ť");
        btnSure = GetControl<Button>("ȷ����ť");
        inputUN = GetControl<TMP_InputField>("�˺������");
        inputPW = GetControl<TMP_InputField>("���������");


        btnCancel.onClick.AddListener(() =>
        {
            //�����Լ�
            UIManager.Instance.HidePanel<RegisterPanel>();
            //��ʾ��¼���
            UIManager.Instance.ShowPanel<LoginPanel>();
        });

        btnSure.onClick.AddListener(() =>
        {
            //�ж�������˺������Ƿ����
            if(inputUN.text.Length < 6 ||  inputPW.text.Length < 6)
            {
                //��ʾ���Ϸ�
                TipPanel panel = UIManager.Instance.ShowPanel<TipPanel>(E_UI_Layer.Top);
                //�ı�����
                panel.ChangeInfo("�˺ź����붼������ڵ���6λ");
                return;
            }

            //ȥע���˺�����
            if (LoginMgr.Instance.RegisterUser(inputUN.text, inputPW.text))
            {
                //ע��ɹ�
                //��ʾ��¼���
                LoginPanel loginPanel = UIManager.Instance.ShowPanel<LoginPanel>();
                //��ʾ��¼����ϵ��û���������
                loginPanel.SetInfo(inputUN.text,inputPW.text);

                LoginMgr.Instance.ClearLoginData();
              
                //�����Լ�
                UIManager.Instance.HidePanel<RegisterPanel>();

            }
            else
            {
                TipPanel tipPanel = UIManager.Instance.ShowPanel<TipPanel> (E_UI_Layer.Top);
                //�ı���ʾ���ݡ�
                tipPanel.ChangeInfo("�û����Ѵ���");

                inputUN.text = "";
                inputPW.text = "";
            }
        });
    }
}
