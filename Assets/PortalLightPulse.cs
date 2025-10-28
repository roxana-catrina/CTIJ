using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PortalLightPulse : MonoBehaviour
{
    private Light2D light2D;
    public float minIntensity = 2f;
    public float maxIntensity = 3f;
    public float pulseSpeed = 2f;

    private void Start()
    {
        light2D = GetComponent<Light2D>();
    }

    private void Update()
    {
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
        light2D.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
    }
}
