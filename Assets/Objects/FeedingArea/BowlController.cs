using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlController : Singleton<BowlController>
{
    [SerializeField] private Transform interactionPoint = null;
    public float feedRate = 1;

    public Transform GetInteractionPoint => interactionPoint;
}
