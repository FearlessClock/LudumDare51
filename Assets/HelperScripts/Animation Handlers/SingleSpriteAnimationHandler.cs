using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SingleSpriteAnimationHandler : MonoBehaviour
{
    enum State { INTRO, IDLE, OUTRO }
    [HorizontalGroup("IsUsed", LabelWidth = 30), LabelText("Intro")]
    [SerializeField] private bool isIntroUsed = false;

    [HorizontalGroup("IsUsed"), LabelText("Idle")]
    [SerializeField] private bool isIdleUsed = false;

    [HorizontalGroup("IsUsed", LabelWidth = 30), LabelText("Outro")]
    [SerializeField] private bool isOutroUsed = false;
    [HideIf("@!isIntroUsed"), InlineEditor]
    [SerializeField] private AnimationSettings introSettings = null;
    [HideIf("@!isIdleUsed"), InlineEditor, OnValueChanged("IdleSettingsChanged")]
    [SerializeField] private AnimationSettings idleSettings = null;
    [HideIf("@!isOutroUsed"), InlineEditor]
    [SerializeField] private AnimationSettings outroSettings = null;
    private State currentState = State.INTRO;

    private SpriteRenderer spriteRenderer = null;

    private float timer = 0;

    private int currentIndex = 0;

    public Action OnFinishedLoop = null;

    private void IdleSettingsChanged()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer && idleSettings != null)
        {
            spriteRenderer.sprite = idleSettings.GetSprites()[0];
        }
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (isIntroUsed && introSettings != null)
        {
            introSettings = Instantiate(introSettings);
        }
        if (isIdleUsed && idleSettings != null)
        {
            idleSettings = Instantiate(idleSettings);
        }
        if (isOutroUsed && outroSettings != null)
        {
            outroSettings = Instantiate(outroSettings);
        }

        if (currentState == State.INTRO && !isIntroUsed)
        {
            SwapToState(State.IDLE);
        }
        else
        {
            SwapToState(State.INTRO);
        }
        spriteRenderer.sprite = GetCurrentAnimationSettings(currentState).GetSprite(ref currentIndex);
    }

    private void Update()
    {
        timer += Time.unscaledDeltaTime;
        if (timer >= 1.0f / GetCurrentAnimationSettings(currentState).fps)
        {
            spriteRenderer.sprite = GetCurrentAnimationSettings(currentState).GetSprite(ref currentIndex);
            timer = 0;
        }
    }

    private State CheckTransition(State state)
    {
        switch (state)
        {
            case State.INTRO:
                return State.IDLE;
            case State.IDLE:
                return State.IDLE;
            case State.OUTRO:
                return State.IDLE;
        }
        return State.IDLE;
    }

    private AnimationSettings GetCurrentAnimationSettings(State state)
    {
        switch (state)
        {
            case State.INTRO:
                return introSettings;
            case State.IDLE:
                return idleSettings;
            case State.OUTRO:
                return outroSettings;
        }
        return new SingleAnimationSettings();
    }

    private void SwapToState(State state)
    {
        if (CurrentSettingsIsUsed(currentState))
        {
            GetCurrentAnimationSettings(currentState).OnFinishedLoop -= FinishedLoop;
        }
        currentState = state;
        currentIndex = 0;
        GetCurrentAnimationSettings(currentState).OnFinishedLoop += FinishedLoop;
    }

    private bool CurrentSettingsIsUsed(State currentState)
    {
        switch (currentState)
        {
            case State.INTRO:
                return isIntroUsed;
            case State.IDLE:
                return isIdleUsed;
            case State.OUTRO:
                return isOutroUsed;
        }
        return false;
    }

    private void FinishedLoop()
    {
        SwapToState(CheckTransition(currentState));
        OnFinishedLoop?.Invoke();
    }

    public void ShowOutro()
    {
        SwapToState(State.OUTRO);
    }
    public AnimationSettings GetIdleSO()
    {
        return idleSettings;
    }

}
