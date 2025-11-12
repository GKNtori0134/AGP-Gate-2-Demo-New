using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;

    private Rigidbody rb;
    private Camera playerCamera;
    private float rotationX = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();

        // ��������꣬ʹ�䲻�ɼ����̶�����Ļ����
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");  // A, D �����ƶ�
        float moveZ = Input.GetAxis("Vertical");    // W, S ǰ���ƶ�

        // �����ƶ�����
        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        moveDirection *= moveSpeed;

        // �� Rigidbody ���������ƶ�
        rb.linearVelocity = new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // ��ת���ˮƽ�ӽ�
        transform.Rotate(Vector3.up * mouseX);

        // ��ת�������ֱ�ӽ�
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); // ���������ӽǷ�Χ
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }
}
