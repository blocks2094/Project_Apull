using Cinemachine;
using Photon.Pun;
using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviourPunCallbacks, IPunObservable
{
    public Transform player;
    public Transform pivot;
    public GameObject bulletPrefab;
    public float rotationSpeed = 5.0f;
    public float bulletSpeed = 10.0f;
    public float bulletLifetime = 3.0f;

    private PhotonView pv;
    private CinemachineVirtualCamera virtualCamera;

    private Vector3 receivePos;
    private Quaternion receiveRot;
    public float damping = 10.0f;

    // �߻� ��Ÿ�� ����
    private bool canShoot = true;
    public float shootCooldown = 5.0f;

    public SoundManager soundManager;
    void Start()
    {
        pv = GetComponent<PhotonView>();
        virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();

        if (pv.IsMine)
        {
            virtualCamera.Follow = transform;
            virtualCamera.LookAt = transform;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, receivePos, Time.deltaTime * damping);
            transform.rotation = Quaternion.Slerp(transform.rotation, receiveRot, Time.deltaTime * damping);
        }
    }

    void Update()
    {
        if (pv.IsMine)
        {
            Attack();
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
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
        if (Input.GetKey(KeyCode.H))
        {
            RotateAroundPlayer(1f);
        }

        if (Input.GetKey(KeyCode.K))
        {
            RotateAroundPlayer(-1f);
        }

        if (Input.GetKeyDown(KeyCode.Space) && canShoot)
        {
            pv.RPC("ShootBullet", RpcTarget.AllBuffered);
            StartCoroutine(ShootCooldown());
        }
    }

    [PunRPC]
    void ShootBullet()
    {
        soundManager.PlaySound(0);  // Bullet �߻� ����

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

    void RotateAroundPlayer(float direction)
    {
        transform.Rotate(Vector3.forward, direction * rotationSpeed * Time.deltaTime);
    }

    // �߻� ��Ÿ���� ó���ϴ� �ڷ�ƾ
    IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
}
