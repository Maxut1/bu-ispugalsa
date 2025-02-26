using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TypingText : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro; // Ссылка на компонент TextMeshPro
    public string[] sentences; // Массив предложений
    public float typingSpeed = 0.05f; // Скорость печати
    public AudioClip[] letterSounds; // Массив звуков для букв
    public AudioClip[] wordSounds;   // Массив звуков для слов
    private int currentSentenceIndex = 0; // Индекс текущего предложения
    private AudioSource audioSource; // Источник звука
    public string Scene; // Сцена на которую следует перемещаться после текста

    void Start()
    {
        // Получаем компонент AudioSource
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(DisplayText());
    }

    void Update()
    {
        // Меняем текст при нажатии на экран
        if (Input.GetMouseButtonDown(0))
        {
            if (currentSentenceIndex < sentences.Length - 1)
            {
                currentSentenceIndex++;
                StartCoroutine(DisplayText());
            }
            else
            {
                SceneManager.LoadScene(Scene);
            }
        }
    }

    IEnumerator DisplayText()
    {
        textMeshPro.text = ""; // Очищаем текст перед началом печати нового предложения

        // Разделяем предложение на слова
        string[] words = sentences[currentSentenceIndex].Split(' ');

        foreach (string word in words)
        {
            // Печатаем каждую букву в слове
            foreach (char letter in word.ToCharArray())
            {
                textMeshPro.text += letter;

                // Проигрываем звук для буквы, если есть
                if (letterSounds.Length > 0)
                {
                    audioSource.PlayOneShot(letterSounds[Random.Range(0, letterSounds.Length)]);
                }

                yield return new WaitForSeconds(typingSpeed);
            }

            // Добавляем паузу после каждого слова
            yield return new WaitForSeconds(0.2f); // Пауза между словами (настраиваемая)

            // Проигрываем звук для слова, если есть
            if (wordSounds.Length > 0)
            {
                audioSource.PlayOneShot(wordSounds[Random.Range(0, wordSounds.Length)]);
            }

            // Добавляем пробел между словами
            textMeshPro.text += " ";
        }
    }
}
