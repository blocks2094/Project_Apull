using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform 컴포넌트를 연결하세요.
    public Transform pivot;  // Bullet을 생성할 Pivot point의 Transform 컴포넌트를 연결하세요.
    public GameObject bulletPrefab; // 생성할 Bullet의 프리팹을 연결하세요.
    public float rotationSpeed = 5.0f; // 회전 속도 조절을 위한 변수
    public float bulletSpeed = 10.0f;  // Bullet의 발사 속도
    public float bulletLifetime = 3.0f; // Bullet의 생존 시간

   

   
    void Start()
    {
       
    }

    void Update()
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
