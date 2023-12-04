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

    // 발사 쿨타임 변수
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
        soundManager.PlaySound(0);  // Bullet 발사 사운드

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

    void RotateAroundPlayer(float direction)
    {
        transform.Rotate(Vector3.forward, direction * rotationSpeed * Time.deltaTime);
    }

    // 발사 쿨타임을 처리하는 코루틴
    IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
}
