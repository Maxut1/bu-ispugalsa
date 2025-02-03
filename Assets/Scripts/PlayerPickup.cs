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
        if (currentBattery != null)
        {
            heldBattery = currentBattery;
            heldBattery.PickUp(hand);
            isHolding = true;
            currentBattery = null; // Чтобы нельзя было взять другую
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
        }
    }
}
