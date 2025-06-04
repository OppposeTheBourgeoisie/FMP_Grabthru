using UnityEngine;

[ExecuteInEditMode]
public class RingDistortionEffect : MonoBehaviour
{
    [Header("References")]
    public Material distortionMat;

    [Header("Distortion Settings")]
    public float speed = 1.0f;
    public float maxRingSize = 2.0f;

    private float ringSize = 0f;
    private bool isActive = false;

    void Start()
    {
        // Initialize the distortion material values
        if (distortionMat != null)
        {
            distortionMat.SetFloat("_RingSize", 0f);
            distortionMat.SetFloat("_DistortionStrength", 0f);
        }
    }

    void Update()
    {
        // Animate the ring distortion if active
        if (!isActive || distortionMat == null) return;

        ringSize += Time.deltaTime * speed;
        distortionMat.SetFloat("_RingSize", ringSize);

        float fade = Mathf.Lerp(0.1f, 0f, ringSize / maxRingSize);
        distortionMat.SetFloat("_DistortionStrength", fade);

        if (ringSize >= maxRingSize)
        {
            isActive = false;
            distortionMat.SetFloat("_DistortionStrength", 0f);
        }
    }

    public void TriggerDistortion()
    {
        // Start the ring distortion effect
        ringSize = 0f;
        distortionMat.SetFloat("_RingSize", ringSize);
        distortionMat.SetFloat("_RingWidth", 0.15f);
        distortionMat.SetFloat("_DistortionStrength", 0.1f);
        distortionMat.SetVector("_RingCenter", new Vector4(0.5f, 0.5f, 0, 0));
        isActive = true;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        // Apply the distortion material as a post-process effect
        if (distortionMat != null)
            Graphics.Blit(src, dest, distortionMat);
        else
            Graphics.Blit(src, dest);
    }
}
