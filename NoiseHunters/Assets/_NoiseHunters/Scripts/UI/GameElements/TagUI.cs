using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TagUI : MonoBehaviour
{
    [Header("Tag UI External Components")]
    private Camera tagUI_PlayerCamera;
    [HideInInspector] public CharacterCombat tagUI_PlayerCombat;


    [Header("Tag UI Internal Components")]
    [SerializeField] private Image tagUI_EntityImage;
    [SerializeField] private Image tagUI_EntityBackgroundImage;
    [SerializeField] private Image tagUI_EntityBackgroundFillImage;
    [SerializeField] private Image tagUI_FocusedImage;

    [HideInInspector] public Sprite tagUI_EntitySprite;


    [Header("Tag UI Values")]
    [SerializeField] private float tagUI_maxTimeToClearFill;
    private float tagUI_currentTagFill;

    private bool tagUI_Active;
    public bool TagUI_Active
    {
        get { return tagUI_Active; }
        set
        {
            if (tagUI_Active != value)
            {
                tagUI_Active = value;

                OnTagStateChange?.Invoke();
            }
        }
    }


    [Header("Tag UI Update Coroutines")]
    private Coroutine tagUI_UpdateTagCoroutine;


    [Header("Tag UI Alert Colors")]
    [SerializeField] private Gradient tagUI_AlertColor;
    [SerializeField] private float tagUI_AlphaLerpTime;
    [SerializeField] private float tagUI_ColorLerpTime;

    private Coroutine tagUI_ColorChangeCoroutine;
    private Coroutine tagUI_EntityAlphaChangeCoroutine;
    private Coroutine tagUI_MarkedAlphaChangeCoroutine;


    [Header("Tag UI Events")]
    public Action OnTagStateChange;


    // Get Camera on Instantiation
    public void Awake()
    {
        tagUI_PlayerCamera = FindObjectOfType<ThirdPersonCamera>().GetComponent<Camera>();
        tagUI_PlayerCombat = FindObjectOfType<CharacterCombat>();
    }

    // Set Tag Visibility
    void Start()
    {

        tagUI_EntityImage.color = new Color(tagUI_EntityImage.color.r, tagUI_EntityImage.color.g, tagUI_EntityImage.color.b, 0f);
        tagUI_EntityBackgroundImage.color = new Color(tagUI_EntityBackgroundImage.color.r, tagUI_EntityBackgroundImage.color.g, tagUI_EntityBackgroundImage.color.b, 0f);
        tagUI_EntityBackgroundFillImage.color = new Color(tagUI_EntityBackgroundFillImage.color.r, tagUI_EntityBackgroundFillImage.color.g, tagUI_EntityBackgroundFillImage.color.b, 0f);
        tagUI_FocusedImage.color = new Color(tagUI_FocusedImage.color.r, tagUI_FocusedImage.color.g, tagUI_FocusedImage.color.b, 0f);
    }

    // Check when to rotate towards Camera position
    void Update()
    {
        Vector3 direction = (tagUI_PlayerCamera.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z), Vector3.up);
    }



    // Tag Image becomes visible
    public void Tagged(float tagIntensity)
    {
        if (tagIntensity > 0f)
        {
            tagUI_currentTagFill += tagIntensity;
            tagUI_currentTagFill = Mathf.Clamp01(tagUI_currentTagFill);

            if (tagUI_UpdateTagCoroutine != null)
            {
                StopCoroutine(tagUI_UpdateTagCoroutine);
                tagUI_UpdateTagCoroutine = null;
            }

            if (tagUI_currentTagFill != 0f)
            {
                TagEntityImage_SetState(true);

                tagUI_UpdateTagCoroutine = StartCoroutine(Tag_UpdateFill());
            }
            else
            {
                StopTag();
            }
        }
    }

    // Slowly decrease fill amount of Tag
    IEnumerator Tag_UpdateFill()
    {
        yield return new WaitForSeconds(0.2f);

        float timeToClearFill = tagUI_maxTimeToClearFill * tagUI_currentTagFill;

        for (float i = timeToClearFill; i > 0f; i -= Time.deltaTime)
        {
            tagUI_currentTagFill = Mathf.InverseLerp(0f, tagUI_maxTimeToClearFill, i);
            tagUI_EntityImage.fillAmount = tagUI_currentTagFill;

            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        StopTag();

        yield break;
    }

    // Tag Image becomes invisible
    public void StopTag()
    {
        tagUI_currentTagFill = 0f;
        TagEntityImage_SetState(false);

        if (tagUI_UpdateTagCoroutine != null)
        {
            StopCoroutine(tagUI_UpdateTagCoroutine);
            tagUI_UpdateTagCoroutine = null;
        }
    }



    // Sets Visibility State of Tag
    void TagEntityImage_SetState(bool active)
    {
        TagUI_Active = active;

        if (tagUI_EntityAlphaChangeCoroutine != null)
        {
            StopCoroutine(tagUI_EntityAlphaChangeCoroutine);
        }

        tagUI_EntityAlphaChangeCoroutine = StartCoroutine(Tag_EntityAlphaLerp(active));

        if (!active)
        {
            TagFocusedImage_SetState(active);
        }
    }

    // Set Visibility State of Focus Image
    public void TagFocusedImage_SetState(bool active)
    {
        if (tagUI_MarkedAlphaChangeCoroutine != null)
        {
            StopCoroutine(tagUI_MarkedAlphaChangeCoroutine);
        }

        tagUI_MarkedAlphaChangeCoroutine = StartCoroutine(Tag_MarkedAlphaLerp(active));
    }


    // Set Alert Color
    public void TagEntityImage_AlertState(float alertLevel)
    {
        if (tagUI_ColorChangeCoroutine != null)
        {
            StopCoroutine(tagUI_ColorChangeCoroutine);
        }

        tagUI_ColorChangeCoroutine = StartCoroutine(Tag_ColorLerp(alertLevel));
    }

    IEnumerator Tag_ColorLerp(float alertLevel)
    {
        Color lerpedColor;
        float lerpPercentage;

        for (float timeToReachColor = tagUI_ColorLerpTime; timeToReachColor > 0f; timeToReachColor -= Time.deltaTime)
        {
            lerpPercentage = Mathf.InverseLerp(tagUI_ColorLerpTime, 0f, timeToReachColor);
            lerpedColor = (alertLevel != 0f) ? tagUI_AlertColor.Evaluate(alertLevel * lerpPercentage) : tagUI_AlertColor.Evaluate(1  * (1 - lerpPercentage));
            lerpedColor = new Color(lerpedColor.r, lerpedColor.g, lerpedColor.b, tagUI_EntityImage.color.a);

            tagUI_EntityImage.color = lerpedColor;
            tagUI_EntityBackgroundImage.color = lerpedColor;

            yield return null;
        }

        tagUI_EntityImage.color = tagUI_AlertColor.Evaluate(alertLevel); ;
        tagUI_EntityBackgroundImage.color = tagUI_AlertColor.Evaluate(alertLevel); ;

        tagUI_ColorChangeCoroutine = null;

        yield break;
    }

    IEnumerator Tag_EntityAlphaLerp(bool appear)
    {
        float alphaToSet = (appear) ? 1f : 0f;
        float initialAlpha = tagUI_EntityImage.color.a;

        float lerpPercentage;
        float alpha;

        for (float timeToReachAlpha = tagUI_AlphaLerpTime; timeToReachAlpha > 0f; timeToReachAlpha -= Time.deltaTime)
        {
            lerpPercentage = Mathf.InverseLerp(tagUI_AlphaLerpTime, 0f, timeToReachAlpha);
            alpha = Mathf.Lerp(initialAlpha, alphaToSet, lerpPercentage);

            tagUI_EntityImage.color = new Color(tagUI_EntityImage.color.r, tagUI_EntityImage.color.g, tagUI_EntityImage.color.b, alpha);
            tagUI_EntityBackgroundImage.color = new Color(tagUI_EntityBackgroundImage.color.r, tagUI_EntityBackgroundImage.color.g, tagUI_EntityBackgroundImage.color.b, alpha);
            tagUI_EntityBackgroundFillImage.color = new Color(tagUI_EntityBackgroundFillImage.color.r, tagUI_EntityBackgroundFillImage.color.g, tagUI_EntityBackgroundFillImage.color.b, alpha);

            yield return null;
        }

        tagUI_EntityImage.color = new Color(tagUI_EntityImage.color.r, tagUI_EntityImage.color.g, tagUI_EntityImage.color.b, alphaToSet);
        tagUI_EntityBackgroundImage.color = new Color(tagUI_EntityBackgroundImage.color.r, tagUI_EntityBackgroundImage.color.g, tagUI_EntityBackgroundImage.color.b, alphaToSet);
        tagUI_EntityBackgroundFillImage.color = new Color(tagUI_EntityBackgroundFillImage.color.r, tagUI_EntityBackgroundFillImage.color.g, tagUI_EntityBackgroundFillImage.color.b, alphaToSet);

        tagUI_EntityAlphaChangeCoroutine = null;

        yield break;
    }
    IEnumerator Tag_MarkedAlphaLerp(bool appear)
    {
        float alphaToSet = (appear) ? 1f : 0f;
        float initialAlpha = tagUI_FocusedImage.color.a;

        float lerpPercentage;
        float alpha;

        for (float timeToReachAlpha = tagUI_AlphaLerpTime; timeToReachAlpha > 0f; timeToReachAlpha -= Time.deltaTime)
        {
            lerpPercentage = Mathf.InverseLerp(tagUI_AlphaLerpTime, 0f, timeToReachAlpha);
            alpha = Mathf.Lerp(initialAlpha, alphaToSet, lerpPercentage);

            tagUI_FocusedImage.color = new Color(tagUI_FocusedImage.color.r, tagUI_FocusedImage.color.g, tagUI_FocusedImage.color.b, alpha);

            yield return null;
        }

        tagUI_FocusedImage.color = new Color(tagUI_FocusedImage.color.r, tagUI_FocusedImage.color.g, tagUI_FocusedImage.color.b, alphaToSet);

        tagUI_MarkedAlphaChangeCoroutine = null;

        yield break;
    }


    // Sets Tag Image Sprite
    public void TagEntityImage_SetSprite(Sprite entitySpriteToSet, Sprite entityBackgroundSpriteToSet, Sprite entityBackgroundFillSpriteToSet, Sprite focusedSpriteToSet)
    {
        tagUI_EntityImage.sprite = entitySpriteToSet;
        tagUI_EntityBackgroundImage.sprite = entityBackgroundSpriteToSet;
        tagUI_EntityBackgroundFillImage.sprite = entityBackgroundFillSpriteToSet;
        tagUI_FocusedImage.sprite = focusedSpriteToSet;
    }
}