using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class AnimationSettings : ScriptableObject
{
    public abstract Sprite[] GetSprites();
    public abstract Sprite GetSprite(ref int index);

    public Action OnFinishedLoop = null;
    protected void FinishedLoop()
    {
        OnFinishedLoop?.Invoke();
    }

    [BoxGroup("AnimationInfo")]
    [SerializeField] protected bool reverse;
    [BoxGroup("AnimationInfo")]
    public int fps = 24;
    [BoxGroup("AnimationInfo")]
    [SerializeField] protected bool hasEndEvent;
    public Action OnEndEvent = null;
}
