using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;
    public Animator walkAnimator;        // Аниматор модели, которая ходит
    public GameObject normalModel;       // Модель, которая ходит
    public GameObject killerModel;       // Модель, которая бьёт
    public Animator killerAnimator;      // Аниматор модели, которая бьёт

    private bool isAttacking = false;
    private bool isDead = false;

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
        if (other.CompareTag("Player") && !isAttacking)
        {
            StartCoroutine(AttackPlayer(other.GetComponent<PlayerDeath>()));
        }
    }

    private IEnumerator AttackPlayer(PlayerDeath playerDeath)
    {
        isAttacking = true;
        agent.isStopped = true;
        walkAnimator.enabled = false;

        // Перемещаем KillerModel на позицию BooWalking
        killerModel.transform.SetPositionAndRotation(normalModel.transform.position, normalModel.transform.rotation);

        // Включаем модель убийства и отключаем ходьбу
        normalModel.SetActive(false);
        killerModel.SetActive(true);

        // Поворачиваем монстра к игроку
        Vector3 lookPos = player.position - transform.position;
        lookPos.y = 0;
        transform.rotation = Quaternion.LookRotation(lookPos);

        // Привязываем камеру к KillerModel
        Camera.main.transform.SetParent(killerModel.transform);
        Camera.main.transform.localPosition = new Vector3(34.4f, 21.3f, 392.2003f); // Координаты камеры
        Camera.main.transform.localRotation = Quaternion.Euler(-38.705f, -182.277f, 3.839f);

        // Отключаем игрока
        player.gameObject.SetActive(false);

        // Проигрываем анимацию удара
        killerAnimator.SetTrigger("Attack");

        // Ждем завершения анимации удара
        yield return new WaitForSeconds(killerAnimator.GetCurrentAnimatorStateInfo(0).length);

        // После завершения анимации вызовем метод Die() и перейдем на сцену "finish"
        playerDeath.Die();
    }

}