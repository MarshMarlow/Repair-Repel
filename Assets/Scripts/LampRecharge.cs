using UnityEngine;

public class LampRecharge : MonoBehaviour
{
    [SerializeField] private float rechargeSecondsPerSecond = 3f;

    private LanternFlashlight currentLantern;
    public AudioSource audiosource;
    public AudioClip charge;
    public float soundInterval = 0.1f;
    private float nextSoundTime = 0f;

    [SerializeField] private Renderer targetRenderer; // white battery part
    private Material mat;

    private void Start()
    {
        if (targetRenderer != null)
        {
            mat = targetRenderer.material;
            mat.DisableKeyword("_EMISSION");
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out LanternFlashlight lantern))
        {
            currentLantern = lantern;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentLantern != null && other.GetComponent<LanternFlashlight>() == currentLantern)
        {
            currentLantern = null;

            // turn glow off when lantern leaves
            if (mat != null)
            {
                mat.SetColor("_EmissionColor", Color.black);
                mat.DisableKeyword("_EMISSION");
            }
        }
    }

    private void Update()
    {
        if (currentLantern == null) return;

        // charging sound (your original code)
        if (currentLantern.getRemainingSeconds() < currentLantern.getMaxSeconds())
        {
            if (Time.time >= nextSoundTime)
            {
                audiosource.PlayOneShot(charge);
                nextSoundTime = Time.time + soundInterval;
            }
        }
        else
        {
            nextSoundTime = 0f;
        }

        // charge lantern
        currentLantern.RechargeSeconds(rechargeSecondsPerSecond * Time.deltaTime);

        // show battery % as yellow glow
        if (mat != null)
        {
            float percent = currentLantern.getRemainingSeconds() / currentLantern.getMaxSeconds();
            percent = Mathf.Clamp01(percent);

            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", Color.yellow * percent * 4f);
        }
    }
}