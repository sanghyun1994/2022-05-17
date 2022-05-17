using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// IDamageable�� ��ӹ޾ұ� ������ �ݵ�� onDamage()�޼ҵ带 �����ؾ��Ѵ�.


public class LivingEntity : MonoBehaviour, IDamageable
{
    // ���� ü��
    public float startingHealth = 100f;
    // ���� ü��
    public float health { get; protected set; }
    // ��� ����
    public bool dead { get; protected set; }
    // ����� �ߵ� �̺�Ʈ
    public event Action onDeath; 

    // ����ü�� Ȱ��ȭ�ɶ� ���¸� �����Ѵ�.
    protected virtual void OnEnable()
    {
        // ������ ���·� ����
        dead = false;
        // ü���� ���� ü������ �ʱ�ȭ
        health = startingHealth;
    }

    // �������� �޴� �޼���
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        // �޴� ��������ŭ ü�� ����
        health -= damage;

        // ü���� 0 �����̸鼭 ������ ���¶�� die ó��
        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    // ȸ�� �޼���
    public virtual void RestoreHealth(float newHealth)
    {
        if (dead)
        {
            // �̹� ����� ��� ü�� ȸ���� �Ұ�����
            return;
        }

        // ü�� �߰�
        health += newHealth;
    }

    // ���
    public virtual void Die()
    {
        // onDeath �̺�Ʈ�� ��ϵ� �޼��尡 �ִٸ� ����
        if (onDeath != null)
        {
            onDeath();
        }

        // ��� ���·� ����
        dead = true;
    }


}
