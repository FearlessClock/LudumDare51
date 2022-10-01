using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggController : MonoBehaviour
{
    [SerializeField] private float timeToHatch = 10;
    [SerializeField] private float checkRadius = 1;
    [SerializeField] private LayerMask breedingPlaceLayer = 0;
    private float timer = 0;
    [SerializeField] private GameObject catPrefab = null;
    private bool hasHatched = false;

    private void Update()
    {
        Collider2D coll = Physics2D.OverlapCircle(this.transform.position, checkRadius, breedingPlaceLayer);
        if (coll)
        {
            timer -= TimeManager.deltaTime;
            if(timer <= 0 && !hasHatched)
            {
                hasHatched = true;
                Hatch();
            }
        }
        else
        {
            timer = timeToHatch;
        }
    }

    private void Hatch()
    {
        Instantiate<GameObject>(catPrefab, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(this.transform.position + Vector3.up * 0.7f, this.transform.position + Vector3.up * 0.7f + Vector3.right * (timer/timeToHatch));

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, checkRadius);
    }
}
