//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;

//public class GameManagerBackup : MonoBehaviour
//{

//    // ���� ������ �ؽ�Ʈ ������Ʈ
//    public GameObject gameoverText;
//    // ���� �ð� ����
//    public Text timeText;
//    // �ְ� ��� ����
//    public Text recordText;

//    // ���� �ð�
//    private float surviveTime;

//    // ���� ���� ���θ� ������ ��� ����
//    private bool isGameOver;

//    //// ������ ���� ��ġ�� �����ϴ� ����Ʈ Ÿ�� ����
//    //public List<Transform> points = new List<Transform>();

//    ////���͸� �̸� ������ ������ ����Ʈ �ڷ���
//    //public List<GameObject> monsterPool = new List<GameObject>();

//    //// ������Ʈ Ǯ���� ������ ������ �ִ� ����
//    //public int maxMonsters = 10; 

//    //public GameObject monster;

//    //// ������ ���� ����
//    //public float createTime = 3.0f;

//    // ���� ���� ���θ� ������ ������Ƽ
//    public bool IsGameOver
//    {
//        get { return isGameOver; }
//        set
//        {
//            isGameOver = value;
//            //if (isGameOver)
//            //{
//                //CancelInvoke("CreateMonster");
//            //}
//        }
//    }

//    // �̱��� �ν��Ͻ� ����
//    public static GameManagerBackup instance = null;

//    // ��ũ��Ʈ����� ���� ���� ȣ��� �̺�Ʈ �Լ�
//    void Awake()
//    {
//        // instance�� �Ҵ���� ���� ���
//        if (instance == null)
//        {
//            instance = this;
//        }
//        // instance�� �Ҵ�� Ŭ������ �ν��Ͻ��� �ٸ� ��� ���ο� class�� �ǹ���
//        else if (instance != this)
//        {
//            Destroy(this.gameObject);
//        }
//        // �ٸ� ������ �Ѿ���� �������� �ʰ� ������
//        DontDestroyOnLoad(this.gameObject);

//    }

//    void Start()
//    {

//        // �����ð� �� ���� ���� ���� �ʱ�ȭ
//        surviveTime = 0;
//        isGameOver = false;

//        //// ���� ������Ʈ Ǯ ����
//        //CreateMonsterPool();

//        //Transform spawnPointGroup = GameObject.Find("SpawnPointGroup")?.transform;

//        ////SPG�� ��� ���ϵ� ������Ʈ�κ��� TR����
//        ////spawnPointGroup?.GetComponentsInChildren<Transform>(points);
        
//        //foreach(Transform point in spawnPointGroup)
//        //{
//        //    points.Add(point);
//        //}

//        //// ���� �������� �Լ� ȣ��
//        //InvokeRepeating("CreateMonster", 2.0f, createTime);
//    }
//    void Update()
//    {
//        if (!isGameOver)
//        {
//            // ���� �ð� ����
//            surviveTime += Time.deltaTime;
//            // ���� �ð��� timetext�� �̿��� ǥ���Ѵ�.
//            timeText.text = "Time: " + (int) surviveTime;
//        }
//        else
//        {
//            // ���� ���� ���¿��� RŰ�� ���� ���
//            if (Input.GetKeyDown(KeyCode.R))
//            {
//                // play back up ���� �ٽ� �ε��Ѵ�
//                SceneManager.LoadScene("Play _Backup");
//            }
//        }
//    }

//    //void CreateMonster()
//    //{
//    //    // ������ ������ġ�� �ұ�Ģ�ϰ� ����
//    //    int idx = Random.Range(0, points.Count);
//    //    // ���� ������ ����
//    //    //Instantiate(monster, points[idx].position, points[idx].rotation);
        
//    //    // ������Ʈ Ǯ�κ��� ���� ����
//    //    GameObject _monster = GetMonsterInPool();
//    //    // ���� ������ ��ġ ȸ�� ����
//    //    _monster?.transform.SetPositionAndRotation(points[idx].position, points[idx].rotation);

//    //    // ���� ���� Ȱ��ȭ 
//    //    _monster?.SetActive(true);

//    //}
    
//    //void CreateMonsterPool()
//    //{
//    //    for( int i=0; i < maxMonsters; i++)
//    //    {
//    //        // ���� ����
//    //        var _monster = Instantiate<GameObject>(monster);
//    //        // ���� �̸� ����
//    //        _monster.name = $"Monster_{i:00}";
//    //        // ���� ��Ȱ��ȭ
//    //        _monster.SetActive(false);

//    //        // ������ ���͸� ������Ʈ Ǯ�� �߰��Ѵ�.
//    //        monsterPool.Add(_monster);

//    //    }
//    //}
//    //public GameObject GetMonsterInPool()
//    //{
//    //    // ������Ʈ Ǯ�� ó������ ������ ��ȸ
//    //    foreach (var _monster in monsterPool)
//    //    {
//    //        if ( _monster.activeSelf == false)
//    //        {
//    //            // ���� ��ȯ
//    //            return _monster;
//    //        }

//    //    }
//    //    return null;

//    //}

//    public void GameOver()
//    {
//        isGameOver = true;
//        gameoverText.SetActive(true);

//        // ���������� �ְ� ����� �ҷ��´�
//        float bestTime = PlayerPrefs.GetFloat("BestTime");

//        // ���������� �ְ� ��Ϻ��� ���� �ð��� �� �� ���
//        if (bestTime < surviveTime)
//        {
//            bestTime = surviveTime;
//            // ����� �ְ� ����� BestTime���� ���
//            PlayerPrefs.SetFloat("BestTime", bestTime);
//        }

        
//        recordText.text = "Best time : " + (int)bestTime;

//    }

    
//}
