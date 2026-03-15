using UnityEngine;
using UnityEngine.UI;

public class LampBatteryHUD : MonoBehaviour
{
    public LanternFlashlight lantern;

    public Image bar1;
    public Image bar2;
    public Image bar3;
    public Image bar4;

    public GameObject lowBatteryText;

    public Color emptyColor = Color.gray;
    public Color fullColor = Color.yellow;

    void Update()
    {
        if (lantern == null) return;

        float percent = lantern.getRemainingSeconds() / lantern.getMaxSeconds();
        percent = Mathf.Clamp01(percent);

        int activeBars = 0;

        if (percent > 0f) activeBars = 1;
        if (percent > 0.25f) activeBars = 2;
        if (percent > 0.50f) activeBars = 3;
        if (percent > 0.75f) activeBars = 4;

        Color currentYellow = Color.Lerp(emptyColor, fullColor, percent);

        SetBar(bar1, activeBars >= 1, currentYellow);
        SetBar(bar2, activeBars >= 2, currentYellow);
        SetBar(bar3, activeBars >= 3, currentYellow);
        SetBar(bar4, activeBars >= 4, currentYellow);

        if (lowBatteryText != null)
        {
            lowBatteryText.SetActive(percent > 0f && percent <= 0.25f);
        }
    }

    void SetBar(Image bar, bool isOn, Color activeColor)
    {
        if (bar == null) return;

        if (isOn)
            bar.color = activeColor;
        else
            bar.color = emptyColor;
    }
}