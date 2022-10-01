using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHandler : MonoBehaviour
{
    public Player player;
    public int controllerId = 0;
    public int playerId;
    public UnityEvent playerLoaded;
    [SerializeField] private BoolVariable isStunned;
    private float stunnedTimer = 0;

    void Awake()
    {
        player = ReInput.players.GetPlayer(controllerId);
        playerLoaded?.Invoke();
    }

    private void Update()
    {
        if (isStunned.value && stunnedTimer > 0)
        {
            stunnedTimer -= Time.deltaTime;
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
