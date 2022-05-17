using System.Collections;
using UnityEngine;


// �ݵ�� �ʿ��� ������Ʈ�� ǥ���� �ش� ������Ʈ�� ������� ���� ������
[RequireComponent(typeof(AudioSource))]
public class WeaponCtrl : MonoBehaviour
{
    public enum State
    {
        Ready,  // �߻� ����
        Empty,  // źâ ��
        Reload  // ��������
    }

    public State state { get; private set; }    // ���� ���� ����

    public int maxBullet = 50;                  // ���� ��ü ź��
    public int capacityBullet = 10;             // źâ �뷮
    public int numBullet;                       // źâ�� ���� �����ϴ� ź ����

    public float timeFire = 0.15f;              // �߻� ����
    public float reloadTime = 1.5f;             // ������ �ð�
    public float lastFireTime;                  // ���������� ���� �� �ð�

    public float damage = 20;                   // ���� ���ݷ�
    private float fireDistance = 50.0f;         // ���� ���� �Ÿ�

    // �Ѿ� �߻� ��ǥ
    public Transform firepos;

    // �ѹ߻� ����
    public AudioClip fireSfx;
    // ������ ����
    public AudioClip reloadSfx;

    // ���� ǥ�ÿ� ������
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

        // firepos������ muzzleflash�� meterial����
        muzzleFlash = firepos.GetComponentInChildren<MeshRenderer>();
        // ù ���� ��Ȱ��ȭ
        muzzleFlash.enabled = false;
    }

    private void OnEnable()
    {
        // ���� źâ�� ���� ä���.
        numBullet = capacityBullet;
        // ���� �� �� �ִ� �غ���·� ����
        state = State.Ready;
        // ���������� ���� �� �ð��� �ʱ�ȭ
        lastFireTime = 0;

    }

    void Update()
    {
        //// ����ĳ��Ʈ�� �ð������� ǥ���ϱ� ���� ���
        //Debug.DrawRay(firepos.position, firepos.forward * 10.0f, Color.green);


        //if (Input.GetMouseButtonDown(0))
        //{
        //    //Fire();

        //    // ���̸� ��
        //    if (Physics.Raycast(firepos.position, firepos.forward, out hit, 10.0f, 1 << 6))
        //    {
        //        Debug.Log($"HIT={hit.transform.name}");
        //        //hit.transform.GetComponent<MonsterCtrl>()?.OnDamage(hit.point, hit.normal);

        //    }

        //}



    }

    public void Fire()
    {
        // ���� ���� ���°� �߻� ������ �����̸� ������ �߻� �������� �߻簣���� ���
        if (state == State.Ready && Time.time >= lastFireTime + timeFire)
        {
            // ������ �� �߻� ������ ����
            lastFireTime = Time.time;
            // ���� �߻� ó��
            Shot();
                
        }    

    //    Instantiate(bullet, firepos.position, firepos.rotation);

    //    audio.PlayOneShot(fireSfx, 1.0f);
    //    StartCoroutine(ShowMuzzleFlash());

    }

    IEnumerator ShotEffect(Vector3 hitPosition)
    {

        // ������ ��ǥ���� �����Լ��� ����
        Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2) * 0.5f);
        // �ؽ����� ������ �� ����
        muzzleFlash.material.mainTextureOffset = offset;

        // muzzleflash ȸ�� �ݰ�
        float angle = Random.Range(0, 360);
        muzzleFlash.transform.localRotation = Quaternion.Euler(0, 0, angle);

        // muzzleflash ũ�� ����
        float scale = Random.Range(1.0f, 2.0f);
        muzzleFlash.transform.localScale = Vector3.one * scale;

        muzzleFlash.enabled = true;

        yield return new WaitForSeconds(0.2f);

        muzzleFlash.enabled = false;

        audio.PlayOneShot(fireSfx, 1.0f);

        // ���� ������ �ѱ��� ��ġ
        bulletLineRenderer.SetPosition(0, firepos.position);
        // ���� ������ �Է����� ���� �浹 ��ġ
        bulletLineRenderer.SetPosition(1, hitPosition);
        // ���� �������� Ȱ��ȭ�� ź�� ������ �׸�
        bulletLineRenderer.enabled = true;

        // 0.03�ʵ��� ��� ó��
        yield return new WaitForSeconds(0.03f);

        // ���� �������� �ٽ� ��Ȱ���� ź�� ������ ����
        bulletLineRenderer.enabled = false;
        
    }

    private void Shot()
    {
        RaycastHit hit;
        // ź�� ������ ���� ������ ����
        Vector3 hitPosition = Vector3.zero;

        // ����ĳ��Ʈ(���� ����, ����, �浹 ����, �����Ÿ�)
        if (Physics.Raycast(firepos.position, firepos.forward, out hit, fireDistance))
        {
            // ���̰� � ��ü�� �浹���� ��� �浹�� ������κ��� ������Ʈ�� �����´�.
            IDamageable target = hit.collider.GetComponent<IDamageable>();
            // IDamageable ������Ʈ�� �������� �� �����ߴٸ�
            if (target != null)
            {
                // ������ Ondamage�޼��带 ������� �������� �ش�.
                target.OnDamage(damage, hit.point, hit.normal) ;
            }
            // ���̰� �浹�� ��ġ ����
            hitPosition = hit.point;

        }
        else
        {
            // ���̰� �ٸ� ��ü�� �浹���� �ʾҴٸ�
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
        // ���� ���¸� ���������� ��ȯ
        state = State.Reload;
        audio.PlayOneShot(reloadSfx);

        yield return new WaitForSeconds(reloadTime);

        int fillBullet = capacityBullet - numBullet;

        // ä�� ź�� ���� ź�˺��� ������� ä�� ź�˼��� ��ü ź�˼����� ����
        if (maxBullet < fillBullet)
        {
            fillBullet = maxBullet;
        }

        numBullet += fillBullet;
        maxBullet -= fillBullet;

        state = State.Ready;

    }
   




}
