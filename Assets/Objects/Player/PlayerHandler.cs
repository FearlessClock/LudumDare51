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

    void Awake()
    {
        player = ReInput.players.GetPlayer(controllerId);
        playerLoaded?.Invoke();
    }


    void Update()
    {
        
    }
}
