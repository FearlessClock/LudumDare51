using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPetting : MonoBehaviour, IInteractable
{
    [SerializeField] private ParticleSystem catPettingLoveParticle = null;
    private HealthController healthController = null;

    private bool isInteracting = false;
    [SerializeField] private float timeToHeal = 10;
    private float healTimer = 0;
    [SerializeField] private SpriteRenderer outlineSprite;
    [SerializeField] private int priority = 1;

    public bool IsBeingPetted => isInteracting;

    public void Interation()
    {
        if (!isInteracting)
        {
            healTimer = timeToHeal;
            catPettingLoveParticle.Play();
        }
        isInteracting = true;
    }

    public void StopInteration()
    {
        isInteracting = false;
        catPettingLoveParticle.Stop();
    }

    private void Update()
    {
        if (isInteracting)
        {
            Action();
        }
    }

    void Action()
    {
        healTimer -= Time.deltaTime;

        if (healTimer <= 0)
        {
            healthController.AddHealth(1);
        }
    }
    public int GetPriority()
    {
        return priority;
    }

    public void ShowOutline(bool show)
    {
        outlineSprite.material.SetFloat("_showOutline", show ? 1 : 0);
    }
}
