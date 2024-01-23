using UnityEngine;
using UnityEngine.UI;

public class DaggerReturnHUD : MonoBehaviour
{
    [Header("HUD Depended Components")]
    private CharacterCombat playerCombat;

    [Header("Ability Values")]
    [SerializeField] private Image MagnetCooldownImage;

    private void Awake()
    {
        playerCombat = GetComponentInParent<CharacterCombat>();
    }

    private void OnEnable()
    {
            playerCombat.OnThrowCooldownChange += UpdateMagnetCooldown;
    }

    private void OnDisable()
    {
        playerCombat.OnThrowCooldownChange -= UpdateMagnetCooldown;
    }

    // Magnet Methods
    void UpdateMagnetCooldown(float cooldownPercentage)
    {
        MagnetCooldownImage.fillAmount = cooldownPercentage;
    }
}