using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour
{


    // Этот метод вызывается при входе в триггер
    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что объект, вошедший в триггер, — это игрок
        if (other.CompareTag("Player"))
        {
            // Загружаем сцену
            SceneManager.LoadScene(1);
        }
    }
}
