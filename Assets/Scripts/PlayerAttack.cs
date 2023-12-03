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
    public Transform player; // �÷��̾��� Transform ������Ʈ�� �����ϼ���.
    public Transform pivot;  // Bullet�� ������ Pivot point�� Transform ������Ʈ�� �����ϼ���.
    public GameObject bulletPrefab; // ������ Bullet�� �������� �����ϼ���.
    public float rotationSpeed = 5.0f; // ȸ�� �ӵ� ������ ���� ����
    public float bulletSpeed = 10.0f;  // Bullet�� �߻� �ӵ�
    public float bulletLifetime = 3.0f; // Bullet�� ���� �ð�

    // PhotonView ������Ʈ ĳ��ó���� ���� ����
    private PhotonView pv;
    // �ó׸ӽ� ���� ī�޶� ������ ����
    private CinemachineVirtualCamera virtualCamera;

    // ���ŵ� ��ġ�� ȸ������ ������ ����
    private Vector3 receivePos;
    private Quaternion receiveRot;
    // ���ŵ� ��ǥ�� �̵� �� ȸ�� �ӵ��� �ΰ���
    public float damping = 10.0f;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();

        //PhotonView�� �ڽ��� ���� ��� �ó׸ӽ� ����ī�޶� ����
        if (pv.IsMine)
        {
            virtualCamera.Follow = transform;
            virtualCamera.LookAt = transform;
        }
        else
        {
            // ���ŵ� ��ǥ�� ������ �̵�ó��
            transform.position = Vector3.Lerp(transform.position, receivePos, Time.deltaTime * damping);
            // ���ŵ� ȸ�������� ������ ȸ��ó��
            transform.rotation = Quaternion.Slerp(transform.rotation, receiveRot, Time.deltaTime * damping);
        }
    }

    void Update()
    {
        // �ڽ��� ������ ��Ʈ��ũ ��ü�� ��Ʈ��
        if (pv.IsMine)
        {
            Attack();
        }

        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // �ڽ��� ���� ĳ������ ��� �ڽ��� �����͸� �ٸ� ��Ʈ��ũ �������� �۽�
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
