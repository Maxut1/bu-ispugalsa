using System.Collections;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public Camera playerCamera;
    public Transform monster;
    public float deathCameraTurnSpeed = 2f;

    private bool isDead = false;

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        StartCoroutine(HandleDeath());
    }

    private IEnumerator HandleDeath()
    {
        // Останавливаем движение игрока
        GetComponent<PlayerMovement>().enabled = false;

        // Плавно поворачиваем камеру к монстру
        while (true)
        {
            Vector3 directionToMonster = (monster.position - playerCamera.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(directionToMonster);
            playerCamera.transform.rotation = Quaternion.Slerp(playerCamera.transform.rotation, lookRotation, Time.deltaTime * deathCameraTurnSpeed);

            // Проверяем, достаточно ли камера повернулась
            if (Quaternion.Angle(playerCamera.transform.rotation, lookRotation) < 1f)
            {
                break;
            }

            yield return null;
        }

        // Проигрываем анимацию атаки монстра
        Animator monsterAnimator = monster.GetComponent<Animator>();
        if (monsterAnimator != null)
        {
            monsterAnimator.SetTrigger("Attack"); // Убедись, что триггер "Attack" есть в анимациях монстра
        }

        // Ждем завершения анимации (предположим, она длится 3 секунды)
        yield return new WaitForSeconds(3f);

        // Здесь можно добавить, например, переход на экран "Game Over"
        Debug.Log("Игрок погиб!");
    }
}
