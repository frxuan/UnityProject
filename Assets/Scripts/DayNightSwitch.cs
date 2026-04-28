using UnityEngine;

[RequireComponent(typeof(Light))]
public class DayNightSwitch : MonoBehaviour
{
    [Header("白天设置")]
    public Color dayLightColor = Color.white;
    public float dayIntensity = 1.2f;
    public bool dayShadow = true; // 白天开启阴影

    [Header("黑夜设置")]
    public Color nightLightColor = new Color(0.2f, 0.3f, 0.5f);
    public float nightIntensity = 0.2f;
    public bool nightShadow = false; // 黑夜关闭阴影（黑影效果）

    [Header("环境光")]
    public Color dayAmbient = Color.white;
    public Color nightAmbient = new Color(0.1f, 0.1f, 0.15f);

    // 内部变量
    private Light _sunLight;
    private bool _isDay = true;

    void Start()
    {
        // 获取方向光
        _sunLight = GetComponent<Light>();

        // 初始化白天效果
        SetDay();
    }

    void Update()
    {
        // ========== 【按键切换】按 B 键一键切换 ==========
        if (Input.GetKeyDown(KeyCode.B))
        {
            SwitchDayNight();
        }
    }

    /// <summary>
    /// 一键切换白天/黑夜
    /// </summary>
    public void SwitchDayNight()
    {
        _isDay = !_isDay;

        if (_isDay)
            SetDay();
        else
            SetNight();
    }

    /// <summary>
    /// 设置为白天
    /// </summary>
    void SetDay()
    {
        // 灯光颜色 + 强度
        _sunLight.color = dayLightColor;
        _sunLight.intensity = dayIntensity;

        // 阴影开关
        _sunLight.shadows = dayShadow ? LightShadows.Soft : LightShadows.None;

        // 环境光
        RenderSettings.ambientLight = dayAmbient;
    }

    /// <summary>
    /// 设置为黑夜（黑影效果）
    /// </summary>
    void SetNight()
    {
        _sunLight.color = nightLightColor;
        _sunLight.intensity = nightIntensity;

        // 黑夜关闭阴影 = 黑影效果
        _sunLight.shadows = nightShadow ? LightShadows.Soft : LightShadows.None;

        RenderSettings.ambientLight = nightAmbient;
    }
}
