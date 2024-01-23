using UnityEngine;

public class TransformFollower : MonoBehaviour
{
    public FollowMode followMode;
    public enum FollowMode { Collider, Tranform }

    public Transform transformTarget;
    public Collider colliderTarget;

    // Update is called once per frame
    void Update()
    {
        if (followMode == FollowMode.Tranform)
        {
            if (transformTarget != null)
            {
                transform.position = transformTarget.position;
            }
        }
        else if (followMode == FollowMode.Collider)
        {
            if (colliderTarget != null)
            {
                transform.position = colliderTarget.bounds.center;
            }
        }
    }
}
