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
    
    private SoundTransmitter st;

    [Header("Blinking")] 
    [SerializeField] private float timeBtwBlink;
    private IEnumerator blikingCoroutine;
    [SerializeField] private SpriteRenderer sr;

    [SerializeField] private GameObject deathParticle;
    

    private void Start()
    {
        health = healthPoints;
        st = GetComponent<SoundTransmitter>();
    }

    private void FixedUpdate()
    {
        if (isInvunerable)
            timer += TimeManager.fixedDeltaTime;
        if (timer >= timeInvicible)
        {
            timer = 0;
            isInvunerable = false;
            StopBlinking();
        }
    }

    public bool Hit(float damage)
    {
        if (!isInvunerable)
        {
            st.Play("Hit");
            health -= damage;
            isInvunerable = true;
            hitParticles?.Play();
            blikingCoroutine = Blink();
            StartCoroutine(blikingCoroutine);
        }

        if(health <= 0)
        {
            Instantiate(deathParticle, transform.position, transform.rotation);
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

    IEnumerator Blink()
    {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);
        yield return new WaitForSeconds(timeBtwBlink);
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
        yield return new WaitForSeconds(timeBtwBlink);
        if(isInvunerable) StartCoroutine(Blink());
    }

    void StopBlinking()
    {
        StopCoroutine(blikingCoroutine);
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
        
    }
}
