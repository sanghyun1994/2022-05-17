using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // 생성할 적 AI
    public EnemyCtrl enemyPrefab;
    // 적 AI를 소환할 위치
    public Transform[] spawnPoints;

    // 최대 최소 데미지
    public float maxDamage = 50.0f;
    public float minDamage = 10.0f;

    // 최대 최소 체력
    public float maxHp = 120.0f;
    public float minHp = 60.0f;

    // 최대 최소 속력
    public float maxSpeed = 6.0f;
    public float minSpeed = 2.0f;

    // 강한 에너미는 빨간색을 띄게한다.
    public Color strongEnemyColor = Color.red;

    // 생성될 적을 담을 리스트
    private List<EnemyCtrl> enemies = new List<EnemyCtrl>();

    // 현재 웨이브
    private int wave;

    private void Update()
    {
        // 게임 오버 상태일 경우 생성하지 않는다.
        if (GameManager.instance != null && GameManager.instance.isGameover)
        {
            return;
        }
        // 모든 적이 소멸했을 경우 다음 스폰 실행
        if (enemies.Count <= 0)
        {
            SpawnWave();
        }

        // UI갱신
        UpdateUI();
        
    }

    // 웨이브 정보를 UI로 따로 표시
    private void UpdateUI()
    {
        // 현재 웨이브와 남은 적을 표시한다.
        UIManager.instance.UpdateWaveText(wave, enemies.Count);
    }

    // 현재 웨이브에 맞춰 적을 생성
    private void SpawnWave()
    {
        // 웨이브 1 증가
        wave++;
        // 현재 웨이브 * 1.2만큼을 반올림한 수 만큼 적을 생성
        int spawnCount = Mathf.RoundToInt(wave * 1.2f);

        // spawnCount만큼 적을 생성
        for (int i = 0; i < spawnCount; i++)
        {
            // 적의 세기를 0에서 100사이로 랜덤하게 결정
            float enemyIntensity = Random.Range(0f, 1f);
            // 적 생성
            CreateEnemy(enemyIntensity);

        }
            
    }

    private void CreateEnemy(float intensity)
    {
        // intensity를 기반으로 적의 능력치 결정
        float health = Mathf.Lerp(minHp, maxHp, intensity);
        float damage = Mathf.Lerp(minDamage, maxDamage, intensity);
        float speed = Mathf.Lerp(minSpeed, maxSpeed, intensity);

        // intensity를 기반으로 흰색과 enemyStrengh사이에서 enemy의 피부색을 결정
        Color skinColor = Color.Lerp(Color.white, strongEnemyColor, intensity);

        // 생성할 위치를 랜덤으로 결정
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // 적 프리팹으로부터 적 생성
        EnemyCtrl enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        // 생성한 적의 능력치 및 추적 대상 설정
        enemy.Setup(health, damage, speed, skinColor);

        // 생성될 적을 리스트에 추가
        enemies.Add(enemy);

        // 적의 onDeath이벤트의 익명의 메서드 등록 (람다식 활용)
        // 사망한 적을 리스트로부터 제거
        enemy.onDeath += () => enemies.Remove(enemy);
        // 사망한 적을 5초뒤에 파괴
        enemy.onDeath += () => Destroy(enemy.gameObject, 5.0f);
        // 적이 사망시 점수 상승
        enemy.onDeath += () => GameManager.instance.AddScore(100);


    }
















}
