using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;

public class PlayerAttack : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform 컴포넌트를 연결하세요.
    public Transform pivot;  // Bullet을 생성할 Pivot point의 Transform 컴포넌트를 연결하세요.
    public GameObject bulletPrefab; // 생성할 Bullet의 프리팹을 연결하세요.
    public float rotationSpeed = 5.0f; // 회전 속도 조절을 위한 변수
    public float bulletSpeed = 10.0f;  // Bullet의 발사 속도
    public float bulletLifetime = 3.0f; // Bullet의 생존 시간

    // PhotonView 컴포넌트 캐시처리를 위한 변수
    private PhotonView pv;
    // 시네머신 가상 카메라를 저장할 변수
    private CinemachineVirtualCamera virtualCamera;

    // 수신된 위치와 회전값을 저장할 변수
    private Vector3 receivePos;
    private Quaternion receiveRot;
    // 수신된 좌표로 이동 및 회전 속도의 민감도
    public float damping = 10.0f;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();

        //PhotonView가 자신의 것일 경우 시네머신 가상카메라를 연결
        if (pv.IsMine)
        {
            virtualCamera.Follow = transform;
            virtualCamera.LookAt = transform;
        }
        else
        {
            // 수신된 좌표로 보간한 이동처리
            transform.position = Vector3.Lerp(transform.position, receivePos, Time.deltaTime * damping);
            // 수신된 회전값으로 보간한 회전처리
            transform.rotation = Quaternion.Slerp(transform.rotation, receiveRot, Time.deltaTime * damping);
        }
    }

    void Update()
    {
        // 자신이 생성한 네트워크 객체만 컨트롤
        if (pv.IsMine)
        {
            Attack();
        }

        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 자신의 로컬 캐릭터인 경우 자신의 데이터를 다른 네트워크 유저에게 송신
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }

    void Attack()
    {
        // UpArrow 키를 누르면 왼쪽 방향으로 회전
        if (Input.GetKey(KeyCode.H))
        {
            RotateAroundPlayer(1f);
        }

        // DownArrow 키를 누르면 오른쪽 방향으로 회전
        if (Input.GetKey(KeyCode.K))
        {
            RotateAroundPlayer(-1f);
        }

        // Space 키를 누르면 Bullet을 생성하고 발사
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootBullet();
        }
    }

    void RotateAroundPlayer(float direction)
    {
        // 플레이어 주위를 회전
        transform.Rotate(Vector3.forward, direction * rotationSpeed * Time.deltaTime);
    }

    void ShootBullet()
    {
        // Bullet을 생성하고 발사
        GameObject bullet = Instantiate(bulletPrefab, pivot.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        // 플레이어 방향으로 회전된 각도를 적용
        float angle = player.eulerAngles.z;
        Vector2 shootDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

        // 포물선으로 발사
        bulletRb.velocity = bulletSpeed * shootDirection;

        // 일정 시간이 지난 후에 Bullet을 삭제
        Destroy(bullet, bulletLifetime);

    }

}
