using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermove : MonoBehaviour
{
    public float moveSpeed = 5.0f; // �÷��̾� �̵� �ӵ�
    public float jumpPower = 2.0f;

    private Rigidbody2D rb;
    private bool isJumping = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        // A�� D Ű�� ���� ������ �̵�
        float h = Input.GetAxis("Horizontal");
       

        // �Է¿� ���� �̵� ���� ���
        Vector3 movement = new Vector3(0f, h, h);
        transform.Translate(movement * moveSpeed * Time.deltaTime);

        // ���� ī�޶� �÷��̾� ���ͷ� ���󰡰� ��
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);

        if(Input.GetKeyDown(KeyCode.J) && !isJumping)
        {
            Jump();
        }

    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        isJumping = true;
        StartCoroutine(ResetJumpFlag());
    }

    IEnumerator ResetJumpFlag()
    {
        yield return new WaitForSeconds(1.5f);
        isJumping = false;
    }
}
