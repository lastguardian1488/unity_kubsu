using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public int damageAmount = 10;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        // ���������, ��� ����������� � �������� �� ����������� ����
        if (collision.gameObject.CompareTag("Player"))
        {
            Player playerController = collision.gameObject.GetComponent<Player>();
            if (playerController != null)
            {
                // �������� ����� takeDamage() �������, ����� ������� ����
                playerController.takeDamage(damageAmount);
            }
        }
    }
}
