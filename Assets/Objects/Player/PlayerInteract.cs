using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public Player player;

    private IInteractable currentInteractable;
    private BoolVariable isStunned;

    public void InitPlayer()
    {
        player = GetComponent<PlayerHandler>().player;
        isStunned = GetComponent<PlayerHandler>().isStunned;
    }

    private void Update()
    {

        if (player != null && !isStunned.value)
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
