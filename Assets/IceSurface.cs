using UnityEngine;

public class IceSurface : MonoBehaviour
{
    [Header("Arrow Slide Settings")]
    public float slideFriction = 0.1f;       // Very slippery
    public float verticalDamping = 0.1f;     // Prevent sinking into the ground
    public float exitBounceForce = 6f;       // Upward force when leaving ice
}
