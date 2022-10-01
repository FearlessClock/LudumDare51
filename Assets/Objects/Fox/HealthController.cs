using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private float healthPoints = 2;
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

    private void Die()
    {
        Destroy(this.gameObject);
    }
}
