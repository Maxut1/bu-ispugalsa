using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // �������� ������������
    public float lookSpeed = 2f; // �������� �������� ������

    private CharacterController characterController;
    private Transform cameraTransform;
    private Vector3 moveDirection;

    private float xRotation = 0f; // ���� ������� ������

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked; // ������ ������
    }

    private void Update()
    {
        // ���������� ���������
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveDirection = transform.right * horizontal + transform.forward * vertical;
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        // ���������� �������
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        // ������������ ������ ������ (-80� �� 80�)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        // ��������� �������� � ������
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // ������� ��������� �����-������
        transform.Rotate(Vector3.up * mouseX);
    }
}
