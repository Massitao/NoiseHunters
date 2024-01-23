using UnityEngine;
using UnityEngine.UI;

public class InputMenu : Menu
{
    [Header("Input Menu Animator")]
    [SerializeField] private string animatorAccessMenu;
    [SerializeField] private string animatorExitMenu;
    private Animator animator;

    [Header("Input Image on Display")]
    [SerializeField] private Image displayImage;

    [SerializeField] private Sprite psSprite;
    [SerializeField] private Sprite pcSprite;
    [SerializeField] private Sprite xboxSprite;

    [Header("Input Buttons")]
    [SerializeField] private GameObject psButton;
    [SerializeField] private GameObject pcButton;
    [SerializeField] private GameObject xboxButton;

    private CurrentSelectedImage currentSelectedImage = CurrentSelectedImage.PC;
    private enum CurrentSelectedImage { PS, PC, XBOX }


    protected override void GetMenuComponents()
    {
        base.GetMenuComponents();
        animator = GetComponent<Animator>();
    }

    public override void AccessMenu()
    {
        base.AccessMenu();
        animator.SetTrigger(animatorAccessMenu);
    }
    public override void ExitMenu()
    {
        base.ExitMenu();
        animator.SetTrigger(animatorExitMenu);
    }

    public void ChangeDisplayToPS()
    {
        if (currentSelectedImage != CurrentSelectedImage.PS)
        {
            currentSelectedImage = CurrentSelectedImage.PS;
            displayImage.sprite = psSprite;

            defaultElement = psButton;
        }
    }
    public void ChangeDisplayToPC()
    {
        if (currentSelectedImage != CurrentSelectedImage.PC)
        {
            currentSelectedImage = CurrentSelectedImage.PC;
            displayImage.sprite = pcSprite;

            defaultElement = pcButton;
        }
    }
    public void ChangeDisplayToXbox()
    {
        if (currentSelectedImage != CurrentSelectedImage.XBOX)
        {
            currentSelectedImage = CurrentSelectedImage.XBOX;
            displayImage.sprite = xboxSprite;

            defaultElement = xboxButton;
        }
    }
}
