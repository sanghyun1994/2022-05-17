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

        // ü�¹� Ȱ��ȭ
        hpSlider.gameObject.SetActive(true);
        // ü�¹��� �ִ��� �⺻ ü�°����� ����
        hpSlider.maxValue = startingHealth;
        // ü�¹��� ���� ���� ü�� ������ ����
        hpSlider.value = health;

        // ���� �޴� ������Ʈ���� Ȱ��ȭ
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

        // ���� �޴� ������Ʈ 2���� ��Ȱ��ȭ
        playerCtrl.enabled = false;
        playerFire.enabled = false;

    }

    private void OnTriggerEnter(Collider coil)
    {
        // �����۰� �浹���� ��� �������� ���
        // ������°� �ƴ� ��쿡�� �������� ��� �����ϵ��� �Ѵ�

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

                // �浹 ������κ��� ������ ���� ����
                //IItem item = coil.GetComponent<IItem>();

                //if (item != null)
                //{
                //    item.Use(gameObject);
                //}
            }

        }
        
     
    }
    



}
