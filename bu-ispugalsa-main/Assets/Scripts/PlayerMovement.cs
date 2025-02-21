using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // �������� ������������
    public float lookSpeed = 2f;  // �������� �������� ������

    public float bobbingSpeed = 10f;  // �������� ����������� ������
    public float bobbingAmount = 0.08f;  // ��������� ����������� �����-����
    public float swayAmount = 0.05f;  // ��������� ����������� ������-�����

    private CharacterController characterController;
    private Transform cameraTransform;
    private Vector3 moveDirection;

    private float xRotation = 0f; // ���� ������� ������
    private float bobbingTimer = 0f;
    private float defaultCameraY; // �������� ������ ������
    private float defaultCameraX; // �������� �������� ������ �� X

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked; // ������ ������

        defaultCameraY = cameraTransform.localPosition.y; // ���������� �������� ������ ������
        defaultCameraX = cameraTransform.localPosition.x; // ���������� �������� �������� �� X
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

        // ������ ����������� ������ (�����-���� � ������-�����)
        if (horizontal != 0 || vertical != 0) // ���� ����� ���������
        {
            bobbingTimer += Time.deltaTime * bobbingSpeed;

            // ����������� �����-����
            float newY = defaultCameraY + Mathf.Sin(bobbingTimer) * bobbingAmount;

            // ����������� ������-����� (��������� ������������ ������)
            float newX = defaultCameraX + Mathf.Cos(bobbingTimer / 2) * swayAmount;

            cameraTransform.localPosition = new Vector3(newX, newY, cameraTransform.localPosition.z);
        }
        else // ���� ����� �����, ���������� ������ � �������� ���������
        {
            bobbingTimer = 0;
            cameraTransform.localPosition = new Vector3(defaultCameraX, defaultCameraY, cameraTransform.localPosition.z);
        }
    }
}
