using UnityEngine;

public class ArrowController : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasHit = false;
    public float bounceForce = 5f;
    public float bounceMultiplier = 1f;
    public int bounceTimes = 2;

    private int initialBounceTimes;
    private CapsuleCollider CapsuleCollider;
    public TrailerSource TrailerSource;

    public AudioClip colli;
    public AudioClip hitting;

    [Header("Bounce VFX")]
    public GameObject[] bounceVFXList;
    public float vfxLifetime = 2f;

    // ---------------- ICE LOGIC ----------------
    private bool onIce = false;
    private IceSurface currentIce = null;
    private Vector3 lastSlideVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        CapsuleCollider = GetComponent<CapsuleCollider>();
        initialBounceTimes = bounceTimes;
    }

    void Update()
    {
        // Arrow rotation while flying
        if (!hasHit && rb.linearVelocity.sqrMagnitude > 0.01f && !onIce)
        {
            transform.forward = rb.linearVelocity.normalized;
        }

        // ----- ICE SLIDING -----
        if (onIce && currentIce != null)
        {
            Vector3 vel = rb.linearVelocity;

            // Damp vertical so arrow stays on ice
            vel.y *= currentIce.verticalDamping;

            // Apply slippery friction
            vel *= 1f - currentIce.slideFriction * Time.deltaTime;

            rb.linearVelocity = vel;
            lastSlideVelocity = vel;

            // Arrow faces slide direction
            if (vel.sqrMagnitude > 0.01f)
                transform.forward = vel.normalized;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // ===== ICE SURFACE =====
        IceSurface ice = collision.collider.GetComponent<IceSurface>();
        if (ice != null && !hasHit)
        {
            onIce = true;
            currentIce = ice;

            // Lock Y velocity when landing on ice
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            return;
        }

        // ===== BUTTON TRIGGER =====
        if (collision.collider.CompareTag("Button"))
        {
            ButtonTrigger bt = collision.collider.GetComponent<ButtonTrigger>();

            if (bt != null && bounceTimes == bt.bounceTimes)
            {
                bt.triggerTarget();
                bt.PlayHitVFX(collision.contacts[0].point);

                hasHit = true;
                rb.isKinematic = true;

                GetComponent<AudioSource>().clip = hitting;
                GetComponent<AudioSource>().Play();

                return;
            }
        }

        // ===== NORMAL BOUNCE =====
        if (bounceTimes > 0 && !hasHit)
        {
            TrailerSource.changeColor(bounceTimes - 1);

            Vector3 normal = collision.contacts[0].normal;
            Vector3 bounceDirection = Vector3.Reflect(rb.linearVelocity, normal);
            bounceDirection += Vector3.up * bounceForce;

            rb.linearVelocity = bounceDirection * bounceMultiplier;

            SpawnBounceVFX(collision.contacts[0].point);

            bounceTimes--;

            GetComponent<AudioSource>().clip = colli;
            GetComponent<AudioSource>().time = 0.2f;
            GetComponent<AudioSource>().Play();
        }
        else
        {
            // ===== FINAL LANDING =====
            SpawnFinalVFX(collision.contacts[0].point);
            rb.isKinematic = true;

            GetComponent<AudioSource>().clip = colli;
            GetComponent<AudioSource>().time = 0.2f;
            GetComponent<AudioSource>().Play();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // ----- EXITING ICE -----
        if (onIce && collision.collider.GetComponent<IceSurface>() != null)
        {
            if (currentIce != null)
            {
                rb.linearVelocity = lastSlideVelocity + Vector3.up * currentIce.exitBounceForce;
            }

            onIce = false;
            currentIce = null;
        }
    }

    // ---------------- VFX METHODS ----------------

    void SpawnBounceVFX(Vector3 pos)
    {
        int index = initialBounceTimes - bounceTimes;

        if (index >= 0 && index < bounceVFXList.Length)
        {
            GameObject prefab = bounceVFXList[index];
            if (prefab == null) return;

            GameObject vfx = Instantiate(prefab, pos, Quaternion.identity);
            Destroy(vfx, vfxLifetime);
        }
    }

    void SpawnFinalVFX(Vector3 pos)
    {
        int index = bounceVFXList.Length - 1;

        if (index >= 0 && index < bounceVFXList.Length)
        {
            GameObject prefab = bounceVFXList[index];
            if (prefab == null) return;

            GameObject vfx = Instantiate(prefab, pos, Quaternion.identity);
            Destroy(vfx, vfxLifetime);
        }
    }
}
