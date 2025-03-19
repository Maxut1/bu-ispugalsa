using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GeneratorLogic : MonoBehaviour
{
    public int totalBatteries = 5;
    private int batteriesInserted = 0;
    public TextMeshProUGUI batteryCounterText;
    public Image flashImage;
    public float flashSpeed = 0.1f;

    public AudioSource backgroundMusic;
    public AudioClip newMusic;

    private bool isFlashing = false;

    private void Start()
    {
        UpdateBatteryCounter();
        if (flashImage != null)
        {
            flashImage.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Battery"))
        {
            Destroy(other.gameObject);
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
        if (batteryCounterText != null)
        {
            batteryCounterText.text = $"{batteriesInserted}/{totalBatteries}";
        }
    }

    private void EscapeSuccessful()
    {
        Debug.Log("Успех! Все батарейки собраны!");

        if (!isFlashing)
        {
            StartCoroutine(FlashScreen());
        }

        if (backgroundMusic != null && newMusic != null)
        {
            backgroundMusic.Stop();
            backgroundMusic.clip = newMusic;
            backgroundMusic.Play();
        }

        MoveExitDoors();
    }

    private void MoveExitDoors()
    {
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

    public void StopFlashing()
    {
        StopAllCoroutines();
        if (flashImage != null)
        {
            flashImage.gameObject.SetActive(false);
        }
        isFlashing = false;
    }

    private IEnumerator FlashScreen()
    {
        isFlashing = true;
        if (flashImage != null)
        {
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
}
