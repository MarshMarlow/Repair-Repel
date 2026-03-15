using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponDurabilityUI : MonoBehaviour
{
    public ObjectGrabbable[] grabbableObjects;
    public Slider durabilitySlider;
    public TMP_Text durabilityText;

    public Image fillImage;
    public Image backgroundImage;
    public TMP_Text brokenText;

    private WeaponDurability currentWeapon;

    void Update()
    {
        FindHeldWeapon();

        if (currentWeapon == null)
        {
            durabilitySlider.gameObject.SetActive(false);
            durabilityText.gameObject.SetActive(false);

            if (brokenText != null)
                brokenText.gameObject.SetActive(false);

            return;
        }

        int current = currentWeapon.CurrentDurability;
        int max = currentWeapon.MaxDurability;

        // if weapon is broken
        if (current <= 0)
        {
            durabilitySlider.gameObject.SetActive(false);
            durabilityText.gameObject.SetActive(false);

            if (brokenText != null)
                brokenText.gameObject.SetActive(true);

            return;
        }

        // weapon not broken
        durabilitySlider.gameObject.SetActive(true);
        durabilityText.gameObject.SetActive(true);

        if (brokenText != null)
            brokenText.gameObject.SetActive(false);

        durabilitySlider.maxValue = max;
        durabilitySlider.value = current;

        durabilityText.text = current + " / " + max;

        float percent = (float)current / max;

        Color durabilityColor;

        if (percent > 0.6f)
            durabilityColor = Color.green;
        else if (percent > 0.3f)
            durabilityColor = new Color(1f, 0.5f, 0f);
        else
            durabilityColor = Color.red;

        if (fillImage != null)
            fillImage.color = durabilityColor;

        if (backgroundImage != null)
            backgroundImage.color = durabilityColor;
    }

    void FindHeldWeapon()
    {
        currentWeapon = null;

        foreach (ObjectGrabbable grabbable in grabbableObjects)
        {
            if (grabbable == null) continue;
            if (!grabbable.isHeld()) continue;

            WeaponDurability durability = grabbable.GetComponent<WeaponDurability>();

            if (durability != null)
            {
                currentWeapon = durability;
                return;
            }
        }
    }
}