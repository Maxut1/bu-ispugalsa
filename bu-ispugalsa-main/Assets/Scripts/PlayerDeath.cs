using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    public Transform monster;
    public Transform killer;
    public float deathCameraTurnSpeed = 2f;

    private bool isDead = false;

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        FindObjectOfType<GameOverManager>().ShowGameOverScreen();
        // SceneManager.LoadScene("GameOver");
    }
}
