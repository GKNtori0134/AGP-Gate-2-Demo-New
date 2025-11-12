using UnityEngine;

public class ArrowController : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasHit = false; // �Ƿ��Ѿ�����
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
        // �ü�ʸ�ķ���ʼ�յ������ٶȷ���
        if (!hasHit && rb.linearVelocity.sqrMagnitude > 0.01f) // ֻ���ڼ�ʸδ�������ٶȲ�Ϊ0ʱ��������
        {
            transform.forward = rb.linearVelocity.normalized;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        /*if (!hasHit)
        {
            hasHit = true;
            rb.velocity = Vector3.zero; // ֹͣ��ʸ�˶�
            rb.isKinematic = true; // �ü�ʸ�̶�����
            transform.parent = collision.transform; // �ü����������е�������
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

            // ��ȡ��ײ�㷨�߷���
            Vector3 normal = collision.contacts[0].normal;

            // ���㷴�䷽��
            Vector3 bounceDirection = Vector3.Reflect(rb.linearVelocity, normal);

            // ʩ�ӷ��������������ϵ�ƫ��
            bounceDirection += Vector3.up * bounceForce;

            // Ӧ���µ��ٶ�
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
