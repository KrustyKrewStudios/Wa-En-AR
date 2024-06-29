using UnityEngine;

public class WaterfallColorChanger : MonoBehaviour
{
    public ParticleSystem particleSystem;

    void Start()
    {
        if (particleSystem == null)
        {
            particleSystem = GetComponent<ParticleSystem>();
        }

        var colorOverLifetime = particleSystem.colorOverLifetime;
        colorOverLifetime.enabled = true;

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.yellow, 0.0f),  // Yellow at the start
                new GradientColorKey(Color.blue, 0.25f),   // Blue at 25%
                new GradientColorKey(new Color(0.5f, 0f, 0.5f), 0.5f), // Purple at 50%
                new GradientColorKey(new Color(1f, 0.75f, 0.8f), 0.75f), // Pink at 75%
                new GradientColorKey(Color.red, 1.0f)     // Red at the end
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1.0f, 0.0f), // Fully opaque at the start
                new GradientAlphaKey(1.0f, 1.0f)  // Fully opaque at the end
            }
        );

        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
    }
}
