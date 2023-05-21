using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public int damageAmount = 10;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("EndState")) //проверяем находится ли аниматор огня в конечном состоянии
            Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверяем, что столкнулись с объектом на блокирующем слое
        if (collision.gameObject.CompareTag("Player"))
        {
            Player playerController = collision.gameObject.GetComponent<Player>();
            if (playerController != null)
            {
                // Вызываем метод takeDamage() уигрока, чтобы нанести урон
                playerController.takeDamage(damageAmount);
            }
        }
    }
}
