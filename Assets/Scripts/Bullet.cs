using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rd;
    public float bulletSpeed;
    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        rd.AddForce(transform.forward * bulletSpeed);

        Destroy(this.gameObject, 5.0f);

    }


    void Update()
    {
        float angle = Mathf.Atan2(rd.velocity.y, rd.velocity.x) * Mathf.Rad2Deg;
    }
}
