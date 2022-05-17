using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// IDamageable을 상속받았기 때문에 반드시 onDamage()메소드를 구현해야한다.


public class LivingEntity : MonoBehaviour, IDamageable
{
    // 시작 체력
    public float startingHealth = 100f;
    // 현재 체력
    public float health { get; protected set; }
    // 사망 상태
    public bool dead { get; protected set; }
    // 사망시 발동 이벤트
    public event Action onDeath; 

    // 생명체가 활성화될때 상태를 리셋한다.
    protected virtual void OnEnable()
    {
        // 생존한 상태로 시작
        dead = false;
        // 체력을 시작 체력으로 초기화
        health = startingHealth;
    }

    // 데미지를 받는 메서드
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        // 받는 데미지만큼 체력 감소
        health -= damage;

        // 체력이 0 이하이면서 생존한 상태라면 die 처리
        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    // 회복 메서드
    public virtual void RestoreHealth(float newHealth)
    {
        if (dead)
        {
            // 이미 사망한 경우 체력 회복이 불가능함
            return;
        }

        // 체력 추가
        health += newHealth;
    }

    // 사망
    public virtual void Die()
    {
        // onDeath 이벤트에 등록된 메서드가 있다면 실행
        if (onDeath != null)
        {
            onDeath();
        }

        // 사망 상태로 변경
        dead = true;
    }


}
