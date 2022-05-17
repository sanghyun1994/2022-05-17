using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    // HPBAR
    public Slider hpSlider;

    private Animation playerAnimation;
    private PlayerCtrl playerCtrl;
    private FireCtrl playerFire;

    private void Awake()
    {
        playerAnimation = GetComponent<Animation>();
        playerCtrl = GetComponent<PlayerCtrl>();
        playerFire = GetComponent<FireCtrl>();

    }

    protected override void OnEnable()
    {
        base.OnEnable();

        // 체력바 활성화
        hpSlider.gameObject.SetActive(true);
        // 체력바의 최댓값을 기본 체력값으로 변경
        hpSlider.maxValue = startingHealth;
        // 체력바의 값을 현재 체력 값으로 변경
        hpSlider.value = health;

        // 조작 받는 컴포넌트들을 활성화
        playerCtrl.enabled = true;
        playerFire.enabled = true;

    }

    public override void RestoreHealth(float newHealth)
    {
        base.RestoreHealth(newHealth);
        hpSlider.value = health;
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!dead)
        {
            base.OnDamage(damage, hitPoint, hitNormal);
            hpSlider.value = health;
        }
    }

    public override void Die()
    {
        base.Die();

        hpSlider.gameObject.SetActive(false);
        gameObject.SetActive(false);

        // 조작 받는 컴포넌트 2개를 비활성화
        playerCtrl.enabled = false;
        playerFire.enabled = false;

    }

    private void OnTriggerEnter(Collider coil)
    {
        // 아이템과 충돌했을 경우 아이템을 사용
        // 사망상태가 아닐 경우에만 아이템을 사용 가능하도록 한다

        if(!dead)
        {

            if (health >= 0.0f && coil.CompareTag("FIELDBULLET"))
            {
                health -= 40.0f;
                hpSlider.value = health;

                if (health <= 0.0f)
                {
                    Die();
                }

                // 충돌 대상으로부터 아이템 컴포 추출
                //IItem item = coil.GetComponent<IItem>();

                //if (item != null)
                //{
                //    item.Use(gameObject);
                //}
            }

        }
        
     
    }
    



}
