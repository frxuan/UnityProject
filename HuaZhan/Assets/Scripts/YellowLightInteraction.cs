using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowLightInteraction : LightInteraction
{
    protected override void ChangeColor()
    {
        ChangeColor(new Color(1, 0.9f, 0));
    }

    
}
