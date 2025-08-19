using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public abstract class BasePanel : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private float alphaSpeed = 10f;
    private bool isShow;
    private UnityAction hideCallBack;

    // 控件缓存字典
    private Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();

    protected virtual void Awake()
    {
        // CanvasGroup 控制淡入淡出
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        // 自动查找控件
        FindChildrenControl<Button>();
        FindChildrenControl<Image>();
        FindChildrenControl<Text>();
        FindChildrenControl<TMP_Text>();
        FindChildrenControl<Toggle>();
        FindChildrenControl<Slider>();
        FindChildrenControl<ScrollRect>();
        FindChildrenControl<InputField>();
        FindChildrenControl<TMP_InputField>();
        Init();
    }

    protected virtual void Start()
    {
        Init();
    }

    /// <summary>
    /// 子类必须实现的初始化逻辑
    /// </summary>
    public abstract void Init();

    protected virtual void OnUpdate() { }

    private void Update()
    {
        // 淡入
        if (isShow && canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha >= 1)
                canvasGroup.alpha = 1;
        }
        // 淡出
        else if (!isShow && canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                hideCallBack?.Invoke();
            }
        }

        OnUpdate();
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    public virtual void ShowMe()
    {
        isShow = true;
        canvasGroup.alpha = 0;
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    public virtual void HideMe(UnityAction callBack)
    {
        isShow = false;
        canvasGroup.alpha = 1;
        hideCallBack = callBack;
    }

    /// <summary>
    /// 获取控件
    /// </summary>
    protected T GetControl<T>(string controlName) where T : UIBehaviour
    {
        if (controlDic.ContainsKey(controlName))
        {
            foreach (var ctrl in controlDic[controlName])
                if (ctrl is T)
                    return ctrl as T;
        }
        return null;
    }

    /// <summary>
    /// 自动查找子控件并绑定事件
    /// </summary>
    private void FindChildrenControl<T>() where T : UIBehaviour
    {
        T[] controls = GetComponentsInChildren<T>(true);
        foreach (var ctrl in controls)
        {
            string objName = ctrl.gameObject.name;
            if (!controlDic.ContainsKey(objName))
                controlDic.Add(objName, new List<UIBehaviour>());
            controlDic[objName].Add(ctrl);

            // 按钮
            if (ctrl is Button)
            {
                (ctrl as Button).onClick.AddListener(() =>
                {
                    OnClick(objName);
                });
            }
            // Toggle
            else if (ctrl is Toggle)
            {
                (ctrl as Toggle).onValueChanged.AddListener((value) =>
                {
                    OnValueChanged(objName, value);
                });
            }
        }
    }

    protected virtual void OnClick(string btnName) { }
    protected virtual void OnValueChanged(string toggleName, bool value) { }
}
