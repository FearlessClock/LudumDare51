using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWTime : MonoBehaviour
{
    [SerializeField] private float timeBeforeDelete;
    private void Start()
    {
        Destroy(gameObject, timeBeforeDelete);
    }
}
