using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public GameObject firePrefab;
    public float moveSpeed = 3f;
    public float attackRange = 4f;
    public float stopDistance = 3f; //дистанци€ остановки перед игроком
    public float attackDelay = 1f; // «адержка между атаками
    public int health = 100;
    

    private GameObject fireInstance;
    private Animator animator;
    private bool canMove = true;
    private float currentAttackDelay = 0f;

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        //место дл€ анимации получени€ дамага
        if (health <= 0)
        {
            animator.SetTrigger("DemonHit");
            Die();
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void Update()
    {
        // Ќаправл€ем противника на игрока
        Vector2 direction = player.position - transform.position;
        /*float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, angle, 0f);*/

        // ѕровер€ем, находитс€ ли игрок в зоне атаки
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            if (currentAttackDelay <= 0f) // ѕровер€ем, прошла ли задержка между атаками
            {
                animator.SetTrigger("DemonAttack"); // ѕроигрываем анимацию атаки 
                currentAttackDelay = attackDelay; // ”станавливаем текущую задержку между атаками
                Invoke("SpawnFire", 0.9f); //откладываем спавн огн€, чтобы попасть в анимацию
            }
            else
            {
                currentAttackDelay -= Time.deltaTime; // ”меньшаем текущую задержку между атаками на каждом кадре
            }
            canMove = false;
        }
        else
        {
            canMove = true; // ”станавливаем флаг canMove в true, чтобы противник мог двигатьс€
        }

        // ѕеремещаем противника к игроку
        if (canMove && Vector2.Distance(transform.position, player.position) > stopDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    private void SpawnFire()
    {
        float offsetX = -3.4f; //смещение x огн€ относительно противника
        float offsetY = -2.5f; //смещение y огн€ относительно противника
        Vector3 firePosition = transform.position + new Vector3(offsetX, offsetY, 0f); 
        fireInstance = Instantiate(firePrefab, firePosition, Quaternion.identity);  //создаем объект огн€ из префаба огн€
        fireInstance.transform.parent = transform; //назначаем огонь дочерним от демона
        Destroy(fireInstance, 0.5f); //разрушаем спуст€ секунду после создани€
    }

    private void Die()
    {
        //место дл€ анимации смерти
        Destroy(this);
    }
}
