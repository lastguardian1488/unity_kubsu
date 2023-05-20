using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public GameObject firePrefab;
    public float moveSpeed = 3f;
    public float attackRange = 4f;
    public float stopDistance = 3f; //��������� ��������� ����� �������
    public float attackDelay = 1f; // �������� ����� �������
    public int health = 100;
    

    private GameObject fireInstance;
    private Animator animator;
    private bool canMove = true;
    private float currentAttackDelay = 0f;

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        //����� ��� �������� ��������� ������
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
        // ���������� ���������� �� ������
        Vector2 direction = player.position - transform.position;
        /*float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, angle, 0f);*/

        // ���������, ��������� �� ����� � ���� �����
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            if (currentAttackDelay <= 0f) // ���������, ������ �� �������� ����� �������
            {
                animator.SetTrigger("DemonAttack"); // ����������� �������� ����� 
                currentAttackDelay = attackDelay; // ������������� ������� �������� ����� �������
                Invoke("SpawnFire", 0.9f); //����������� ����� ����, ����� ������� � ��������
            }
            else
            {
                currentAttackDelay -= Time.deltaTime; // ��������� ������� �������� ����� ������� �� ������ �����
            }
            canMove = false;
        }
        else
        {
            canMove = true; // ������������� ���� canMove � true, ����� ��������� ��� ���������
        }

        // ���������� ���������� � ������
        if (canMove && Vector2.Distance(transform.position, player.position) > stopDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    private void SpawnFire()
    {
        float offsetX = -3.4f; //�������� x ���� ������������ ����������
        float offsetY = -2.5f; //�������� y ���� ������������ ����������
        Vector3 firePosition = transform.position + new Vector3(offsetX, offsetY, 0f); 
        fireInstance = Instantiate(firePrefab, firePosition, Quaternion.identity);  //������� ������ ���� �� ������� ����
        fireInstance.transform.parent = transform; //��������� ����� �������� �� ������
        Destroy(fireInstance, 0.5f); //��������� ������ ������� ����� ��������
    }

    private void Die()
    {
        //����� ��� �������� ������
        Destroy(this);
    }
}
