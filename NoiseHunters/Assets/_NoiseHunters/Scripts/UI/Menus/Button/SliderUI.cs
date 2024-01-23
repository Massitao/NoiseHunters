using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SliderUI : MonoBehaviour
{
    [Header("Slider Sounds")]
    [SerializeField] protected AudioClip selectSound;
    [SerializeField] protected AudioClip valueChangeSound;

    [Header("Inside Menu")]
    protected Menu insideMenu;

    [Header("Slider")]
    protected Slider slider;

    [Header("On Value Change Coroutine")]
    [SerializeField] protected float onValueChangePlaySoundDelay;
    protected Coroutine onValueChangeCoroutine;


    protected virtual void Awake()
    {
        insideMenu = GetComponentInParent<Menu>();
        slider = GetComponent<Slider>();
    }

    public virtual Slider GetThisSlider()
    {
        return slider;
    }


    public virtual void SetCurrentSelectedGameObject()
    {
        insideMenu.manager.SetSelectedGameObject(slider.gameObject);
    }
    public virtual void OnSelect()
    {
        if (selectSound != null)
        {
            insideMenu.lastSelectedElement = gameObject;
            SoundDictionary.CreateOnlyAudioSource(this, selectSound, transform.position);
        }
    }

    public virtual void SetNewValue(float value)
    {
        slider.value += value;
        slider.value = Mathf.Clamp(slider.value, slider.minValue, slider.maxValue);
    }

    public virtual void OnValueChange()
    {
        if (valueChangeSound != null)
        {
            if (onValueChangeCoroutine != null)
            {
                StopCoroutine(onValueChangeCoroutine);
            }

            onValueChangeCoroutine = StartCoroutine(OnValueChangeSoundWait());
        }
    }
    private IEnumerator OnValueChangeSoundWait()
    {
        yield return new WaitForSecondsRealtime(onValueChangePlaySoundDelay);

        SoundDictionary.CreateOnlyAudioSource(this, valueChangeSound, transform.position);
    }
}
