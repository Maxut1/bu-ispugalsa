using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorLogic : MonoBehaviour
{
    public int totalBatteries = 5; // Общее количество батареек
    private int batteriesInserted = 0;
    public TextMeshProUGUI batteryCounterText;

    private void Start()
    {
        UpdateBatteryCounter();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Battery"))
        {
            Destroy(other.gameObject); // Удаляем батарейку
            batteriesInserted++;
            UpdateBatteryCounter();

            if (batteriesInserted >= totalBatteries)
            {
                EscapeSuccessful();
            }
        }
    }

    private void UpdateBatteryCounter()
    {
        batteryCounterText.text = $"{batteriesInserted}/{totalBatteries}";
    }

    private void EscapeSuccessful()
    {
        Debug.Log("Побег совершен! Игрок выиграл!");

        // Двигаем объекты дверей для открытия
        GameObject exit1 = GameObject.FindWithTag("Exit1");
        GameObject exit2 = GameObject.FindWithTag("Exit2");

        if (exit1 != null)
        {
            exit1.transform.position += new Vector3(0, 0, -1.5f);
        }

        if (exit2 != null)
        {
            exit2.transform.position += new Vector3(0, 0, 1.5f);
        }
    }
}