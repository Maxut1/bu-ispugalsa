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
        // ������������� �������� ������
        GetComponent<PlayerMovement>().enabled = false;

        // ������ ������������ ������ � �������
        while (true)
        {
            Vector3 directionToMonster = (monster.position - playerCamera.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(directionToMonster);
            playerCamera.transform.rotation = Quaternion.Slerp(playerCamera.transform.rotation, lookRotation, Time.deltaTime * deathCameraTurnSpeed);

            // ���������, ���������� �� ������ �����������
            if (Quaternion.Angle(playerCamera.transform.rotation, lookRotation) < 1f)
            {
                break;
            }

            yield return null;
        }

        // ����������� �������� ����� �������
        Animator monsterAnimator = monster.GetComponent<Animator>();
        if (monsterAnimator != null)
        {
            monsterAnimator.SetTrigger("Attack"); // �������, ��� ������� "Attack" ���� � ��������� �������
        }

        // ���� ���������� �������� (�����������, ��� ������ 3 �������)
        yield return new WaitForSeconds(3f);

        // ����� ����� ��������, ��������, ������� �� ����� "Game Over"
        Debug.Log("����� �����!");
    }
}
