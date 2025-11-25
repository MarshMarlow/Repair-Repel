using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFades : MonoBehaviour
{
    public Light[] lights;
    public float max_intensity = 80f;
    public float fade_duration = 5f;

    private EnemyThrow enemyThrow;

    private bool doneFading = false;
    
    void Start() {
        enemyThrow = GetComponent<EnemyThrow>();
        
        // starts all lights at 0
        foreach (Light light in lights) {
            light.intensity = 0;
        }
        
        // start fade in coroutine
        StartCoroutine(FadeInLights());
    }

    // Update is called once per frame
    void Update()
    {
        if (doneFading) enemyThrow.canThrow = true;
    }

    private IEnumerator FadeInLights() {
        float time = 0f;
        
        // fade in the lights throughout the fade duration
        while (time < fade_duration) {
            time += Time.deltaTime;
            float t = time / fade_duration;

            foreach (Light light in lights) {
                light.intensity = Mathf.Lerp(0, max_intensity, t);
            }

            yield return null;
        }

        // ensure all are at their max intensities
        foreach (Light light in lights) {
            light.intensity = max_intensity;
        }
        doneFading = true;
    }

    public IEnumerator FadeOutLights() {
        float time = 0f;
        
        // fade out the lights throughout the fade duration
        while (time < fade_duration) {
            time += Time.deltaTime;
            float t = time / fade_duration;

            foreach (Light light in lights) {
                light.intensity = Mathf.Lerp(max_intensity, 0, t);
            }

            yield return null;
        }

        // ensure all are at 0 intensity
        foreach (Light light in lights) {
            light.intensity = 0;
        }

        yield return new WaitForSeconds(0.5f);

        enemyThrow.DestroyItself();
    }
}
