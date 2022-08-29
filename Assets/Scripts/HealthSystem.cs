using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDeath;
    public event EventHandler OnDamage;
    [SerializeField] private int healthMax = 100;
    [SerializeField] private int health;

    void Awake()
    {
        health = healthMax;
    }
    public void Damage(int damageAmount){
        health -= damageAmount;

        if(health<0){
            health= 0;
        }
        OnDamage?.Invoke(this, EventArgs.Empty);
        if(health == 0){
            Die();
        }

        Debug.Log(health);
    }

    private void Die(){
        OnDeath?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized(){
        return (float)health / healthMax;
    }
}
