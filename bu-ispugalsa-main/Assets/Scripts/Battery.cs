using UnityEngine;

public class Battery : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;
    private bool isPickedUp = false; // ����, ������������, ��� ��������� � ����

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void PickUp(Transform hand)
    {
        // ���� ��������� ��� � ����, �� ��������� ��������
        if (isPickedUp) return;

        rb.isKinematic = true;
        col.enabled = false;
        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        isPickedUp = true; // ������������� ����, ��� ��������� � ����
    }

    public void Drop(Vector3 force)
    {
        if (!isPickedUp) return; // ���� ��������� �� � ����, �� ������ �� ������

        transform.SetParent(null);
        rb.isKinematic = false;
        col.enabled = true;
        rb.AddForce(force, ForceMode.Impulse);
        isPickedUp = false; // ���������� ����, ��������� ���������
    }
}

