using UnityEngine;

public class BowController : MonoBehaviour
{
    public Transform playerCamera; // Player camera
    public Transform arrowSpawnPoint; // Arrow spawn point
    public GameObject arrowPrefab; // Arrow prefab
    public float arrowSpeed = 10f; // Arrow speed

    public float followSpeed = 5f; // Speed of following the player's rotation
    public float positionFollowSpeed = 5f; // Speed of following the player's position
    public Vector3 positionOffset = new Vector3(0.3f, -0.2f, 0.5f); // Position offset of the bow

    void Update()
    {
        FollowPlayerView();

        if (Input.GetMouseButtonDown(0)) // Shoot the arrow
        {
            ShootArrow();
        }
    }

    void FollowPlayerView()
    {
        // **Follow the player's view**
        Vector3 targetPosition = playerCamera.position + playerCamera.TransformDirection(positionOffset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, positionFollowSpeed * Time.deltaTime);

        // **Follow the player's rotation**
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
