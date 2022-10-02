using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHandler : MonoBehaviour
{
    public Player player;
    public int playerId;
    public UnityEvent playerLoaded;
    public BoolVariable isStunned;
    private float stunnedTimer = 0;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer bodyRenderer;
    [SerializeField] private SpriteRenderer handRenderer;

    public void PlayerInit(int id, BoolVariable stunnedVariable)
    {
        playerId = id;
        player = ReInput.players.GetPlayer(playerId);
        isStunned = stunnedVariable;
        playerLoaded?.Invoke();

        (bodyRenderer.sprite, handRenderer.sprite) = MultiManager.Instance.GetSkin(id);
    }

    private void Update()
    {
        if (isStunned.value && stunnedTimer > 0)
        {
            stunnedTimer -= TimeManager.deltaTime;
            if(stunnedTimer <= 0)
            {
                isStunned.SetValue(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("stun");
            StunPlayer(2);
        }
    }

    public void StunPlayer(float duration)
    {
        stunnedTimer = duration;
        isStunned.SetValue(true);
    }
}
