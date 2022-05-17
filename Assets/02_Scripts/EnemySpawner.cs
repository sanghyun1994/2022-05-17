using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // ������ �� AI
    public EnemyCtrl enemyPrefab;
    // �� AI�� ��ȯ�� ��ġ
    public Transform[] spawnPoints;

    // �ִ� �ּ� ������
    public float maxDamage = 50.0f;
    public float minDamage = 10.0f;

    // �ִ� �ּ� ü��
    public float maxHp = 120.0f;
    public float minHp = 60.0f;

    // �ִ� �ּ� �ӷ�
    public float maxSpeed = 6.0f;
    public float minSpeed = 2.0f;

    // ���� ���ʹ̴� �������� ����Ѵ�.
    public Color strongEnemyColor = Color.red;

    // ������ ���� ���� ����Ʈ
    private List<EnemyCtrl> enemies = new List<EnemyCtrl>();

    // ���� ���̺�
    private int wave;

    private void Update()
    {
        // ���� ���� ������ ��� �������� �ʴ´�.
        if (GameManager.instance != null && GameManager.instance.isGameover)
        {
            return;
        }
        // ��� ���� �Ҹ����� ��� ���� ���� ����
        if (enemies.Count <= 0)
        {
            SpawnWave();
        }

        // UI����
        UpdateUI();
        
    }

    // ���̺� ������ UI�� ���� ǥ��
    private void UpdateUI()
    {
        // ���� ���̺�� ���� ���� ǥ���Ѵ�.
        UIManager.instance.UpdateWaveText(wave, enemies.Count);
    }

    // ���� ���̺꿡 ���� ���� ����
    private void SpawnWave()
    {
        // ���̺� 1 ����
        wave++;
        // ���� ���̺� * 1.2��ŭ�� �ݿø��� �� ��ŭ ���� ����
        int spawnCount = Mathf.RoundToInt(wave * 1.2f);

        // spawnCount��ŭ ���� ����
        for (int i = 0; i < spawnCount; i++)
        {
            // ���� ���⸦ 0���� 100���̷� �����ϰ� ����
            float enemyIntensity = Random.Range(0f, 1f);
            // �� ����
            CreateEnemy(enemyIntensity);

        }
            
    }

    private void CreateEnemy(float intensity)
    {
        // intensity�� ������� ���� �ɷ�ġ ����
        float health = Mathf.Lerp(minHp, maxHp, intensity);
        float damage = Mathf.Lerp(minDamage, maxDamage, intensity);
        float speed = Mathf.Lerp(minSpeed, maxSpeed, intensity);

        // intensity�� ������� ����� enemyStrengh���̿��� enemy�� �Ǻλ��� ����
        Color skinColor = Color.Lerp(Color.white, strongEnemyColor, intensity);

        // ������ ��ġ�� �������� ����
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // �� ���������κ��� �� ����
        EnemyCtrl enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        // ������ ���� �ɷ�ġ �� ���� ��� ����
        enemy.Setup(health, damage, speed, skinColor);

        // ������ ���� ����Ʈ�� �߰�
        enemies.Add(enemy);

        // ���� onDeath�̺�Ʈ�� �͸��� �޼��� ��� (���ٽ� Ȱ��)
        // ����� ���� ����Ʈ�κ��� ����
        enemy.onDeath += () => enemies.Remove(enemy);
        // ����� ���� 5�ʵڿ� �ı�
        enemy.onDeath += () => Destroy(enemy.gameObject, 5.0f);
        // ���� ����� ���� ���
        enemy.onDeath += () => GameManager.instance.AddScore(100);


    }
















}
