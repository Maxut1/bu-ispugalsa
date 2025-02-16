using UnityEngine;

public class Battery : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;
    private bool isPickedUp = false; // Флаг, показывающий, что батарейка в руке

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void PickUp(Transform hand)
    {
        // Если батарейка уже в руке, не выполняем действие
        if (isPickedUp) return;

        rb.isKinematic = true;
        col.enabled = false;
        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        isPickedUp = true; // Устанавливаем флаг, что батарейка в руке
        Debug.Log("Батарейка в руке");
    }

    public void Drop(Vector3 force)
    {
        if (!isPickedUp) return; // Если батарейка не в руке, то ничего не делаем

        transform.SetParent(null);
        rb.isKinematic = false;
        col.enabled = true;
        rb.AddForce(force, ForceMode.Impulse);
        isPickedUp = false; // Сбрасываем флаг, батарейка выброшена
        Debug.Log("Батарейка выбросилась");
    }
}
