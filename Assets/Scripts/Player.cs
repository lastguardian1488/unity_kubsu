using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f; // �������� ����������� ������
    public float jumpForce = 2f;

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
        // �������� �������� ��� ����������� (A, D ��� ��������� ����� � ������)
        float horizontalInput = Input.GetAxis("Horizontal");

        // ��������� ������ �����������
        Vector3 movement = new Vector3(horizontalInput, 0f, 0f) * speed * Time.deltaTime;

        // ���������� ������
        transform.position += movement;
        
        //���������� �������� ��� ������������
        if (horizontalInput > 0)
        {
            // ������������ ������
            animator.SetBool("PlayerRun", true);
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (horizontalInput < 0)
        {
            // ������������ �����
            animator.SetBool("PlayerRun", true);
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
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