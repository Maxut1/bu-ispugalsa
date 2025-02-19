using UnityEngine;
using UnityEngine.UI;

public class PlayerPickup : MonoBehaviour
{
    public float pickupRange = 2f; // Максимальное расстояние для взаимодействия
    public Transform hand; // Куда кладём предмет
    public Image crosshair; // Прицел
    private Battery currentBattery = null;
    private Battery heldBattery = null;
    private bool isHolding = false;
    private float holdTime = 0f; // Таймер для броска
    private float throwThreshold = 0.5f; // Время удержания для броска

    void Update()
    {
        CheckForBattery();

        // Логгируем состояние isHolding перед проверкой
        Debug.Log("isHolding перед проверкой: " + isHolding);

        // Если игрок держит батарейку, отслеживаем зажатие для броска
        if (isHolding && Input.GetMouseButton(0))
        {
            holdTime += Time.deltaTime;
            if (holdTime >= throwThreshold)
            {
                DropBattery();
                holdTime = 0f; // Сбрасываем таймер
            }
        }

        // Если игрок не держит батарейку, проверяем клик на подъем
        if (!isHolding && Input.GetMouseButtonDown(0) && currentBattery != null)
        {
            Debug.Log("Пытаемся поднять батарейку");
            PickUpBattery();
        }

        // Если игрок отпустил кнопку, но не держал достаточно долго — сбрасываем таймер
        if (Input.GetMouseButtonUp(0))
        {
            holdTime = 0f;
        }
    }

    void CheckForBattery()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * pickupRange, Color.green); // Визуализируем луч

        if (Physics.Raycast(ray, out hit, pickupRange))
        {
            Battery battery = hit.collider.GetComponent<Battery>();
            if (battery != null)
            {
                currentBattery = battery;
                crosshair.color = Color.green; // Изменяем цвет прицела
                return;
            }
        }

        // Если батарейки перед прицелом нет — сбрасываем
        currentBattery = null;
        crosshair.color = Color.red;
    }

    void PickUpBattery()
    {
        if (isHolding) // Если батарейка уже в руке
        {
            Debug.Log("Батарейка уже в руке.");
            return;
        }

        if (currentBattery != null)
        {
            Debug.Log("Поднята батарейка: " + currentBattery.name); // Выводим информацию о батарейке
            heldBattery = currentBattery;
            heldBattery.PickUp(hand);
            isHolding = true;
            currentBattery = null; // Чтобы нельзя было взять другую
            Debug.Log("isHolding после поднятия: " + isHolding); // Логгируем после поднятия
        }
    }

    void DropBattery()
    {
        if (heldBattery != null)
        {
            Vector3 throwForce = transform.forward * 5f + transform.up * 2f;
            heldBattery.Drop(throwForce);
            isHolding = false;
            heldBattery = null;
            Debug.Log("isHolding после выброса: " + isHolding); // Логгируем после выброса
        }
    }
}
