using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseInteraction : MonoBehaviour
{
    [SerializeField] public InteractionController _interactionController;
    Outline outline;

    private void Awake()
    {

        BaseAwake();

    }

    protected virtual void AddOutLine(Color color, float widthValue, bool enableValue)
    {
        if (!enableValue)
        {
            return;
        }
        outline = gameObject.AddComponent<Outline>();
        outline.OutlineColor = color;
        outline.OutlineWidth = widthValue;
    }

    protected virtual void BaseAwake()
    {
        AddOutLine(new Color(0, 0.7f, 1), 10f, true);
    }

    private void OnEnable()
    {
        BaseOnEnable();
    }

    protected virtual void BaseOnEnable()
    {

    }

    private void Start()
    {
        BaseStart();
    }

    protected virtual void BaseStart()
    {
        outline.enabled = false;
    }

    private void Update()
    {
        BaseUpdate();
    }

    protected virtual void BaseUpdate()
    {

    }

    private void OnDestroy()
    {
        BaseOnDestroy();
    }

    protected virtual void BaseOnDestroy()
    {

    }

    private void OnDisable()
    {
        BaseOnDisable();
    }

    protected virtual void BaseOnDisable()
    {

    }

    public virtual void Interact()
    {
        //ToDo:执行交互前所要执行的事件
        //ToDo:所有的交互执行语句需要通过该方法来进行。
        Debug.Log("交互");
    }
    /// <summary>
    /// 显示交互按钮信息
    /// </summary>
    public virtual void Select()
    {
        //ToDo:当物体被焦点选中的时候，所执行的逻辑语句
        ChangeInformationText();
        try
        {
            outline.enabled = true;
        }
        catch
        {
            Debug.LogError("异常物体" + gameObject.name + "缺失控件Outline，查询是否缺少OutLine上的赋值，以及BaseAwake上的调用");
        }
    }

    public virtual void Select(InteractionController value)
    {
        _interactionController = value;
        Select();
    }
    /// <summary>
    /// 隐藏交互按钮信息
    /// </summary>
    public virtual void CancelSelect()
    {
        //ToDo:当物体失去焦点的时候所执行的逻辑语句，同时要注意该方法在被继承的时候需要在子类中执行base.CancelSelect
        InteractionButtonManager.Instance.HideButton();
        try
        {
            outline.enabled = false;
        }
        catch
        {
            Debug.LogError("异常物体" + gameObject.name + "缺失控件Outline，查询是否缺少OutLine上的赋值，以及BaseAwake上的调用");
        }
    }
    /// <summary>
    /// 改变按钮文本信息
    /// </summary>
    protected virtual void ChangeInformationText()
    {
        InteractionButtonManager.Instance.ShowButton("按E交互");
    }
    
}
