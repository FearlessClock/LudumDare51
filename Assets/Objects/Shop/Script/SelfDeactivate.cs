using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDeactivate : MonoBehaviour
{
    [SerializeField] private float timeBeforeDeactivate;
    private void Start()
    {
        StartCoroutine(SelfDea());
    }

    IEnumerator SelfDea()
    {
        yield return new WaitForSeconds(timeBeforeDeactivate);
        gameObject.SetActive(false);
    }
}
