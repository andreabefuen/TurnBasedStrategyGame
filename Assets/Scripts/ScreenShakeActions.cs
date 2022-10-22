using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{
    void Start()
    {
        ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
        GranadeProjectile.OnAnyGranadeExploted += GranadeProjectile_OnAnyGranadeExploted;
    }

    private void GranadeProjectile_OnAnyGranadeExploted(object sender, EventArgs e)
    {
        ScreenShake.Instance.Shake(5f);
    }

    private void ShootAction_OnAnyShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        ScreenShake.Instance.Shake();
    }
}
