using UnityEngine;

public class BowController : MonoBehaviour
{
    public Transform playerCamera;
    public Transform arrowSpawnPoint;
    public GameObject arrowPrefab;
    public float arrowSpeed = 10f;

    public float followSpeed = 5f;
    public float positionFollowSpeed = 5f;
    public Vector3 positionOffset = new Vector3(0.3f, -0.2f, 0.5f);

    [Header("Shooting Angle")]
    public float extraShootAngle = 10f; // degrees upward

    void Update()
    {
        FollowPlayerView();

        if (Input.GetMouseButtonDown(0))
        {
            ShootArrow();
        }
    }

    void FollowPlayerView()
    {
        // Follow camera position
        Vector3 targetPosition = playerCamera.position + playerCamera.TransformDirection(positionOffset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, positionFollowSpeed * Time.deltaTime);

        // Apply fixed upward angle relative to camera
        Vector3 forwardDir =
            playerCamera.TransformDirection(
                Quaternion.Euler(-extraShootAngle, 0, 0) * Vector3.forward
            );

        Quaternion targetRotation = Quaternion.LookRotation(forwardDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, followSpeed * Time.deltaTime);
    }

    void ShootArrow()
    {
        GameObject arrowInstance = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);

        Rigidbody arrowRb = arrowInstance.GetComponent<Rigidbody>();
        arrowRb.linearVelocity = arrowSpawnPoint.forward * arrowSpeed;
    }
}
