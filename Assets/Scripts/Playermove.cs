using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;

public class Playermove : MonoBehaviour
{
    public float moveSpeed = 5.0f; // 플레이어 이동 속도
    public float jumpPower = 2.0f;

    private Rigidbody2D rb;
    private bool isJumping = false;

    // PhotonView 컴포넌트 캐시처리를 위한 변수
    private PhotonView pv;
    // 시네머신 가상 카메라를 저장할 변수
    private CinemachineVirtualCamera virtualCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        pv = GetComponent<PhotonView>();
        virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();

        //PhotonView가 자신의 것일 경우 시네머신 가상카메라를 연결
        if (pv.IsMine)
        {
            virtualCamera.Follow = transform;
            virtualCamera.LookAt = transform;
        }
    }

    void Update()
    {
        // 자신이 생성한 네트워크 객체만 컨트롤
        if (pv.IsMine)
        {
            Move();
            //Turn();
        }
    }

    void Move()
    {
        // A와 D 키로 왼쪽 오른쪽 이동
        float h = Input.GetAxis("Horizontal");


        // 입력에 따라 이동 벡터 계산
        Vector3 movement = new Vector3(0f, h, h);
        transform.Translate(movement * moveSpeed * Time.deltaTime);

        // 메인 카메라를 플레이어 센터로 따라가게 함
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);

        if (Input.GetKeyDown(KeyCode.J) && !isJumping)
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
