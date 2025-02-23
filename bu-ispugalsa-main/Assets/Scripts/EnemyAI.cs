using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    public Transform player;  // ������ �� ������
    public NavMeshAgent agent;
    public Animator monsterAnimator;

    private bool isAttacking = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        switch (DifficultyManager.Difficulty)
        {
            case 0: // Easy
                agent.speed = 3f;
                break;
            case 1: // Average
                agent.speed = 4f;
                break;
            case 2: // Hard
                agent.speed = 5f;
                break;
        }
    }

    private void Update()
    {
        if (player != null && !isAttacking)
        {
            agent.SetDestination(player.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(AttackPlayer(other));
        }
    }

    private IEnumerator AttackPlayer(Collider player)
    {
        isAttacking = true;
        agent.isStopped = true; // ������������� �������
        monsterAnimator.SetBool("isAttacking", true);

        yield return new WaitForSeconds(6f); // ��� ���������� �������� ����� (������� �� ������������ ��������)

        // ����� ����� �������� ������ ������ ������
        Debug.Log("����� ����!");
        SceneManager.LoadScene("GameOver");

        monsterAnimator.SetBool("isAttacking", false);
        agent.isStopped = false;
        isAttacking = false;
    }
}
