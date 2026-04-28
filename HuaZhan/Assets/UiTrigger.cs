using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiTrigger : MonoBehaviour
{
    public Canvas ui;

    private void OnTriggerEnter(Collider other)
    {
        ui.gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        ui.gameObject.SetActive(false);
    }
}
