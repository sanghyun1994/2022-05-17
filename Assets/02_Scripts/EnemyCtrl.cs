using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCtrl : LivingEntity
{
    // ���� ����� ���̾�
    public LayerMask targetLayer;
    // ���� ���
    private LivingEntity targetEntity;
    private NavMeshAgent pathFinder;

    // �ǰ� ��ƼŬ ȿ��
    public ParticleSystem hitEffect;

    // �ִϸ����� �� ������ ������Ʈ
    private Animator enemyAnimator;
    private Renderer enemyRenderer;

    // ���ݷ�
    public float damage = 20.0f;
    // ���� ����
    public float timeBeAttack = 0.8f;
    // ������ ���� ����
    private float lastAttackTime;

    // ���� ����� �����ϴ��� �˷��ִ� ������Ƽ
    private bool hasTarget
    {
        get
        {
            //���� ����� �����ϰ� ����� ��� �ִٸ� true
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

        // ������ ������Ʈ�� �ڽ� ������Ʈ�� �����ϹǷ� getchildren���
        enemyRenderer = GetComponentInChildren<Renderer>();

    }

    // �� ai�� �ʱ� ������ �����ϴ� �¾� �޼���
    public void Setup(float newHealth, float newDamage, float newSpeed, Color skinColor) 
    {
        // ü�� ����
        startingHealth = newHealth;
        health = newHealth;
        // ���ݷ� ����
        damage = newDamage;
        // ����޽� ������Ʈ�� �̵� �ӵ�
        pathFinder.speed = newSpeed;
        // �������� ������� ���׸����� ���� ����, �������� �����
        enemyRenderer.material.color = skinColor;
    
    }

    private void Start()
    {
        // ���� ������Ʈ Ȱ��ȭ�� ���ÿ� ai�� ���� ��ƾ�� ����
        StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        // ���� ����� ���� ���ο� ���� �ٸ� �ִϸ��̼��� ���
        enemyAnimator.SetBool("IsTrace", hasTarget);
        
    }

    private IEnumerator UpdatePath()
    {
        // ������ ���� ���� ����
        while(!dead)
        {
            if (hasTarget)
            {
                // ���� ����� ������ ��� ��θ� �����ϰ� ai�̵��� ��� ������
                pathFinder.isStopped = false;
                pathFinder.SetDestination(targetEntity.transform.position);

            }
            else
            {
                // ���� ����� ���� ��� ai�̵��� ������
                pathFinder.isStopped = true;

                // 20������ �������� ���� ������ ���� �׷����� ���� ��ġ�� ��� �ݶ��̴��� �����´�
                // �� targetLayer�� ���� �ݶ��̴��� ���������� ���͸�
                Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, targetLayer);

                for (int i = 0; i < colliders.Length; i++)
                {
                    // �ݶ��̴��κ��� livingentity���� ����
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();
                    // livingentity ������Ʈ�� �����ϸ�, �ش� livingentity�� �������¶��
                    if (livingEntity != null && !livingEntity.dead)
                    {
                        // ���� ����� �ش� livingentity�� ����
                        targetEntity = livingEntity;
                        // for ���� ��� �����Ѵ�.
                        break;
                    }
                }
            }
            // 0.25�ʸ� �ֱ�� ��� �ݺ�
            yield return new WaitForSeconds(0.25f);
        }

    }

    // �������� ���� ���
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        // ���� ������� ���� ��츸 �ǰ� ȿ���� ���
        if(!dead)
        {
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();
        }

        base.OnDamage(damage, hitPoint, hitNormal);
    }

    // ��� ó��
    public override void Die()
    {
        base.Die();

        // �ٸ� ai�� �������� �ʵ��� �ڽ��� ��� �ݶ��̴��� ��Ȱ��
        Collider[] enemyColliders = GetComponents<Collider>();
        for ( int i = 0; i < enemyColliders.Length; i++)
        {
            enemyColliders[i].enabled = false;
        }

        // AI������ �����ϰ� ���񿡼� ���� ��Ȱ��ȭ
        pathFinder.isStopped = true;
        pathFinder.enabled = false;

        // ��� �ִϸ��̼� ���
        enemyAnimator.SetTrigger("Die");

    }

    private void OnTriggerStay(Collider coil)
    {
        // ���� �ڽ��� ���������̸�, �ֱ� ���ݽ������κ��� timeBeAttack�� ����ߴٸ� �ٽ� ���� ����
        if(!dead && Time.time >= lastAttackTime + timeBeAttack )
        {
            // ������ livingEntity��������
            LivingEntity attackTarget = coil.GetComponent<LivingEntity>();

            // ������ livingEntity�� ���� ����̶�� ���� ����

            if (attackTarget != null && attackTarget == targetEntity)
            {
                // �ֱ� ���� �ð� ����
                lastAttackTime = Time.time;
                // ������ �ǰ� ��ġ�� �ǰ� ������ �ٻ����� ���
                Vector3 hitPoint = coil.ClosestPoint(transform.position);
                Vector3 hitNormal = transform.position - coil.transform.position;

                // ���� ����
                attackTarget.OnDamage(damage, hitPoint, hitNormal);

                enemyAnimator.SetBool("IsAttack", true);
            }
        }
    }




}
