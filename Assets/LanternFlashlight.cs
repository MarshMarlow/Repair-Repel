using UnityEngine;

public class LanternFlashlight : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light lanternLight;

    [Header("Settings")]
    [SerializeField] private KeyCode toggleKey = KeyCode.F;
    [SerializeField] private bool startOn = true;

    [Header("Battery")]
    [Tooltip("Total seconds of light time available.")]
    [SerializeField] private float maxOnTimeSeconds = 60f;

    // Remaining battery time
    [SerializeField] private float remainingSeconds;

    private bool _isOn;
    private bool _isDepleted;

    private void Awake()
    {
        if (remainingSeconds <= 0f)
            remainingSeconds = maxOnTimeSeconds;

        // If empty battery, turn flashlight off
        if (remainingSeconds <= 0f)
        {
            _isDepleted = true;
            SetOn(false);
            return;
        }

        SetOn(startOn);
    }

    private void Update()
    {
        var grabbable = GetComponent<ObjectGrabbable>();
        if (grabbable == null || !grabbable.isHeld()) return;

        // If ON, drain battery
        if (_isOn && !_isDepleted)
        {
            remainingSeconds -= Time.deltaTime;

            if (remainingSeconds <= 0f)
            {
                remainingSeconds = 0f;
                _isDepleted = true; // flashlight battery is dead
                SetOn(false); // turn off flashlight
            }
        }

        // flashlight off and on toggle
        if (Input.GetKeyDown(toggleKey))
        {
            // If dead, do not turn on
            if (_isDepleted)
            {
                return;
            }

            SetOn(!_isOn); // turn on
        }
    }

    // Turn on flashlight
    public void SetOn(bool on)
    {
        if (_isDepleted) on = false;

        _isOn = on;

        if (lanternLight != null)
            lanternLight.enabled = on;
    }

    public bool IsOn() => _isOn;
    public bool IsDepleted() => _isDepleted;
    public float RemainingSeconds() => remainingSeconds;

    // Recharges the battery
    public void RechargeFull()
    {
        remainingSeconds = maxOnTimeSeconds;
        _isDepleted = false;
    }
}
