using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionButtonManager : Singleton<InteractionButtonManager>
{
    /// <summary>
    /// 装交互按钮的容器
    /// </summary>
    [SerializeField] public GameObject _informationText;
    /// <summary>
    /// 按钮文本
    /// </summary>
    [SerializeField] Text _text;


    /// <summary>
    /// 显示按钮
    /// </summary>

    
    public void ShowButton(string value)
    {
        _informationText.SetActive(true);
        ChangeText(value);
    }
    /// <summary>
    /// 隐藏按钮
    /// </summary>
    
    public void HideButton()
    {
        _informationText.SetActive(false);
    }
    /// <summary>
    /// 更改按钮文本
    /// </summary>
    
    public void ChangeText(string value)
    {
        _text.text = value;
    }
}
