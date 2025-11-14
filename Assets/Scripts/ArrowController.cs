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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        CapsuleCollider = GetComponent<CapsuleCollider>();
        initialBounceTimes = bounceTimes;
    }

    void Update()
    {
        if (!hasHit && rb.linearVelocity.sqrMagnitude > 0.01f)
            transform.forward = rb.linearVelocity.normalized;
    }

    void OnCollisionEnter(Collision collision)
    {
        // ----- BUTTON TRIGGER -----
        if (collision.collider.CompareTag("Button"))
        {
            ButtonTrigger bt = collision.collider.GetComponent<ButtonTrigger>();

            // Bounce match ¡ú activate the button
            if (bt != null && bounceTimes == bt.bounceTimes)
            {
                bt.triggerTarget();  // Activate logic

                // Play VFX at hit pos
                bt.PlayHitVFX(collision.contacts[0].point);

                // Arrow stops
                hasHit = true;
                rb.isKinematic = true;

                GetComponent<AudioSource>().clip = hitting;
                GetComponent<AudioSource>().Play();

                return;
            }
        }

        // ----- NORMAL BOUNCE -----
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
            // ----- FINAL LANDING -----
            SpawnFinalVFX(collision.contacts[0].point);

            rb.isKinematic = true;

            GetComponent<AudioSource>().clip = colli;
            GetComponent<AudioSource>().time = 0.2f;
            GetComponent<AudioSource>().Play();
        }
    }

    // VFX for bounce #1, #2, #3...
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

    // Final VFX
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
