using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{


    public float speed = 5f; // �������� ����������� ������

    public float jumpForce = 2f;

    public float attackWeakRate = 2.1f;
    public float attackStrongRate = 1.8f;
    public float nextAttackTime = 0f;
    private bool comboAttack = false;

    public float attackWeakRange = 1.1f;
    public float attackStrongRange = 1.5f;

    public int attackWeakDamage = 20;
    public int attackComboDamage = 35;
    public int attackStrongDamage = 50;

    public LayerMask enemyLayers;
    public Transform attackPoint; 

    private int currentHealth;
    public int maxHealth = 100;

    private bool isFalling = false;
    private bool isGrounded = true;



    private Animator animator;
    private Rigidbody2D rb;

    private void Start()
    {
        currentHealth = maxHealth;
    }
    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
    //�������� ������������ ���������//

        // �������� �������� ��� ����������� (A, D ��� ��������� ����� � ������)
        float horizontalInput = Input.GetAxis("Horizontal");

        // ��������� ������ �����������
        Vector3 movement = speed * Time.deltaTime * new Vector3(horizontalInput, 0f, 0f);

        Vector3 direction = transform.localScale;

        // ���������� ������
        transform.position += movement;
        
        //���������� �������� ��� ������������
        if (horizontalInput > 0)
        {
            // ������������ ������
            animator.SetBool("PlayerRun", true);
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            direction.x = 180;
        }
        else if (horizontalInput < 0)
        {
            // ������������ �����
            animator.SetBool("PlayerRun", true);
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            direction.x = -180;
        }
        else
        {
            // ����� �� ��������
            animator.SetBool("PlayerRun", false);
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            // ����� �������
            isFalling = false;
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            animator.SetTrigger("PlayerJump");
        }
        else if (rb.velocity.y < 0 && !isFalling)
        {
            isFalling = true;
            animator.SetBool("PlayerFall", true);
        }
        else if (rb.velocity.y >= 0 && isFalling)
        {
            isFalling = false;
            animator.SetBool("PlayerFall", false);
        }

        //�������� ������������ ���������//

        //�������� ������ ���������
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0) && !comboAttack)
            {
                animator.SetTrigger("PlayerWeakAttack");
                nextAttackTime = Time.time +1f/attackWeakRate;
                comboAttack = true;
              
            }else if (Input.GetMouseButtonDown(0) && comboAttack)
            {
                animator.SetTrigger("PlayerComboAttack");
                nextAttackTime = Time.time + 1f / attackWeakRate;
                comboAttack = false;

               
            }
           
        }
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(1))
            {
                animator.SetTrigger("PlayerStrongAttack");
     
                nextAttackTime = Time.time + 2f / attackStrongRate;
            }
        }
       
        //�������� ������ ���������

    }

    void ComboAttackOff()
    {
        comboAttack = false;
    }
    void WeakAttack()
    {
       

        Collider2D [] hitEnemys = Physics2D.OverlapCircleAll(attackPoint.position, attackWeakRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemys)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackWeakDamage);
            Debug.Log("������ ������ �� " + enemy.name);
        }
    }

    void CombatAttack()
    {

        Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(attackPoint.position, attackWeakRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemys)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackWeakDamage);
            Debug.Log("������ ������ �� " + enemy.name);
        }
    }
    void StrongAttack()
    {
       

        Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(attackPoint.position, attackStrongRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemys)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackStrongDamage);
            Debug.Log("������� ������ �� " + enemy.name);
        }
    }

    public void takeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            // �������� �����, ������� ������������ ������ ������
            die();
        }
        else
        {
            animator.SetTrigger("PlayerGetHit");
            animator.SetBool("PlayerDeath", false);
        }
        Debug.Log("player health = " + currentHealth);
    }

    void die()
    {
        animator.SetBool("PlayerDeath", true);
        //Restart();
        Debug.Log("Player died");
    

    }

   
    void Restart()
    {
        Invoke("Restart", 30f);
        SceneManager.LoadScene("PlaerScene", LoadSceneMode.Single);
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        // ���������, ��� ����������� � �������� �� ����������� ����
        if (collision.gameObject.CompareTag("Ground"))
        {
            // ���������, ��� ����������� � �������� � ����� "ground"
            isGrounded = true; // �������� �������� ��������� isGround
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackWeakRange);
        Gizmos.DrawWireSphere(attackPoint.position, attackStrongRange);
    }
}