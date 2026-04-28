using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurplrLightInteraction : LightInteraction
{
    protected override void ChangeColor()
    {
        ChangeColor(new Color(0.65f, 0, 1));
    }
}
