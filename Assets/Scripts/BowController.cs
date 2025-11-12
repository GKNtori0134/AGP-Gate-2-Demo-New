using UnityEngine;

public class BowController : MonoBehaviour
{
    public Transform playerCamera; // ��������
    public Transform arrowSpawnPoint; // ��������λ��
    public GameObject arrowPrefab; // ����Ԥ����
    public float arrowSpeed = 10f; // �����ٶ�

    public float followSpeed = 5f; // Lerp �ٶȣ����ƹ���תƽ���ȣ�
    public float positionFollowSpeed = 5f; // Lerp �ٶȣ����ƹ�λ��ƽ���ȣ�
    public Vector3 positionOffset = new Vector3(0.3f, -0.2f, 0.5f); // ��������������λ��ƫ����

    void Update()
    {
        FollowPlayerView();

        if (Input.GetMouseButtonDown(0)) // ����������
        {
            ShootArrow();
        }
    }

    void FollowPlayerView()
    {
        // **�ù���λ��ƽ�����������**
        Vector3 targetPosition = playerCamera.position + playerCamera.TransformDirection(positionOffset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, positionFollowSpeed * Time.deltaTime);

        // **�ù��ķ���ƽ�����������**
        Quaternion targetRotation = Quaternion.LookRotation(playerCamera.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, followSpeed * Time.deltaTime);
    }

    void ShootArrow()
    {
        GameObject arrowInstance = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);

        Rigidbody arrowRb = arrowInstance.GetComponent<Rigidbody>();
        arrowRb.linearVelocity = arrowSpawnPoint.forward * arrowSpeed;
    }
}
