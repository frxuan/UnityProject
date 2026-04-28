using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightInteraction : BaseInteraction
{
    [SerializeField]protected GameObject _light;
    [SerializeField]protected List<Light> pointLights;

    protected override void BaseStart()
    {
        base.BaseStart();
        
    }
    public override void Interact()
    {
        base.Interact();
        ChangeColor();
        _light.SetActive(!_light.activeSelf);
    }

    protected virtual void ChangeColor(Color color)
    {
        foreach(Light i in pointLights)
        {
            i.color = color;
        }
    }

    protected virtual void ChangeColor()
    {
        ChangeColor(new Color(1, 1, 1));
    }
}
