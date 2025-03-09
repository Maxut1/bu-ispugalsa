using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;
    public Animator walkAnimator;    
    public GameObject normalModel;      
    public GameObject killerModel;      
    public Animator killerAnimator;     
    public Transform[] patrolPoints; // Точки патрулирования
    public float viewDistance = 10f; // Дальность зрения
    public float chaseTime = 10f; // Время преследования после потери игрока

    private bool isAttacking = false;
    private bool isChasing = false;
    private int currentPatrolIndex = 0;
    private float chaseTimer = 0f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        // Устанавливаем скорость в зависимости от сложности
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

        // Запускаем патрулирование
        PatrolToNextPoint();
    }

    private void Update()
    {
        if (isAttacking) return; // Если атакует, ничего не делаем

        if (CanSeePlayer())
        {
            // Если монстр видит игрока, начинает преследовать
            isChasing = true;
            chaseTimer = chaseTime; // Обновляем таймер погони
            agent.SetDestination(player.position);
        }
        else if (isChasing)
        {
            // Если преследует, но игрока не видно - убывает таймер
            chaseTimer -= Time.deltaTime;

            if (chaseTimer <= 0)
            {
                isChasing = false; // Прекращаем погоню
                PatrolToNextPoint();
            }
        }
        else if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            // Если не видит игрока и дошел до точки - переходит к следующей
            PatrolToNextPoint();
        }
    }

    private bool CanSeePlayer()
    {
        if (player == null) return false;

        float distance = Vector3.Distance(transform.position, player.position);
        return distance <= viewDistance;
    }

    private void PatrolToNextPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length; // Переход к следующей точке
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
        isChasing = false;
        agent.isStopped = true;
        walkAnimator.enabled = false;

        // Переключаем модель на убийцу
        killerModel.transform.SetPositionAndRotation(normalModel.transform.position, normalModel.transform.rotation);
        Renderer normalRenderer = normalModel.GetComponentInChildren<Renderer>();
        if (normalRenderer != null) normalRenderer.enabled = false;
        killerModel.SetActive(true);

        // Разворачиваемся к игроку
        Vector3 lookPos = player.position - transform.position;
        lookPos.y = 0;
        transform.rotation = Quaternion.LookRotation(lookPos);

        // Камера переходит к убийце
        Camera.main.transform.SetParent(killerModel.transform);
        Camera.main.transform.localPosition = new Vector3(34.4f, 21.3f, 392.2003f);
        Camera.main.transform.localRotation = Quaternion.Euler(-38.705f, -182.277f, 3.839f);

        // Выключаем игрока
        player.gameObject.SetActive(false);

        // Запуск анимации атаки
        killerAnimator.SetTrigger("Attack");

        Debug.Log("Игрок убит!");
        yield return new WaitForSeconds(4f);

        // Перенаправление на сцену "finish"
        playerDeath.Die();
    }
}
