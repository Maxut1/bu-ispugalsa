using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Скорость передвижения
    public float lookSpeed = 2f;  // Скорость вращения камеры

    public float bobbingSpeed = 10f;  // Скорость покачивания камеры
    public float bobbingAmount = 0.08f;  // Амплитуда покачивания вверх-вниз
    public float swayAmount = 0.05f;  // Амплитуда покачивания вправо-влево

    private CharacterController characterController;
    private Transform cameraTransform;
    private Vector3 moveDirection;

    private float xRotation = 0f; // Угол наклона камеры
    private float bobbingTimer = 0f;
    private float defaultCameraY; // Исходная высота камеры
    private float defaultCameraX; // Исходное смещение камеры по X

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked; // Прячем курсор

        defaultCameraY = cameraTransform.localPosition.y; // Запоминаем исходную высоту камеры
        defaultCameraX = cameraTransform.localPosition.x; // Запоминаем исходное смещение по X
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

        // Эффект покачивания камеры (вверх-вниз и вправо-влево)
        if (horizontal != 0 || vertical != 0) // Если игрок двигается
        {
            bobbingTimer += Time.deltaTime * bobbingSpeed;

            // Покачивание вверх-вниз
            float newY = defaultCameraY + Mathf.Sin(bobbingTimer) * bobbingAmount;

            // Покачивание вправо-влево (имитируем раскачивание головы)
            float newX = defaultCameraX + Mathf.Cos(bobbingTimer / 2) * swayAmount;

            cameraTransform.localPosition = new Vector3(newX, newY, cameraTransform.localPosition.z);
        }
        else // Если игрок стоит, возвращаем камеру в исходное положение
        {
            bobbingTimer = 0;
            cameraTransform.localPosition = new Vector3(defaultCameraX, defaultCameraY, cameraTransform.localPosition.z);
        }
    }
}
