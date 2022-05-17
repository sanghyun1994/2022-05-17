using System.Collections;
using UnityEngine;


// 반드시 필요한 컴포넌트를 표기해 해당 컴포넌트가 사라지는 것을 방지함
[RequireComponent(typeof(AudioSource))]
public class WeaponCtrl : MonoBehaviour
{
    public enum State
    {
        Ready,  // 발사 가능
        Empty,  // 탄창 빔
        Reload  // 재장전중
    }

    public State state { get; private set; }    // 현재 총의 상태

    public int maxBullet = 50;                  // 남은 전체 탄알
    public int capacityBullet = 10;             // 탄창 용량
    public int numBullet;                       // 탄창내 현재 존재하는 탄 숫자

    public float timeFire = 0.15f;              // 발사 간격
    public float reloadTime = 1.5f;             // 재장전 시간
    public float lastFireTime;                  // 마지막으로 총을 쏜 시간

    public float damage = 20;                   // 총의 공격력
    private float fireDistance = 50.0f;         // 총의 사정 거리

    // 총알 발사 좌표
    public Transform firepos;

    // 총발사 음원
    public AudioClip fireSfx;
    // 재장전 음원
    public AudioClip reloadSfx;

    // 궤적 표시용 랜더러
    private LineRenderer bulletLineRenderer;

    private new AudioSource audio;
    private MeshRenderer muzzleFlash;

    private RaycastHit hit;

    
    void Awake()
    {
        audio = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();

        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;

        // firepos하위의 muzzleflash의 meterial추출
        muzzleFlash = firepos.GetComponentInChildren<MeshRenderer>();
        // 첫 시작 비활성화
        muzzleFlash.enabled = false;
    }

    private void OnEnable()
    {
        // 현재 탄창을 가득 채운다.
        numBullet = capacityBullet;
        // 총을 쏠 수 있는 준비상태로 변경
        state = State.Ready;
        // 마지막으로 총을 쏜 시간을 초기화
        lastFireTime = 0;

    }

    void Update()
    {
        //// 레이캐스트를 시각적으로 표시하기 위해 사용
        //Debug.DrawRay(firepos.position, firepos.forward * 10.0f, Color.green);


        //if (Input.GetMouseButtonDown(0))
        //{
        //    //Fire();

        //    // 레이를 쏨
        //    if (Physics.Raycast(firepos.position, firepos.forward, out hit, 10.0f, 1 << 6))
        //    {
        //        Debug.Log($"HIT={hit.transform.name}");
        //        //hit.transform.GetComponent<MonsterCtrl>()?.OnDamage(hit.point, hit.normal);

        //    }

        //}



    }

    public void Fire()
    {
        // 현재 총의 상태가 발사 가능한 상태이며 마지막 발사 시점에서 발사간격이 경과
        if (state == State.Ready && Time.time >= lastFireTime + timeFire)
        {
            // 마지막 총 발사 시점을 갱신
            lastFireTime = Time.time;
            // 실제 발사 처리
            Shot();
                
        }    

    //    Instantiate(bullet, firepos.position, firepos.rotation);

    //    audio.PlayOneShot(fireSfx, 1.0f);
    //    StartCoroutine(ShowMuzzleFlash());

    }

    IEnumerator ShotEffect(Vector3 hitPosition)
    {

        // 오프셋 좌표값을 랜덤함수로 생성
        Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2) * 0.5f);
        // 텍스쳐의 오프셋 값 설정
        muzzleFlash.material.mainTextureOffset = offset;

        // muzzleflash 회전 반경
        float angle = Random.Range(0, 360);
        muzzleFlash.transform.localRotation = Quaternion.Euler(0, 0, angle);

        // muzzleflash 크기 조절
        float scale = Random.Range(1.0f, 2.0f);
        muzzleFlash.transform.localScale = Vector3.one * scale;

        muzzleFlash.enabled = true;

        yield return new WaitForSeconds(0.2f);

        muzzleFlash.enabled = false;

        audio.PlayOneShot(fireSfx, 1.0f);

        // 선의 시작은 총구의 위치
        bulletLineRenderer.SetPosition(0, firepos.position);
        // 선의 끝점은 입력으로 들어온 충돌 위치
        bulletLineRenderer.SetPosition(1, hitPosition);
        // 라인 랜더러를 활성화해 탄의 궤적을 그림
        bulletLineRenderer.enabled = true;

        // 0.03초동안 대기 처리
        yield return new WaitForSeconds(0.03f);

        // 라인 랜더러를 다시 비활성해 탄알 궤적을 지움
        bulletLineRenderer.enabled = false;
        
    }

    private void Shot()
    {
        RaycastHit hit;
        // 탄이 적중한 곳을 저장할 변수
        Vector3 hitPosition = Vector3.zero;

        // 레이캐스트(시작 지점, 방향, 충돌 정보, 사정거리)
        if (Physics.Raycast(firepos.position, firepos.forward, out hit, fireDistance))
        {
            // 레이가 어떤 물체와 충돌했을 경우 충돌한 대상으로부터 오브젝트를 가져온다.
            IDamageable target = hit.collider.GetComponent<IDamageable>();
            // IDamageable 오브젝트를 가져오는 데 성공했다면
            if (target != null)
            {
                // 상대방의 Ondamage메서드를 실행시켜 데미지를 준다.
                target.OnDamage(damage, hit.point, hit.normal) ;
            }
            // 레이가 충돌한 위치 저장
            hitPosition = hit.point;

        }
        else
        {
            // 레이가 다른 물체와 충돌하지 않았다면
            hitPosition = firepos.position + firepos.forward * fireDistance;
        }

        StartCoroutine(ShotEffect(hitPosition));

        numBullet --;
        if (numBullet <= 0)
        {
            state = State.Empty;
        }


    }

    public bool Reload()
    {
        if (state == State.Reload || maxBullet <= 0 || numBullet >= capacityBullet)
        {
            return false;
        }
        StartCoroutine(ReloadRoutine());
        return true;

    }

    private IEnumerator ReloadRoutine()
    {
        // 현재 상태를 재장전으로 전환
        state = State.Reload;
        audio.PlayOneShot(reloadSfx);

        yield return new WaitForSeconds(reloadTime);

        int fillBullet = capacityBullet - numBullet;

        // 채울 탄이 남은 탄알보다 많을경우 채울 탄알수를 전체 탄알수에서 줄임
        if (maxBullet < fillBullet)
        {
            fillBullet = maxBullet;
        }

        numBullet += fillBullet;
        maxBullet -= fillBullet;

        state = State.Ready;

    }
   




}
