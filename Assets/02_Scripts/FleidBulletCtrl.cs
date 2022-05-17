using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleidBulletCtrl : MonoBehaviour
{

    public float speed = 8.0f;
    private Rigidbody ri;
    
    void Start()
    {
        ri = GetComponent<Rigidbody>();
        ri.velocity = transform.forward * speed;

        Destroy(gameObject, 10.0f);
    }

    void OnTriggerEnter(Collider coil)
    {
        // ���� �ε��� ��� �Ҹ� 
        if (coil.CompareTag("WALL"))
        {
            Destroy(gameObject);    
        }
       
    }

    



}
