using UnityEngine;
using UnityEngine.UI;

public class PlayerPickup : MonoBehaviour
{
    public float pickupRange = 2f; // ������������ ���������� ��� ��������������
    public Transform hand; // ���� ����� �������
    public Image crosshair; // ������
    private Battery currentBattery = null;
    private Battery heldBattery = null;
    private bool isHolding = false;
    private float holdTime = 0f; // ������ ��� ������
    private float throwThreshold = 0.5f; // ����� ��������� ��� ������

    void Update()
    {
        CheckForBattery();

        // ��������� ��������� isHolding ����� ���������
        Debug.Log("isHolding ����� ���������: " + isHolding);

        // ���� ����� ������ ���������, ����������� ������� ��� ������
        if (isHolding && Input.GetMouseButton(0))
        {
            holdTime += Time.deltaTime;
            if (holdTime >= throwThreshold)
            {
                DropBattery();
                holdTime = 0f; // ���������� ������
            }
        }

        // ���� ����� �� ������ ���������, ��������� ���� �� ������
        if (!isHolding && Input.GetMouseButtonDown(0) && currentBattery != null)
        {
            Debug.Log("�������� ������� ���������");
            PickUpBattery();
        }

        // ���� ����� �������� ������, �� �� ������ ���������� ����� � ���������� ������
        if (Input.GetMouseButtonUp(0))
        {
            holdTime = 0f;
        }
    }

    void CheckForBattery()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * pickupRange, Color.green); // ������������� ���

        if (Physics.Raycast(ray, out hit, pickupRange))
        {
            Battery battery = hit.collider.GetComponent<Battery>();
            if (battery != null)
            {
                currentBattery = battery;
                crosshair.color = Color.green; // �������� ���� �������
                return;
            }
        }

        // ���� ��������� ����� �������� ��� � ����������
        currentBattery = null;
        crosshair.color = Color.red;
    }

    void PickUpBattery()
    {
        if (isHolding) // ���� ��������� ��� � ����
        {
            Debug.Log("��������� ��� � ����.");
            return;
        }

        if (currentBattery != null)
        {
            Debug.Log("������� ���������: " + currentBattery.name); // ������� ���������� � ���������
            heldBattery = currentBattery;
            heldBattery.PickUp(hand);
            isHolding = true;
            currentBattery = null; // ����� ������ ���� ����� ������
            Debug.Log("isHolding ����� ��������: " + isHolding); // ��������� ����� ��������
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
            Debug.Log("isHolding ����� �������: " + isHolding); // ��������� ����� �������
        }
    }
}
