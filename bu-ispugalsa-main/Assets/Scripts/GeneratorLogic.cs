using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GeneratorLogic : MonoBehaviour
{
    public int totalBatteries = 5; // Общее количество батарей
    private int batteriesInserted = 0;
    public TextMeshProUGUI batteryCounterText;
    public Image flashImage; // UI-элемент для мерцания
    public float flashSpeed = 0.1f; // Скорость мерцания

    public AudioSource backgroundMusic; // Фоновая музыка
    public AudioClip newMusic; // Музыка, которая включится при сборе всех батареек

    private bool isFlashing = false; // Флаг для управления мерцанием

    private void Start()
    {
        UpdateBatteryCounter();
        flashImage.gameObject.SetActive(false); // Отключаем мерцание в начале
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Battery"))
        {
            Destroy(other.gameObject); // Удаление батарейки
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
        Debug.Log("Успех! Все батарейки собраны!");

        if (!isFlashing)
        {
            StartCoroutine(FlashScreen());
        }

        // Меняем музыку
        if (backgroundMusic != null && newMusic != null)
        {
            backgroundMusic.Stop(); // Останавливаем текущую музыку
            backgroundMusic.clip = newMusic;
            backgroundMusic.Play(); // Включаем новую музыку
        }

        // Двигаем выходные двери
        GameObject exit1 = GameObject.FindWithTag("Exit1");
        GameObject exit2 = GameObject.FindWithTag("Exit2");

        if (exit1 != null)
        {
            exit1.transform.position += new Vector3(0, 0, -1f);
        }

        if (exit2 != null)
        {
            exit2.transform.position += new Vector3(0, 0, 1f);
        }
    }

    private IEnumerator FlashScreen()
    {
        isFlashing = true;
        flashImage.gameObject.SetActive(true);
        while (true)
        {
            flashImage.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(flashSpeed);
            flashImage.color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds(flashSpeed);
        }
    }
}
