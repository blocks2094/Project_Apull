using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermove : MonoBehaviour
{
    public float moveSpeed = 5.0f; // �÷��̾� �̵� �ӵ�
    

    void Update()
    {
        
        // A�� D Ű�� ���� ������ �̵�
        float h = Input.GetAxis("Horizontal");
       

        // �Է¿� ���� �̵� ���� ���
        Vector3 movement = new Vector3(0f, h, h);
        transform.Translate(movement * moveSpeed * Time.deltaTime);

        // ���� ī�޶� �÷��̾� ���ͷ� ���󰡰� ��
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
    }

   
}
