using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;
    public float attackRange = 4f;
    public float stopDistance = 3f;
    public float attackDelay = 1f; // Задержка между атаками
    public GameObject firePrefab;
    public float fireX = -2.5f;
    public float fireY = -2f;

    private GameObject fireInstance;
    private Animator animator;
    private bool canMove = true;
    private float currentAttackDelay = 0f;

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
        // Направляем противника на игрока
        Vector2 direction = player.position - transform.position;
        /*float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, angle, 0f);*/

        // Проверяем, находится ли игрок в зоне атаки
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            if (currentAttackDelay <= 0f) // Проверяем, прошла ли задержка между атаками
            {
                // Проигрываем анимацию атаки
                animator.SetTrigger("DemonAttack");
                currentAttackDelay = attackDelay; // Устанавливаем текущую задержку между атаками

                if (fireInstance == null) // Проверяем, не проигрывается ли анимация огня уже
                {
                    Vector3 firePosition = transform.position + new Vector3(fireX, fireY, 0f);
                    fireInstance = Instantiate(firePrefab, firePosition, Quaternion.identity);
                    fireInstance.transform.parent = transform; // Делаем огонь дочерним объектом демона
                    //fireInstance.Play(player); // Запускаем проигрывание анимации огня
                    Destroy(fireInstance, 1f);
                }
            }
            else
            {
                currentAttackDelay -= Time.deltaTime; // Уменьшаем текущую задержку между атаками на каждом кадре
            }
            canMove = false;
        }
        else
        {
            canMove = true; // Устанавливаем флаг canMove в true, чтобы противник мог двигаться
        }

        // Перемещаем противника к игроку
        if (canMove && Vector2.Distance(transform.position, player.position) > stopDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }
}
