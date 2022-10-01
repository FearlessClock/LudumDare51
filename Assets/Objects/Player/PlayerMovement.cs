using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerMovement : MonoBehaviour
{
    Player player;

    void Awake()
    {
        player = ReInput.players.GetPlayer(0);
        Debug.Log(player.name);
    }

    void Update()
    {
        if (player.GetAxis("MoveHorizontal") !=0)
        {
            Debug.Log("it worked");
        }

        if (player.GetButton("Interact")) 
        {
            Debug.Log("it worked");
        }
    }
}
