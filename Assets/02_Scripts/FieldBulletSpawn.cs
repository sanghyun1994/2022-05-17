using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldBulletSpawn : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float spawnRateMin = 2.0f;
    public float spawnRateMax = 5.0f;

    // 발사 대상
    private Transform target;
    // 생성 주기 (max~min)
    private float spawnRate;
    // 최근 생성으로부터 지난 시간
    private float timeAfterSpawn;


    void Start()
    {
        // 초기화
        timeAfterSpawn = 0.0f;
        // 생성 주기를 max~min 랜덤 지정
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
