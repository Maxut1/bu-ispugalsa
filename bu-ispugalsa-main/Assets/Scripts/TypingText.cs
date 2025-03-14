using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG; // Подключаем Yandex SDK

public class TypingText : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public string[] sentences;
    public float typingSpeed = 0.05f;
    public AudioClip[] letterSounds;
    public AudioClip[] wordSounds;
    private int currentSentenceIndex = 0;
    private AudioSource audioSource;
    public string Scene;

    public GameObject winPanel;
    public Button restartButton;
    public Button menuButton;

    private System.Action pendingAction; // Храним действие, которое нужно выполнить после рекламы

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (sentences == null || sentences.Length == 0)
        {
            Debug.LogError("⚠ Ошибка: массив sentences пуст!");
            return;
        }

        StartCoroutine(DisplayText());

        if (winPanel != null)
        {
            winPanel.SetActive(false);
            restartButton.onClick.AddListener(() => ShowAdBeforeAction(RestartGame));
            menuButton.onClick.AddListener(() => ShowAdBeforeAction(GoToMenu));
        }

        // Подписываемся на событие завершения рекламы
        YandexGame.RewardVideoEvent += OnAdFinished;
    }

    void OnDestroy()
    {
        // Отписываемся от события при уничтожении объекта
        YandexGame.RewardVideoEvent -= OnAdFinished;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentSentenceIndex < sentences.Length - 1)
            {
                currentSentenceIndex++;
                StartCoroutine(DisplayText());
            }
            else
            {
                if (winPanel != null)
                {
                    ShowWinPanel();
                }
                else
                {
                    SceneManager.LoadScene(Scene);
                }
            }
        }
    }

    IEnumerator DisplayText()
    {
        if (currentSentenceIndex >= sentences.Length)
        {
            Debug.LogError("⚠ Ошибка: currentSentenceIndex больше, чем длина массива sentences.");
            yield break;
        }

        textMeshPro.text = "";
        string[] words = sentences[currentSentenceIndex].Split(' ');

        foreach (string word in words)
        {
            foreach (char letter in word.ToCharArray())
            {
                textMeshPro.text += letter;
                if (letterSounds.Length > 0)
                    audioSource.PlayOneShot(letterSounds[Random.Range(0, letterSounds.Length)]);

                yield return new WaitForSeconds(typingSpeed);
            }

            yield return new WaitForSeconds(0.2f);
            if (wordSounds.Length > 0)
                audioSource.PlayOneShot(wordSounds[Random.Range(0, wordSounds.Length)]);

            textMeshPro.text += " ";
        }
    }

    void ShowWinPanel()
    {
        winPanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void ShowAdBeforeAction(System.Action action)
    {
        if (YandexGame.SDKEnabled)
        {
            pendingAction = action; // Запоминаем, какое действие выполнить после рекламы
            YandexGame.RewVideoShow(1); // Показываем рекламу
        }
        else
        {
            action?.Invoke(); // Если SDK не работает, сразу выполняем действие
        }
    }

    void OnAdFinished(int adStatus)
    {
        if (adStatus == 1) // Проверяем, была ли реклама просмотрена
        {
            pendingAction?.Invoke(); // Выполняем отложенное действие после рекламы
        }
        pendingAction = null; // Очищаем переменную
    }

    void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    void GoToMenu()
    {
        SceneManager.LoadScene(0); // Заменить на свою сцену меню
    }
}
