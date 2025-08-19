using System.Collections.Generic;
using UnityEngine;

public enum E_UI_Layer
{
    Bot,
    Mid,
    Top,
    System
}

public class UIManager
{
    private static UIManager instance = new UIManager();
    public static UIManager Instance => instance;

    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    private Transform bot;
    private Transform mid;
    private Transform top;
    private Transform system;
    private RectTransform canvas;

    private UIManager()
    {
        // ���� Canvas
        GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas"));
        canvas = obj.transform as RectTransform;
        GameObject.DontDestroyOnLoad(obj);

        bot = canvas.Find("Bot");
        mid = canvas.Find("Mid");
        top = canvas.Find("Top");
        system = canvas.Find("System");

        // ȷ�� EventSystem ����
        GameObject eventSys = GameObject.Find("EventSystem");
        if (eventSys == null)
        {
            eventSys = GameObject.Instantiate(Resources.Load<GameObject>("UI/EventSystem"));
            GameObject.DontDestroyOnLoad(eventSys);
        }
    }

    /// <summary>
    /// ��ȡ UI ��
    /// </summary>
    private Transform GetLayerFather(E_UI_Layer layer)
    {
        return layer switch
        {
            E_UI_Layer.Bot => bot,
            E_UI_Layer.Mid => mid,
            E_UI_Layer.Top => top,
            E_UI_Layer.System => system,
            _ => mid,
        };
    }

    /// <summary>
    /// ��ʾ���
    /// </summary>
    public T ShowPanel<T>(E_UI_Layer layer = E_UI_Layer.Mid) where T : BasePanel
    {
        string panelName = typeof(T).Name;

        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].gameObject.SetActive(true);
            panelDic[panelName].ShowMe();
            return panelDic[panelName] as T;
        }

        GameObject prefab = Resources.Load<GameObject>("UI/" + panelName);
        if (prefab == null)
        {
            Debug.LogError($"�Ҳ��� UI Ԥ���壺UI/{panelName}");
            return null;
        }

        GameObject obj = GameObject.Instantiate(prefab);
        obj.transform.SetParent(GetLayerFather(layer), false);

        T panel = obj.GetComponent<T>();
        panelDic.Add(panelName, panel);
        panel.ShowMe();
        return panel;
    }

    /// <summary>
    /// �������
    /// </summary>
    public void HidePanel<T>(bool isFade = true) where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (!panelDic.ContainsKey(panelName)) return;

        if (isFade)
        {
            panelDic[panelName].HideMe(() =>
            {
                panelDic[panelName].gameObject.SetActive(false);
            });
        }
        else
        {
            panelDic[panelName].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ��ȡ���
    /// </summary>
    public T GetPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
            return panelDic[panelName] as T;
        return null;
    }
}
