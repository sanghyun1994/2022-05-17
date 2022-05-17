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
            // �̱��� ������ ���� ������Ʈ�� �Ҵ���� �ʾҴٸ�
            if(m_instance == null)
            {
                // �����κ��� GameManager ������Ʈ�� ã�Ƽ� �Ҵ��Ѵ�.
                m_instance = FindObjectOfType<GameManager>();
            }
            return m_instance;
        }
    }

    // �̱����� �Ҵ�� ���� ����
    private static GameManager m_instance;

    //���� ���� ����
    private int score = 0;
    // ���� ���� ����
    public bool isGameover { get; private set; }

    private void Awake()
    {
        // ���� �̱��� ������Ʈ�� �� �ٸ� ���ӸŴ��� ������Ʈ�� �����Ѵٸ�
        if(instance != this)
        {
            // �ڱ� �ڽ��� �ı��Ѵ�.
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // �÷��̾� ĳ������ ��� �̺�Ʈ �߻��� ���� ����
        FindObjectOfType<PlayerHealth>().onDeath += EndGame;
    }

    // ������ �߰��ϰ� UI�� ����
    public void AddScore(int newScore)
    {
        // ���� ���� ���°� �ƴ� ��쿡�� ���� �߰� ����
        if (!isGameover)
        {
            // ���� �߰�
            score += newScore;
            // ���� Ui�� ����
            UIManager.instance.UpdateScoreText(score);
        }
    }

    // ���� ���� ó��
    public void EndGame()
    {
        // ���� ���� ���¸� true�� ����
        isGameover = true;
        // ���� ���� UIȰ��ȭ
        UIManager.instance.SetActiveGameoverUI(true);

    }


}
