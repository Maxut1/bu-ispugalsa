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
        isDead = true;
        if (isDead) return;

        Debug.Log("Игрок погиб!");
        SceneManager.LoadScene("finish");
    }
}
