using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermove : MonoBehaviour
{
    public float moveSpeed = 5.0f; // 플레이어 이동 속도
    public float jumpPower = 2.0f;

    private Rigidbody2D rb;
    private bool isJumping = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        // A와 D 키로 왼쪽 오른쪽 이동
        float h = Input.GetAxis("Horizontal");
       

        // 입력에 따라 이동 벡터 계산
        Vector3 movement = new Vector3(0f, h, h);
        transform.Translate(movement * moveSpeed * Time.deltaTime);

        // 메인 카메라를 플레이어 센터로 따라가게 함
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
