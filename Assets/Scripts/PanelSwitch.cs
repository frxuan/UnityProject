using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelSwitch : MonoBehaviour
{
    public GameObject loginPanel;
    public GameObject regsiterPanel;
    public Text loginTitle;
    // Start is called before the first frame update
    void Start()
    {
        loginPanel.SetActive(true);
        regsiterPanel.SetActive(false);
        Input.imeCompositionMode = IMECompositionMode.Off;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoginPanelShow()
    {
        loginPanel.SetActive(true);
        regsiterPanel.SetActive(false);
        loginTitle.text = null;
    }

    public void RegsiterPanelShow()
    {
        loginPanel.SetActive(false);
        regsiterPanel.SetActive(true);
        loginTitle.text = null;
    }


}
