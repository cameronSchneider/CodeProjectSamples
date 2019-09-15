using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessScript : MonoBehaviour
{
    [Header("Draw Mode Vignette Settings")]
    [Range(0.0f, 1.0f)]
    public float maxDrawVignetteIntensity = 0.5f;

    [Range(-100f, 100f)]
    public float maxDrawDistortionIntensity = 20f;
    public float drawVignetteTime = 0.1f;
    public Color drawVignetteColor;

    [Header("Player Hit Vignette Settings")]
    [Range(0.0f, 1.0f)]
    public float maxHitVignetteIntensity = 0.5f;
    public float hitVignetteDuration = 0.25f;
    public Color hitVignetteColor;

    private float drawTimer;
    private float reverseDrawTimer = 0.0f;
    private bool reverseDraw = false;
    private bool drawing = false;

    private Vignette vignette;
    private LensDistortion distortion;
    private PostProcessVolume vol;

    private PostProcessEffectSettings[] settings;

    private float hitTimer;
    private bool hit = false;

    // Start is called before the first frame update
    void Start()
    {
        vignette = ScriptableObject.CreateInstance<Vignette>();
        vignette.enabled.Override(true);
        vignette.intensity.Override(0f);
        vignette.color.Override(drawVignetteColor);

        distortion = ScriptableObject.CreateInstance<LensDistortion>();
        distortion.enabled.Override(true);
        distortion.intensity.Override(0f);

        settings = new PostProcessEffectSettings[2];
        settings[0] = vignette;
        settings[1] = distortion;

        vol = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, settings);
        vol.priority = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Get input
        if (Input.GetMouseButtonDown(1))
        {
            drawing = !drawing;
            drawTimer = 0f;
        }

        // Update draw vignette
        if (drawing)
        {
            reverseDrawTimer = drawVignetteTime;
            reverseDraw = true;

            vignette.color.Override(drawVignetteColor);
            vignette.intensity.Override(maxDrawVignetteIntensity);

            if (drawTimer < drawVignetteTime)
                drawTimer += Time.deltaTime;
            else
                drawTimer = drawVignetteTime;

            vignette.intensity.value = (drawTimer / drawVignetteTime) * maxDrawVignetteIntensity;
            distortion.intensity.value = (drawTimer / drawVignetteTime) * maxDrawDistortionIntensity;
        }
        else
        {
            if(reverseDraw)
                reverseDrawTimer -= Time.deltaTime;
            if(reverseDrawTimer <= 0.0f)
            {
                reverseDrawTimer = 0.0f;
                reverseDraw = false;
            }

            distortion.intensity.value = (reverseDrawTimer / drawVignetteTime) * maxDrawDistortionIntensity;
            vignette.intensity.value = (reverseDrawTimer / drawVignetteTime) * maxDrawVignetteIntensity;
            drawTimer = 0f;
        }

        // Update hit vignette
        if (hit)
        {
            vignette.color.Override(hitVignetteColor);
            vignette.intensity.Override(maxHitVignetteIntensity);

            if (hitTimer < hitVignetteDuration)
                hitTimer += Time.deltaTime;
            else
            {
                hitTimer = hitVignetteDuration;
                hit = false;
            }

            vignette.intensity.value = Mathf.Sin(Mathf.PI * hitTimer / hitVignetteDuration) * maxHitVignetteIntensity;
        }
        else
        {
            hitTimer = 0f;
        }
    }

    public void GetHit()
    {
        hit = true;
        hitTimer = 0f;
    }

    private void OnDestroy()
    {
        RuntimeUtilities.DestroyVolume(vol, true);
    }
}