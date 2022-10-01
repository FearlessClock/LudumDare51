using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SingleAnimationHandler : MonoBehaviour
{
    public enum State { INTRO, IDLE, OUTRO }
    [HorizontalGroup("IsUsed", LabelWidth = 30), LabelText("Intro")]
    [SerializeField] private bool isIntroUsed = false;

    [HorizontalGroup("IsUsed"), LabelText("Idle"), OnValueChanged("IdleSettingsChanged")]
    [SerializeField] private bool isIdleUsed = false;

    [HorizontalGroup("IsUsed", LabelWidth = 30), LabelText("Outro")]
    [SerializeField] private bool isOutroUsed = false;

    [HideIf("@!isIntroUsed"), InlineEditor]
    [SerializeField] private AnimationSettings introSettings = null;

    [HideIf("@!isIdleUsed"), InlineEditor, OnValueChanged("IdleSettingsChanged")]
    [SerializeField] private AnimationSettings idleSettings = null;

    [HideIf("@!isIdleUsed")]
    [SerializeField] private int idleOffset = 0;

    [HideIf("@!isOutroUsed"), InlineEditor]
    [SerializeField] private AnimationSettings outroSettings = null;

    [HideIf("@!isOutroUsed")]
    public UnityEvent OnOutroEnd = null;

    private State currentState = State.INTRO; 

    private Image image = null;

    private int currentIndex = 0;

    private float timer = 0;

    private void OnDestroy()
    {
        if(idleSettings != null)
        {
            Destroy(idleSettings);
        }
    }

    private void IdleSettingsChanged()
    {
        image = GetComponent<Image>();
        if (image && idleSettings != null)
        {
            image.sprite = idleSettings.GetSprites()[0];
        }
    }

    public void SetAnimationSettings(State state, AnimationSettings animationSettings)
    {
        switch (state)
        {
            case State.INTRO:
                introSettings = Instantiate(animationSettings);
                break;
            case State.IDLE:
                idleSettings = Instantiate(animationSettings);
                break;
            case State.OUTRO:
                outroSettings = Instantiate(animationSettings);
                break;
            default:
                break;
        }
        if(image == null)
        {
            image = GetComponent<Image>();
        }
        AnimationSettings setts = GetCurrentSettings(currentState);
        if (setts)
        {
            image.sprite = setts.GetSprite(ref currentIndex);
        }
    }

    private void Awake()
    {
        image = GetComponent<Image>();

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
            currentIndex = idleOffset;
        }
        else
        {
            SwapToState(State.INTRO);
        }
        if(outroSettings != null)
        {
            outroSettings.OnEndEvent += OnOutroEndEvent;
        }
        image.sprite = GetCurrentSettings(currentState).GetSprite(ref currentIndex);
    }

    private void OnOutroEndEvent()
    {
        OnOutroEnd?.Invoke();
    }

    private void Update()
    {
        timer += Time.unscaledDeltaTime;
        if(timer >= 1.0f/ GetCurrentSettings(currentState).fps)
        {
            image.sprite = GetCurrentSettings(currentState).GetSprite(ref currentIndex);
            timer = 0;
        }
    }

    private State CheckTransition(State state)
    {
        switch (state)
        {
            case State.INTRO:
                if (isIdleUsed)
                {
                    return State.IDLE;
                }
                return State.INTRO;
            case State.IDLE:
                return State.IDLE;
            case State.OUTRO:
                if (isIdleUsed)
                {
                    return State.IDLE;
                }
                return State.OUTRO;
        }
        return State.IDLE;
    }

    private AnimationSettings GetCurrentSettings(State state)
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
            GetCurrentSettings(currentState).OnFinishedLoop -= FinishedLoop;
        }
        currentState = state;
        currentIndex = 0;
        if (CurrentSettingsIsUsed(currentState))
        {
            GetCurrentSettings(currentState).OnFinishedLoop += FinishedLoop;
        }
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
        State state = CheckTransition(currentState);
        if(currentState != state)
        {
            SwapToState(state);
        }
    }

    public void ShowOutro()
    {
        SwapToState(State.OUTRO);
    }
}

