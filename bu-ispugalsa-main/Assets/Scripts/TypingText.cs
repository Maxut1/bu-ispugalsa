using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // ✅ Добавлено!
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
    private bool isTyping = true; // Флаг, указывающий, печатается ли текст в данный момент

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
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                // Если текст еще печатается, то завершаем его печать
                StopAllCoroutines();
                textMeshPro.text = sentences[currentSentenceIndex]; // Завершаем текст
                isTyping = false; // Устанавливаем флаг, что текст больше не печатается
            }
            else
            {
                // Если текст уже завершен, переходим к следующему предложению
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
    }

    IEnumerator DisplayText()
    {
        if (currentSentenceIndex >= sentences.Length)
        {
            Debug.LogError("⚠ Ошибка: currentSentenceIndex больше, чем длина массива sentences.");
            yield break;
        }

        isTyping = true; // Устанавливаем флаг, что текст печатается
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

        isTyping = false; // Устанавливаем флаг, что текст больше не печатается
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
            pendingAction = action; // Запоминаем действие, которое нужно выполнить после рекламы
            YandexGame.FullscreenShow(); // Показываем обычную полноэкранную рекламу
            Invoke(nameof(ExecutePendingAction), 1.5f); // Запускаем действие после рекламы с задержкой
        }
        else
        {
            action?.Invoke(); // Если SDK не работает, сразу выполняем действие
        }
    }

    void ExecutePendingAction()
    {
        pendingAction?.Invoke(); // Выполняем отложенное действие
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