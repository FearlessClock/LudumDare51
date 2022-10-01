using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public Player player;

    //private IInteract currentInteractable

    public void InitPlayer()
    {
        player = GetComponent<PlayerHandler>().player;

    }

    private void Update()
    {

        if (player != null)
        {
            if (player.GetButton("Interact") /*currentInteractable != null*/)
            {
                Debug.Log("interacted");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if(collision.GetComponent<IInteract>())

        //currentInteractable = collision.GetComponent<IInteract>();
    }
}
