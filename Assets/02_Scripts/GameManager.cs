using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance
    {
        get
        {
            // 싱글턴 변수에 아직 오브젝트가 할당되지 않았다면
            if(m_instance == null)
            {
                // 씬으로부터 GameManager 오브젝트를 찾아서 할당한다.
                m_instance = FindObjectOfType<GameManager>();
            }
            return m_instance;
        }
    }

    // 싱글턴이 할당될 정적 변수
    private static GameManager m_instance;

    //현재 게임 점수
    private int score = 0;
    // 게임 오버 상태
    public bool isGameover { get; private set; }

    private void Awake()
    {
        // 씬에 싱글턴 오브젝트가 된 다른 게임매니저 오브젝트가 존재한다면
        if(instance != this)
        {
            // 자기 자신을 파괴한다.
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 플레이어 캐릭터의 사망 이벤트 발생시 게임 오버
        FindObjectOfType<PlayerHealth>().onDeath += EndGame;
    }

    // 점수를 추가하고 UI를 갱신
    public void AddScore(int newScore)
    {
        // 게임 오버 상태가 아닐 경우에만 점수 추가 가능
        if (!isGameover)
        {
            // 점수 추가
            score += newScore;
            // 점수 Ui의 갱신
            UIManager.instance.UpdateScoreText(score);
        }
    }

    // 게임 오버 처리
    public void EndGame()
    {
        // 게임 오버 상태를 true로 변경
        isGameover = true;
        // 게임 오버 UI활성화
        UIManager.instance.SetActiveGameoverUI(true);

    }


}
