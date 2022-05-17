using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCtrl : LivingEntity
{
    // 추적 대상의 레이어
    public LayerMask targetLayer;
    // 추적 대상
    private LivingEntity targetEntity;
    private NavMeshAgent pathFinder;

    // 피격 파티클 효과
    public ParticleSystem hitEffect;

    // 애니메이터 및 랜더러 컴포넌트
    private Animator enemyAnimator;
    private Renderer enemyRenderer;

    // 공격력
    public float damage = 20.0f;
    // 공격 간격
    public float timeBeAttack = 0.8f;
    // 마지막 공격 시점
    private float lastAttackTime;

    // 추적 대상이 존재하는지 알려주는 프로퍼티
    private bool hasTarget
    {
        get
        {
            //추적 대상이 존재하고 대상이 살아 있다면 true
            if (targetEntity != null && !targetEntity.dead)
            {
                return true;
            }
            return false;
        }
    }

    private void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();

        // 랜더러 컴포넌트는 자식 오브젝트에 존재하므로 getchildren사용
        enemyRenderer = GetComponentInChildren<Renderer>();

    }

    // 적 ai의 초기 스텟을 결정하는 셋업 메서드
    public void Setup(float newHealth, float newDamage, float newSpeed, Color skinColor) 
    {
        // 체력 설정
        startingHealth = newHealth;
        health = newHealth;
        // 공격력 설정
        damage = newDamage;
        // 내비메시 에이전트의 이동 속도
        pathFinder.speed = newSpeed;
        // 랜더러가 사용중인 메테리얼의 색을 변경, 외형색이 변경됨
        enemyRenderer.material.color = skinColor;
    
    }

    private void Start()
    {
        // 게임 오브젝트 활성화와 동시에 ai의 추적 루틴을 실행
        StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        // 추적 대상의 존재 여부에 따라 다른 애니메이션을 재생
        enemyAnimator.SetBool("IsTrace", hasTarget);
        
    }

    private IEnumerator UpdatePath()
    {
        // 생존한 동안 무한 루프
        while(!dead)
        {
            if (hasTarget)
            {
                // 추적 대상이 존재할 경우 경로를 갱신하고 ai이동을 계속 진행함
                pathFinder.isStopped = false;
                pathFinder.SetDestination(targetEntity.transform.position);

            }
            else
            {
                // 추적 대상이 없을 경우 ai이동을 중지함
                pathFinder.isStopped = true;

                // 20유닛의 반지름을 가진 가상의 구를 그렸을때 구와 겹치는 모든 콜라이더를 가져온다
                // 단 targetLayer를 가진 콜라이더만 가져오도록 필터링
                Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, targetLayer);

                for (int i = 0; i < colliders.Length; i++)
                {
                    // 콜라이더로부터 livingentity컴포 추출
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();
                    // livingentity 컴포넌트가 존재하며, 해당 livingentity가 생존상태라면
                    if (livingEntity != null && !livingEntity.dead)
                    {
                        // 추적 대상을 해당 livingentity로 설정
                        targetEntity = livingEntity;
                        // for 문을 즉시 정지한다.
                        break;
                    }
                }
            }
            // 0.25초를 주기로 계속 반복
            yield return new WaitForSeconds(0.25f);
        }

    }

    // 데미지를 입을 경우
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        // 아직 사망하지 않은 경우만 피격 효과를 재생
        if(!dead)
        {
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();
        }

        base.OnDamage(damage, hitPoint, hitNormal);
    }

    // 사망 처리
    public override void Die()
    {
        base.Die();

        // 다른 ai를 방해하지 않도록 자신의 모든 콜라이더를 비활성
        Collider[] enemyColliders = GetComponents<Collider>();
        for ( int i = 0; i < enemyColliders.Length; i++)
        {
            enemyColliders[i].enabled = false;
        }

        // AI추적을 중지하고 내비에서 컴포 비활성화
        pathFinder.isStopped = true;
        pathFinder.enabled = false;

        // 사망 애니메이션 재생
        enemyAnimator.SetTrigger("Die");

    }

    private void OnTriggerStay(Collider coil)
    {
        // 몬스터 자신이 생존상태이며, 최근 공격시점으로부터 timeBeAttack이 경과했다면 다시 공격 가능
        if(!dead && Time.time >= lastAttackTime + timeBeAttack )
        {
            // 상대방의 livingEntity가져오기
            LivingEntity attackTarget = coil.GetComponent<LivingEntity>();

            // 상대방의 livingEntity가 추적 대상이라면 공격 실행

            if (attackTarget != null && attackTarget == targetEntity)
            {
                // 최근 공격 시간 갱신
                lastAttackTime = Time.time;
                // 상대방의 피격 위치와 피격 방향을 근삿값으로 계산
                Vector3 hitPoint = coil.ClosestPoint(transform.position);
                Vector3 hitNormal = transform.position - coil.transform.position;

                // 공격 실행
                attackTarget.OnDamage(damage, hitPoint, hitNormal);

                enemyAnimator.SetBool("IsAttack", true);
            }
        }
    }




}
