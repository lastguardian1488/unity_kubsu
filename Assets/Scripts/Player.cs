using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f; // �������� ����������� ������
    public float jumpForce = 2f;
    public float attackForce = 2f;
    public int health = 100;

    private bool isFalling = false;
    private bool isGrounded = true;
    private Animator animator;
    private Rigidbody2D rb;

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
        Vector3 movement = new Vector3(horizontalInput, 0f, 0f) * speed * Time.deltaTime;

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
        if (Input.GetMouseButton(1) && direction.x > 0)
        {
            //if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerRun"))
            //{
            //    animator.SetBool("PlayerAttack", true);

            //}
            //else
            //{
            //    animator.SetBool("PlayerRun", false);
            //    animator.SetBool("PlayerAttack", true);

            //}
            animator.SetBool("PlayerRun", false);
            animator.SetBool("PlayerAttackMega", true);
        }
        else if(Input.GetMouseButtonDown(1) && direction.x < 0)
        {
            animator.SetBool("PlayerRun", false);
            animator.SetBool("PlayerAttackMega", true);
            
        }
        else
        {
            animator.SetBool("PlayerAttackMega", false);
        }

        if (Input.GetMouseButton(0) && direction.x > 0)
        {
            //if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerRun"))
            //{
            //    animator.SetBool("PlayerAttack", true);

            //}
            //else
            //{
            //    animator.SetBool("PlayerRun", false);
            //    animator.SetBool("PlayerAttack", true);

            //}
            animator.SetBool("PlayerRun", false);
            animator.SetBool("PlayerAttack", true);
        }
        else if (Input.GetMouseButtonDown(0) && direction.x < 0)
        {
            animator.SetBool("PlayerRun", false);
            animator.SetBool("PlayerAttack", true);

        }
        else
        {
            animator.SetBool("PlayerAttack", false);
        }
    }

    public void takeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            // �������� �����, ������� ������������ ������ ������
            die();
        }
        Debug.Log("player health = " + health);
    }

    void die()
    {
        Debug.Log("Player died");
        // ��������� ������ ������
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
}