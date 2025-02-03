using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Скорость передвижения
    public float lookSpeed = 2f; // Скорость вращения камеры

    private CharacterController characterController;
    private Transform cameraTransform;
    private Vector3 moveDirection;

    private float xRotation = 0f; // Угол наклона камеры

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked; // Прячем курсор
    }

    private void Update()
    {
        // Управление движением
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveDirection = transform.right * horizontal + transform.forward * vertical;
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Управление камерой
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        // Ограничиваем наклон камеры (-80° до 80°)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        // Применяем вращение к камере
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Вращаем персонажа влево-вправо
        transform.Rotate(Vector3.up * mouseX);
    }
}
