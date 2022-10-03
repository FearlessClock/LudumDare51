using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public Player player;

    private IInteractable currentInteractable;
    private List<IInteractable> interactableInRange;
    private BoolVariable isStunned;

    public void InitPlayer()
    {
        player = GetComponent<PlayerHandler>().player;
        isStunned = GetComponent<PlayerHandler>().isStunned;
        currentInteractable = null;
        interactableInRange = new List<IInteractable>();
    }

    private void Update()
    {

        if (player != null && !isStunned.value)
        {
            if (player.GetButton("Interact"))
            {
                if (currentInteractable != null)
                {
                    currentInteractable.Interation();
                }
                else
                {
                    GetInteractionPriority();
                }
            }
            else if (player.GetButtonUp("Interact"))
            {
                if (currentInteractable != null)
                    currentInteractable.StopInteration();
            }
        }
    }

    private void GetInteractionPriority()
    {
        if(currentInteractable != null)
        {
            currentInteractable.StopInteration();
        }
        currentInteractable = null;

        int x = -1;
        for (int i = 0; i < interactableInRange.Count; i++)
        {
            if (x == -1 || interactableInRange[i].GetPriority() > interactableInRange[x].GetPriority())
            {
                 x = i;
            }
        }

        if(x != -1)
        {
            currentInteractable = interactableInRange[x];
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractable interact = collision.GetComponent<IInteractable>();
        if (interact != null)
        {
            interactableInRange.Add(interact);
            interact?.ShowOutline(true);
        }
        else if (collision.CompareTag("Egg"))
        {
            ResourcesManager.Instance.AddEgg(1);
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable interact = collision.GetComponent<IInteractable>();
        if (interact != null  )
        {
            interact?.ShowOutline(false);
            interactableInRange.Remove(interact);
            if (interact == currentInteractable)
            {
                currentInteractable.StopInteration();
                currentInteractable = null;
            }
        }
    }
}
