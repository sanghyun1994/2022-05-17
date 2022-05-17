using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{

    public string fireButton = "Fire";

    public string reloadButton = "Reload";

    public bool fire { get; private set; }
    public bool reload { get; private set; }

    //������Ʈ�� ĳ��ó��ȭ�� ����
    [SerializeField] private Transform tr;
    //�ִϸ��̼� ����
    [SerializeField] private Animation ani;
    //�̵� �ӵ�
    public float moveSpeed = 10.0f;
    //ȸ�� �ӵ�
    public float rotSpeed = 80.0f;

    //// �ʱ� HP��
    //private readonly float initHp = 120.0f;
    //// ���� HP��
    //public float currHp;
    //// HPBAR
    //private Image hpBar;

    //// ��������Ʈ �� �̺�Ʈ ����
    //public delegate void PlayerDieHandler();
    //public static event PlayerDieHandler OnPlayerDie;

    IEnumerator Start()
    {
        //// hp�� ����
        //hpBar = GameObject.FindGameObjectWithTag("HP_BAR")?.GetComponent<Image>();
        //// hp�� �ʱ�ȭ
        //currHp = initHp;
        

        tr = GetComponent<Transform>();
        ani = GetComponent<Animation>();

        ani.Play("Idle");

        rotSpeed = 0.0f;
        yield return new WaitForSeconds(0.3f);
        rotSpeed = 160.0f;
    }

    
    void Update()
    {

        if ( GameManager.instance != null && GameManager.instance.isGameover)
        {

        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float r = Input.GetAxis("Mouse X");

        fire = Input.GetButton("fireButton");
        reload = Input.GetButton("reloadButton");


        //Debug.Log("h=" + h);
        //Debug.Log("v=" + v);

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        tr.Translate(moveDir.normalized * Time.deltaTime * moveSpeed);
        tr.Rotate(Vector3.up * rotSpeed * Time.deltaTime * r);

        PlayerAni(h, v);

    }

    void PlayerAni(float h, float v)
    {
        if( v>=0.1f)
        {
            ani.CrossFade("RunF", 0.25f);
        }
        else if (v<= -0.1f)
        {
            ani.CrossFade("RunB", 0.25f);
        }
        else if ( h>= 0.1f)
        {
            ani.CrossFade("RunR", 0.25f);
        }
        else if (h <= -0.1f)
        {
            ani.CrossFade("RunL", 0.25f);
        }
        else
            ani.CrossFade("Idle", 0.25f);

    }

    //void OnTriggerEnter(Collider coll)
    //{
    //    // �浹�� collider�� ������ punch�� �ش��� ��� player hp����
    //    if (currHp >= 0.0f && coll.CompareTag("PUNCH"))
    //    {
    //        currHp -= 10.0f;
    //        DisplayHealth();


    //        // plyaer hp�� 0�� �Ǿ��� ��� ��� ó��
    //        if (currHp <= 0.0f)
    //        {
    //            PlayerDie();
    //        }

    //    }
    //    // �浹�� coll�� fieldbullet �±׿� �ش��� ���
    //    else if (currHp >= 0.0f && coll.CompareTag("FIELDBULLET"))
    //    {
    //        // ��üü�� 120, �ܺ� �Ѿ˿� �ǰݽ� 3�� �ǰݽ� ���
    //        currHp -= 40.0f;
    //        DisplayHealth();

    //        if (currHp <= 0.0f)
    //        {
    //            PlayerDie();
    //        }

    //    }

    //}
    //public void PlayerDie()
    //{
        
    //    Debug.Log("�÷��̾ ����߽��ϴ� !!");

    //    //OnPlayerDie();

    //    //GameManager.instance.IsGameOver = true;
    //    gameObject.SetActive(false);

    //    GameManager gameManager = FindObjectOfType<GameManager>();
    //    gameManager.GameOver();

    //}

    //void DisplayHealth()
    //{
    //    hpBar.fillAmount = currHp / initHp;
    //}

}
