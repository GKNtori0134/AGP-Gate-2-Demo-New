using UnityEngine;

public class ArrowController : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasHit = false; // Whether the arrow has hit something
    public float bounceForce = 5f;
    public float bounceMultiplier = 1f;
    public int bounceTimes = 2;
    private CapsuleCollider CapsuleCollider;
    public TrailerSource TrailerSource;

    public AudioClip colli;
    public AudioClip hitting;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        CapsuleCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        // The initial velocity direction of the arrow
        if (!hasHit && rb.linearVelocity.sqrMagnitude > 0.01f) // Only when the arrow's velocity is not 0, the arrow will move
        {
            transform.forward = rb.linearVelocity.normalized;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        /*if (!hasHit)
        {
            hasHit = true;
            rb.velocity = Vector3.zero; // Stop the arrow's movement
            rb.isKinematic = true; // Make the arrow kinematic
            transform.parent = collision.transform; // Make the arrow a child of the object it hit
        }*/

        if (collision.collider.CompareTag("Button") && bounceTimes == collision.collider.GetComponent<ButtonTrigger>().bounceTimes)
        {
            collision.collider.GetComponent<ButtonTrigger>().triggerTarget(); 
            hasHit = true;
            GetComponent<AudioSource>().clip = hitting;
            GetComponent<AudioSource>().Play();
            rb.isKinematic = true;
            return;
        }

        if (bounceTimes > 0 && !hasHit)
        {

            TrailerSource.changeColor(bounceTimes-1);

            // Get the normal of the collision
            Vector3 normal = collision.contacts[0].normal;

            // Calculate the bounce direction
            Vector3 bounceDirection = Vector3.Reflect(rb.linearVelocity, normal);

            // Apply the bounce force to the arrow
            bounceDirection += Vector3.up * bounceForce;

            // Apply the new velocity
            rb.linearVelocity = bounceDirection * bounceMultiplier;

            bounceTimes--;

            GetComponent<AudioSource>().clip = colli;
            GetComponent<AudioSource>().time = 0.2f;
            GetComponent<AudioSource>().Play();

        } else
        {
            //CapsuleCollider.enabled = false;
            rb.isKinematic = true;
            GetComponent<AudioSource>().clip = colli;
            GetComponent<AudioSource>().time = 0.2f;
            GetComponent<AudioSource>().Play();
        }
    }
}
