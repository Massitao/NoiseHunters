using UnityEngine;
using UnityEngine.UI;

public class FollowingIconHUD : MonoBehaviour
{
    [Header("Following Icon Components")]
    [SerializeField] private Camera cameraToFollow;
    private Image imageToMove;
    private Canvas thisCanvas;


    [Header("Target To Follow")]
    public Transform TargetToFollow;
    public Vector3 TargetOffset;


    [Header("Icon Screen Clamp Values")]
    [Range(0, 0.25f)]
    public float screenBorderOffset;


    [Header("Icon Offscreen Size")]
    [SerializeField] private bool reduceSizeIfOffscreen;
    [Range(0, 1)] [SerializeField] private float reducedSizeMultiplier;
    [SerializeField] private float sizeChangeLerpSpeed;


    [Header("Canvas, Screen and Icon Values")]
    [SerializeField] private Vector3 posChecker;
    private float scaleFactor;
    private Vector2 originalSize;

    private float leftX;
    private float rightX;
    private float downY;
    private float upY;


    void Awake()
    {
        // Set Components
        cameraToFollow = FindObjectOfType<Camera>();
        imageToMove = GetComponent<Image>();
        thisCanvas = GetComponentInParent<Canvas>();

        // Set Scaling Factor
        scaleFactor = thisCanvas.scaleFactor;

        // Set Icon Original Size
        originalSize = imageToMove.rectTransform.sizeDelta;
    }

    private void Update()
    {
        // If Target has been set
        if (TargetToFollow != null && imageToMove.enabled)
        {
            // Set Screen Point Values
            leftX = (Screen.width / scaleFactor) * screenBorderOffset;
            rightX = (Screen.width / scaleFactor) * (1 - screenBorderOffset);
            downY = (Screen.height / scaleFactor) * (screenBorderOffset * 2);
            upY = (Screen.height / scaleFactor) * (1 - screenBorderOffset * 2);

            // Get Target Screen Point from World Point
            Vector3 TargetToFollow_CameraView = cameraToFollow.WorldToScreenPoint(TargetToFollow.position + TargetOffset);

            // Set proper Screen Point by dividing Point by the Screen Scale Factor
            TargetToFollow_CameraView = TargetToFollow_CameraView / scaleFactor;


            // Clamp Values to Screen Point Values
            TargetToFollow_CameraView = new Vector3(Mathf.Clamp(TargetToFollow_CameraView.x, leftX, rightX), Mathf.Clamp(TargetToFollow_CameraView.y, downY, upY), TargetToFollow_CameraView.z);


            // Check if Target Position is OffScreen
            bool isOffscreen = (TargetToFollow_CameraView.x <= leftX || TargetToFollow_CameraView.x >= rightX || TargetToFollow_CameraView.y <= downY || TargetToFollow_CameraView.y >= upY);

            // If Target is Offscreen, clamp Y values due to inproper placement. Else, keep using the normal value
            TargetToFollow_CameraView = (TargetToFollow_CameraView.z <= 0f) ? new Vector3(TargetToFollow_CameraView.x * -1, TargetToFollow_CameraView.y * -1, TargetToFollow_CameraView.z) : TargetToFollow_CameraView;
            posChecker = TargetToFollow_CameraView;

            // Decide which multiplier to use if Target Position is Offscreen (If Offscreen, apply reduced Multiplier. Else, set size to Original Size)
            float multiplierToAdd = (isOffscreen && reduceSizeIfOffscreen) ? reducedSizeMultiplier : 1f;

            // Lerp Sizes to newly setted size
            imageToMove.rectTransform.sizeDelta = Vector2.Lerp(imageToMove.rectTransform.sizeDelta, originalSize * multiplierToAdd, Time.deltaTime * sizeChangeLerpSpeed);

            // Set Icon Position to calculated Screen Position
            imageToMove.rectTransform.anchoredPosition = TargetToFollow_CameraView;
        }
    }

    public void FollowingIcon_SetActiveImage(bool isActive)
    {
        imageToMove.enabled = isActive;
    }

    public void FollowingIcon_ChangeImage(Sprite newSpriteToSet)
    {
        imageToMove.sprite = newSpriteToSet;
    }

    // Used To Represent Screen Limits
    private void OnDrawGizmosSelected()
    {
        if (cameraToFollow != null)
        {
            Vector2 leftBottomVP = cameraToFollow.ViewportToScreenPoint(new Vector2(screenBorderOffset, screenBorderOffset * 2));
            Vector2 rightBottomVP = cameraToFollow.ViewportToScreenPoint(new Vector2((1 - screenBorderOffset), screenBorderOffset * 2));
            Vector2 leftUpVP = cameraToFollow.ViewportToScreenPoint(new Vector2(screenBorderOffset, (1 - screenBorderOffset * 2)));
            Vector2 rightUpVP = cameraToFollow.ViewportToScreenPoint(new Vector2((1 - screenBorderOffset), (1 - screenBorderOffset * 2)));

            Gizmos.color = Color.red;

            Gizmos.DrawLine(leftBottomVP, rightBottomVP);
            Gizmos.DrawLine(leftBottomVP, leftUpVP);
            Gizmos.DrawLine(leftUpVP, rightUpVP);
            Gizmos.DrawLine(rightUpVP, rightBottomVP);
        }
    }
}