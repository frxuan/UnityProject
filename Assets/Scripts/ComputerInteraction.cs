using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerInteraction : BaseInteraction
{
    public GameObject ui;
    public override void Interact()
    {
        base.Interact();
        PlayerControllerManager.Instance.isCanController = false;
        ui.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
