using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TomatoHit : MonoBehaviour
{
    //public GameObject damagePanel;
    public GameObject inkPrefab;
    public Transform canvas;
    public Sprite[] ink;
    public float flashTime = 0.5f; 
    public float fadeTime = 2f;

    public void FlashRed()
    {
        SpawnInk();
    }

    void SpawnInk()
    {
        GameObject splat = Instantiate(inkPrefab, canvas);
        Image img = splat.GetComponent<Image>();

        img.sprite = ink[Random.Range(0, ink.Length)];

        RectTransform rect = splat.GetComponent<RectTransform>();

        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        StartCoroutine(FlashCoroutine(img));
    }


    private IEnumerator FlashCoroutine(Image image)
    {

        image.sprite = ink[Random.Range(0, ink.Length)];
        
        Color c = image.color;
        c.a = 1f;
        image.color = c;

        yield return new WaitForSeconds(flashTime);

        float t = 0;

        while (t < fadeTime)
        {
            t += Time.deltaTime;

            c.a = Mathf.Lerp(1f, 0f, t / fadeTime);
            image.color = c;

            yield return null;
        }
        
        c.a = 0f;
        image.color = c;

        Destroy(image.gameObject);
    }
}
