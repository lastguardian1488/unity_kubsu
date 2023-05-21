using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public GameObject firePrefab;
    public GameObject visionArea;
    public GameObject dieAnimation;

    public float moveSpeed = 3f;
    public float attackRange = 4f;
    //public float attackDelay = 1f; // �������� ����� �������

    public int maxHeath = 100;
    private int health;


    private GameObject fireInstance;
    private Animator animator;
    private EnemyFSM brain;
    private AttackCooldown cooldown;

    private bool isMovingRight = true;
    private float timer = 0f;
    private float distance = 2f;
    private float pauseTime = 1f;

    public void TakeDamage(int damageAmount)
    {
        animator.SetTrigger("DemonHit");
        health -= damageAmount;
        //����� ��� �������� ��������� ������
        if (health <= 0)
        {
            Die();
        }
    }

    private void Start()
    {
        health = maxHeath;
        animator = GetComponent<Animator>();
        brain = GetComponent<EnemyFSM>();
        cooldown = GetComponent<AttackCooldown>();

        visionArea = Instantiate(visionArea, transform.position, Quaternion.identity);
        visionArea.transform.parent = transform;

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        brain.SetState(Wander);
    }

    private void Update()
    {
        // ���������� ���������� �� ������
        //Vector2 direction = player.position - transform.position;
        /*float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, angle, 0f);*/
    }


    private void ChasePlayer()
    {
        if (Vector2.Distance(transform.position, player.position) >= attackRange)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            }
        }
        else
        {
            brain.SetState(Attack);
        }
        if (!IsPlayerInSight())
        {
            brain.SetState(Wander);
        }
    }

    private void Wander()
    {
        if (IsPlayerInSight())
        {
            brain.SetState(ChasePlayer);
        }
        else
        {
            
            if (isMovingRight)
            {
                transform.Translate(0.5f * moveSpeed * Time.deltaTime * Vector2.right);
            }
            else
            {
                transform.Translate(0.5f * moveSpeed * Time.deltaTime * Vector2.left);
            }

            timer += Time.deltaTime;
            if (timer >= 2f)
            {
                if (isMovingRight)
                    isMovingRight = false;
                else
                    isMovingRight = true;
                timer = 0f;
            }
        }
    }

    private void Attack()
    {
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            if (cooldown.isZeroed()) // ���������, ������ �� �������� ����� �������
            {
                animator.SetTrigger("DemonAttack"); // ����������� �������� ����� 
                cooldown.StartCooldown(); // ������������� ������� �������� ����� �������
                Invoke(nameof(SpawnFire), 0.9f); //����������� ����� ����, ����� ������� � ��������
            }
        }
        else
        {
            brain.SetState(ChasePlayer);
        }
        if (!IsPlayerInSight())
        {
            brain.SetState(Wander);
        }
    }

    private void SpawnFire()
    {
        float offsetX = -3.4f; //�������� x ���� ������������ ����������
        float offsetY = -2.5f; //�������� y ���� ������������ ����������
        Vector3 firePosition = transform.position + new Vector3(offsetX, offsetY, 0f); 
        fireInstance = Instantiate(firePrefab, firePosition, Quaternion.identity);  //������� ������ ���� �� ������� ����
        fireInstance.transform.parent = transform; //��������� ����� �������� �� ������
    }

    private bool IsPlayerInSight()
    {
        return visionArea.GetComponent<VisionArea>().playerInSight;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        ParticleSystem dieEffect = Instantiate(dieAnimation.GetComponent<ParticleSystem>(), transform.position, Quaternion.identity);
        dieEffect.Play();
    }
}
