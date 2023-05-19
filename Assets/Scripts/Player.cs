using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f; // скорость перемещения игрока
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
        // получаем значение оси горизонтали (A, D или стрелочки влево и вправо)
        float horizontalInput = Input.GetAxis("Horizontal");

        // вычисляем вектор перемещения
        Vector3 movement = new Vector3(horizontalInput, 0f, 0f) * speed * Time.deltaTime;

        // перемещаем игрока
        transform.position += movement;
        
        //регулируем анимацию при передвижении
        if (horizontalInput > 0)
        {
            // передвижение вправо
            animator.SetBool("PlayerRun", true);
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (horizontalInput < 0)
        {
            // передвижение влево
            animator.SetBool("PlayerRun", true);
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            // игрок не движется
            animator.SetBool("PlayerRun", false);
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            // игрок прыгает
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
        // Проверяем, что столкнулись с объектом на блокирующем слое
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Проверяем, что столкнулись с объектом с тегом "ground"
            isGrounded = true; // Изменяем значение параметра isGround
        }
    }
}