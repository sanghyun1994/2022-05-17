using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // 싱글턴 접근용 프로퍼티
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

    // 게임 종료 여부를 저장할 프로퍼티
    public bool IsGameOver
    {
        get { return isGameOver; }
        set
        {
            isGameOver = value;
        }
    }

    // 탄 표시용 텍스트
    public Text ammoText;
    // 점수 표시용 텍스트
    public Text scoreText;
    // 에너미 웨이브 텍스트
    public Text waveText;
    // 생존 시간 텍스트
    public Text timeText;

    // 게임오버시 활성화 될 UI
    public GameObject gameoverUI;
    // 생존 시간
    private float surviveTime;
    
    void Start()
    {

        // 생존시간 및 게임 오버 상태 초기화
        surviveTime = 0;
        isGameOver = false;
    }

    void Update()
    {
        if (!isGameOver)
        {
            // 생존 시간 갱신
            surviveTime += Time.deltaTime;
            // 갱신 시간을 timetext를 이용해 표시한다.
            timeText.text = "Time: " + (int)surviveTime;
        }
    }




        // 탄 텍스트 갱신
        // num : 현재 탄창내 탄환, max : 남은 전체 탄알
        public void UpdateAmmoText(int numBullet, int maxBullet)
    {
        ammoText.text = numBullet + "/" + maxBullet;
    }

    // 스코어 갱신
    public void UpdateScoreText(int newScore)
    {
        scoreText.text = "Score : " + newScore;
    }

    // 웨이브 갱신
    public void UpdateWaveText(int waves, int count)
    {
        waveText.text = "Wave : " + waves + "\nEnemy Left : " + count;
    }

    // 게임 오버UI 활성화
    public void SetActiveGameoverUI(bool active)
    {
        gameoverUI.SetActive(active);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
