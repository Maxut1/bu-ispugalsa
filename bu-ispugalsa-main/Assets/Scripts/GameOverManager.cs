using UnityEngine;
using UnityEngine.SceneManagement;
using YG; // Подключаем Yandex SDK

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel; // Панель поражения

    private System.Action pendingAction; // Храним действие, которое нужно выполнить после рекламы

    void Start()
    {
        // Подписываемся на событие завершения рекламы
        YandexGame.RewardVideoEvent += OnAdFinished;
    }

    void OnDestroy()
    {
        // Отписываемся от события при уничтожении объекта
        YandexGame.RewardVideoEvent -= OnAdFinished;
    }

    public void ShowGameOverScreen()
    {
        Invoke("ActivateGameOverPanel", 2f); // Показываем панель через 2 секунды
    }

    void ActivateGameOverPanel()
    {
        gameOverPanel.SetActive(true);

        // Включаем курсор и делаем его видимым
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Метод для показа рекламы перед перезапуском
    public void RestartGame()
    {
        ShowAdBeforeAction(() =>
        {
            // После рекламы перезапускаем игру
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
    }

    // Метод для показа рекламы перед переходом в меню
    public void GoToMenu()
    {
        ShowAdBeforeAction(() =>
        {
            // После рекламы переходим в меню
            SceneManager.LoadScene(0); // Заменить на сцену главного меню
        });
    }

    // Метод для показа рекламы перед выполнением действия
    void ShowAdBeforeAction(System.Action action)
    {
        if (YandexGame.SDKEnabled)
        {
            pendingAction = action; // Запоминаем действие
            YandexGame.RewVideoShow(1); // Показываем рекламу
        }
        else
        {
            action?.Invoke(); // Если SDK не доступна, сразу выполняем действие
        }
    }

    // Колбек для завершения рекламы
    void OnAdFinished(int adStatus)
    {
        if (adStatus == 1) // Реклама была просмотрена
        {
            pendingAction?.Invoke(); // Выполняем отложенное действие после рекламы
        }
        pendingAction = null; // Очищаем переменную после выполнения действия
    }
}
