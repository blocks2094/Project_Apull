using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform ������Ʈ�� �����ϼ���.
    public Transform pivot;  // Bullet�� ������ Pivot point�� Transform ������Ʈ�� �����ϼ���.
    public GameObject bulletPrefab; // ������ Bullet�� �������� �����ϼ���.
    public float rotationSpeed = 5.0f; // ȸ�� �ӵ� ������ ���� ����
    public float bulletSpeed = 10.0f;  // Bullet�� �߻� �ӵ�
    public float bulletLifetime = 3.0f; // Bullet�� ���� �ð�

   

   
    void Start()
    {
       
    }

    void Update()
    {
       
        // UpArrow Ű�� ������ ���� �������� ȸ��
        if (Input.GetKey(KeyCode.H))
        {
            RotateAroundPlayer(1f);
        }

        // DownArrow Ű�� ������ ������ �������� ȸ��
        if (Input.GetKey(KeyCode.K))
        {
            RotateAroundPlayer(-1f);
        }

        // Space Ű�� ������ Bullet�� �����ϰ� �߻�
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootBullet();
        }
    }

    void RotateAroundPlayer(float direction)
    {
        // �÷��̾� ������ ȸ��
        transform.Rotate(Vector3.forward, direction * rotationSpeed * Time.deltaTime);
    }

    void ShootBullet()
    {
        // Bullet�� �����ϰ� �߻�
        GameObject bullet = Instantiate(bulletPrefab, pivot.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        // �÷��̾� �������� ȸ���� ������ ����
        float angle = player.eulerAngles.z;
        Vector2 shootDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

        // ���������� �߻�
        bulletRb.velocity = bulletSpeed * shootDirection;

        // ���� �ð��� ���� �Ŀ� Bullet�� ����
        Destroy(bullet, bulletLifetime);

    }

}
