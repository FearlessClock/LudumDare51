using HelperScripts.EventSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private float healthPoints = 2;
    [SerializeField] private EventObjectScriptable diedEvent;
    [SerializeField] private float timeInvicible = 2;
    public bool isInvunerable = false;
    private float timer = 0;

    private void FixedUpdate()
    {
        if (isInvunerable)
            timer += TimeManager.fixedDeltaTime;
        if (timer >= timeInvicible)
        {
            timer = 0;
            isInvunerable = false;
        }
    }

    public bool Hit(float damage)
    {
        if (!isInvunerable)
        {
            healthPoints -= damage;
            isInvunerable = true;
        }

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
