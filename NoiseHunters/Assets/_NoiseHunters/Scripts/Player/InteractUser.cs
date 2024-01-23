using UnityEngine;

public class InteractUser : MonoBehaviour
{
    private CharacterMeshAnimator animator;
    public Interactuable InteractuableNear;

    private void Awake()
    {
        animator = GetComponent<CharacterMeshAnimator>();
    }


    public void CanUse()
    {
        if (InteractuableNear != null)
        {
            if (InteractuableNear.CanInteract())
            {
                InteractuableNear.TryGetComponent(out PickableProp pickable);

                if (pickable != null)
                {
                    animator.PickableInteract();
                }

                InteractuableNear = null;
            }
        }
    }
}
