using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public Player player;

    private IInteractable currentInteractable;

    public void InitPlayer()
    {
        player = GetComponent<PlayerHandler>().player;

    }

    private void Update()
    {

        if (player != null)
        {
            if (player.GetButton("Interact") && currentInteractable != null)
            {
                currentInteractable.Interation();
            }
            if (player.GetButtonUp("Interact") && currentInteractable != null)
            {
                currentInteractable.StopInteration();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<IInteractable>() != null)
            currentInteractable = collision.GetComponent<IInteractable>();

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<IInteractable>() != null)
        {
            currentInteractable.StopInteration();
            currentInteractable = null;
        }
    }
}
