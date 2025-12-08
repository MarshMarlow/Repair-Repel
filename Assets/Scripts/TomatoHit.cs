using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TomatoHit : MonoBehaviour
{
    public GameObject damagePanel;
    public float flashTime = 0.2f; 

    public void FlashRed()
    {
        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        damagePanel.SetActive(true);
        yield return new WaitForSeconds(flashTime);
        damagePanel.SetActive(false);
    }
}
