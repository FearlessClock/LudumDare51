using HelperScripts.EventSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private float healthPoints = 2;
    private float health = 0;
    [SerializeField] private EventObjectScriptable diedEvent;
    [SerializeField] private float timeInvicible = 2;
    public bool isInvunerable = false;
    private float timer = 0;
    [SerializeField] private ParticleSystem hitParticles;

    private void Start()
    {
        health = healthPoints;
    }

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
            health -= damage;
            isInvunerable = true;
            hitParticles?.Play();
        }

        if(health <= 0)
        {
            Die();
            return true;
        }
        return false;
    }

    public void AddHealth(float amount)
    {
        health += amount;
        if (health > healthPoints)
        {
            health = healthPoints;
        }
    }

    private void Die()
    {
        diedEvent?.Call(this.gameObject);
    }
}
