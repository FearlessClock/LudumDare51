using HelperScripts.EventSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private float healthPoints = 2;
    [SerializeField] private EventObjectScriptable diedEvent; 
    public bool Hit(float damage)
    {
        healthPoints -= damage;
        if(healthPoints <= 0)
        {
            Die();
            return true;
        }
        return false;
    }
    
    public void AddHealth(float health)
    {
        healthPoints += health;
    }

    private void Die()
    {
        diedEvent?.Call(this.gameObject);
    }
}
