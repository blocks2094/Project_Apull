using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booom : MonoBehaviour
{
    public GameObject explosionAreaGO;

    public SoundManager soundManager;
    void Start()
    {
        explosionAreaGO.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.transform.CompareTag("Ground") ||
            collision.transform.CompareTag("Player")) return;
        soundManager.PlaySound(0);
        explosionAreaGO.SetActive(true);
        Destroy(this.gameObject, 0.2f);
    }
}
