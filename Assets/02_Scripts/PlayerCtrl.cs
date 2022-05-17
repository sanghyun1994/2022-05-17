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

    //컴포넌트를 캐시처리화한 변수
    [SerializeField] private Transform tr;
    //애니메이션 변수
    [SerializeField] private Animation ani;
    //이동 속도
    public float moveSpeed = 10.0f;
    //회전 속도
    public float rotSpeed = 80.0f;

    //// 초기 HP값
    //private readonly float initHp = 120.0f;
    //// 현재 HP값
    //public float currHp;
    //// HPBAR
    //private Image hpBar;

    //// 델리게이트 및 이벤트 선언
    //public delegate void PlayerDieHandler();
    //public static event PlayerDieHandler OnPlayerDie;

    IEnumerator Start()
    {
        //// hp바 연결
        //hpBar = GameObject.FindGameObjectWithTag("HP_BAR")?.GetComponent<Image>();
        //// hp값 초기화
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
    //    // 충돌한 collider가 몬스터의 punch에 해당할 경우 player hp감소
    //    if (currHp >= 0.0f && coll.CompareTag("PUNCH"))
    //    {
    //        currHp -= 10.0f;
    //        DisplayHealth();


    //        // plyaer hp가 0이 되었을 경우 사망 처리
    //        if (currHp <= 0.0f)
    //        {
    //            PlayerDie();
    //        }

    //    }
    //    // 충돌한 coll이 fieldbullet 태그에 해당할 경우
    //    else if (currHp >= 0.0f && coll.CompareTag("FIELDBULLET"))
    //    {
    //        // 전체체력 120, 외부 총알에 피격시 3번 피격시 사망
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
        
    //    Debug.Log("플레이어가 사망했습니다 !!");

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
