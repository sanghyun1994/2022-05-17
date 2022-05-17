using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // �̱��� ���ٿ� ������Ƽ
    public static UIManager instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }

            return m_instance;
        }
    }

    private static UIManager m_instance;
    private bool isGameOver;

    // ���� ���� ���θ� ������ ������Ƽ
    public bool IsGameOver
    {
        get { return isGameOver; }
        set
        {
            isGameOver = value;
        }
    }

    // ź ǥ�ÿ� �ؽ�Ʈ
    public Text ammoText;
    // ���� ǥ�ÿ� �ؽ�Ʈ
    public Text scoreText;
    // ���ʹ� ���̺� �ؽ�Ʈ
    public Text waveText;
    // ���� �ð� �ؽ�Ʈ
    public Text timeText;

    // ���ӿ����� Ȱ��ȭ �� UI
    public GameObject gameoverUI;
    // ���� �ð�
    private float surviveTime;
    
    void Start()
    {

        // �����ð� �� ���� ���� ���� �ʱ�ȭ
        surviveTime = 0;
        isGameOver = false;
    }

    void Update()
    {
        if (!isGameOver)
        {
            // ���� �ð� ����
            surviveTime += Time.deltaTime;
            // ���� �ð��� timetext�� �̿��� ǥ���Ѵ�.
            timeText.text = "Time: " + (int)surviveTime;
        }
    }




        // ź �ؽ�Ʈ ����
        // num : ���� źâ�� źȯ, max : ���� ��ü ź��
        public void UpdateAmmoText(int numBullet, int maxBullet)
    {
        ammoText.text = numBullet + "/" + maxBullet;
    }

    // ���ھ� ����
    public void UpdateScoreText(int newScore)
    {
        scoreText.text = "Score : " + newScore;
    }

    // ���̺� ����
    public void UpdateWaveText(int waves, int count)
    {
        waveText.text = "Wave : " + waves + "\nEnemy Left : " + count;
    }

    // ���� ����UI Ȱ��ȭ
    public void SetActiveGameoverUI(bool active)
    {
        gameoverUI.SetActive(active);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
