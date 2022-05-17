using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldBulletSpawn : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float spawnRateMin = 2.0f;
    public float spawnRateMax = 5.0f;

    // �߻� ���
    private Transform target;
    // ���� �ֱ� (max~min)
    private float spawnRate;
    // �ֱ� �������κ��� ���� �ð�
    private float timeAfterSpawn;


    void Start()
    {
        // �ʱ�ȭ
        timeAfterSpawn = 0.0f;
        // ���� �ֱ⸦ max~min ���� ����
        spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        
        target = FindObjectOfType<PlayerCtrl>().transform;
        
    }

   
    void Update()
    {
        timeAfterSpawn += Time.deltaTime;

        if (timeAfterSpawn >= spawnRate)
        {
            timeAfterSpawn = 0.0f;
            GameObject bullet = Instantiate(bulletPrefab, transform.position + (Vector3.up * 1.5f)  , transform.rotation);
            bullet.transform.LookAt(target);

            spawnRate = Random.Range(spawnRateMin, spawnRateMax);

        }
        
    }
}
