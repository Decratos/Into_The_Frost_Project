using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWeapon : WeaponsClass
{
    private void Start() {
        InvokeRepeating("attackRateManagement", 0, attackRate);
    }

    public override void Shoot(WeaponsClass myWeapon)
    {
        base.Shoot(this);
        actualAttackRate = attackRate;
    }

    public override void Reload(WeaponsClass myWeapon)
    {
        base.Reload(this);
    }

    public override void attackRateManagement()
    {
        actualAttackRate = 0;
    }

    public void StartReloading()
    {
        Invoke("Reload", reloadSpeed);
    }
}
