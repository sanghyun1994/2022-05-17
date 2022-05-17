using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    public WeaponCtrl gun;
    private PlayerCtrl playerCtrl;
    
    void Start()
    {
        playerCtrl = GetComponent<PlayerCtrl>();

    }

    private void OnEnable()
    {
        gun.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        gun.gameObject.SetActive(false);
    }

    void Update()
    {
        if (playerCtrl.fire)
        {
            gun.Fire();
        }
        else if (playerCtrl.reload)
        {
            gun.Reload();
        }
    }

    private void UpdateUI()
    {
        
    }
}
