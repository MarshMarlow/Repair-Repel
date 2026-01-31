using UnityEngine;

public class LanternFlashlight : MonoBehaviour
{
    [SerializeField] private Light lanternLight;
    [SerializeField] private KeyCode toggleKey = KeyCode.F;
    [SerializeField] private bool startOn = true;

    [SerializeField] private float maxOnTimeSeconds = 60f;
    [SerializeField] private float remainingSeconds;

    public AudioSource audioSource;
    public AudioClip turn_on;
    public AudioClip turn_off;
    
    private bool isOn;
    private bool isEmpty;

    private void Awake()
    {
        if (remainingSeconds <= 0f)
            remainingSeconds = maxOnTimeSeconds;

        // If empty battery, turn flashlight off
        if (remainingSeconds <= 0f)
        {
            isEmpty = true;
            SetOn(false);
            return;
        }

        SetOn(startOn);
    }

    private void Update()
    {
        var grabbable = GetComponent<ObjectGrabbable>();
        if (grabbable == null || !grabbable.isHeld()) return;

        if (isOn && !isEmpty) // drain battery if on and not empty
        {
            remainingSeconds -= Time.deltaTime;

            if (remainingSeconds <= 0f)
            {
                remainingSeconds = 0f;
                isEmpty = true; // flashlight battery is dead
                
                SetOn(false); // turn off flashlight
            }
        }

        // flashlight off and on toggle
        if (Input.GetKeyDown(toggleKey))
        {
            // If dead, do not turn on
            if (isEmpty)
            {
                return;
            }
            
            // turn on
            if (isOn) {
                audioSource.PlayOneShot(turn_off);
            }
            // turn off
            else {
                audioSource.PlayOneShot(turn_on);
            }
            SetOn(!isOn); // turn on
        }
        
    }

    // Turn on flashlight
    public void SetOn(bool on)
    {
        if (isEmpty) on = false;

        isOn = on;

        if (lanternLight != null)
            lanternLight.enabled = on;
    }

    public bool IsOn() => isOn;
    public bool IsDepleted() => isEmpty;
    public float RemainingSeconds() => remainingSeconds;

    // Recharges the battery
    public void RechargeFull()
    {
        remainingSeconds = maxOnTimeSeconds;
        isEmpty = false;
    }


    public void RechargeSeconds(float seconds) // function to rechage the helmet lantern over time used in Lamp Recharge
    {
        if (seconds <= 0f) return;

        remainingSeconds = Mathf.Clamp(remainingSeconds + seconds, 0f, maxOnTimeSeconds);

        if (remainingSeconds > 0f)
            isEmpty = false;
    }

    public float getRemainingSeconds() {
        return remainingSeconds;
    }

    public float getMaxSeconds() {
        return maxOnTimeSeconds;
    }

}

