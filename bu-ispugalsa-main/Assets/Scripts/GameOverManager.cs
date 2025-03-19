using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverCanvas;
    public Button restartButton;
    public Button menuButton;
    public Image screenFlash;
    public AudioSource gameMusic;
    public AudioSource flashSound;
    public Animator flashAnimator;

    private bool isGameOver = false;

    void Start()
    {
        if (gameOverCanvas == null)
        {
            Debug.LogError("GameOverCanvas не назначен в Inspector!");
        }
        else
        {
            gameOverCanvas.SetActive(false);
        }

        if (restartButton != null) restartButton.onClick.AddListener(RestartGame);
        if (menuButton != null) menuButton.onClick.AddListener(GoToMenu);
    }

    public void PlayerDied()
    {
        if (isGameOver) return;
        isGameOver = true;

        StopAllEffects();
        gameOverCanvas.SetActive(true);

        if (restartButton != null) restartButton.interactable = true;
        if (menuButton != null) menuButton.interactable = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void StopAllEffects()
    {
        if (screenFlash != null)
        {
            screenFlash.color = new Color(0, 0, 0, 0);
        }

        if (gameMusic != null && gameMusic.isPlaying)
        {
            gameMusic.Stop();
        }

        if (flashSound != null && flashSound.isPlaying)
        {
            flashSound.Stop();
        }

        if (flashAnimator != null)
        {
            flashAnimator.enabled = false;
        }

        GeneratorLogic generator = FindObjectOfType<GeneratorLogic>();
        if (generator != null)
        {
            generator.StopFlashing();
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
