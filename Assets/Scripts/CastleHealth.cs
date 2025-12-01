using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class CastleHealth : MonoBehaviour
{
    public TMP_Text healthText;
    public Slider healthSlider;

    public int maxHealth = 8;
    public int currentHealth;
    public float sliderEaseTime = 0.15f;
    
    void Start() {
        currentHealth = maxHealth;
    }
    
    public void Reset() {
        currentHealth = maxHealth;
        healthSlider.value = maxHealth;
    }
    
    void Update() {
        healthText.text = currentHealth + " / " + maxHealth;
    }

    public IEnumerator Damage(int damage) {
        // if current health is negative, set it to 0
        if (currentHealth - damage < 0) currentHealth = 0;
        else currentHealth -= damage;
        
        healthSlider.DOValue((float)currentHealth / maxHealth, sliderEaseTime).SetEase(Ease.Linear);

        yield return new WaitForSeconds(sliderEaseTime);

        if (currentHealth <= 0) {
            // END GAME
        }
    }

    public int getHealth() {
        return currentHealth;
    }
}
