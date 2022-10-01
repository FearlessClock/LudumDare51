using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public Player player;

    private IInteractable currentInteractable;
    private BoolVariable isStunned;
    private bool isInBreedingArea;

    public void InitPlayer()
    {
        player = GetComponent<PlayerHandler>().player;
        isStunned = GetComponent<PlayerHandler>().isStunned;
        currentInteractable = null;
    }

    private void Update()
    {

        if (player != null && !isStunned.value)
        {
            if (player.GetButton("Interact") )
            {
                if (isInBreedingArea)
                {
                    Debug.Log("breed");
                }
                else if(currentInteractable != null)
                {
                    currentInteractable.Interation();
                }
            }
            else if (player.GetButtonUp("Interact") && currentInteractable != null)
            {
                currentInteractable.StopInteration();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IInteractable>() != null)
        {
            if (currentInteractable != null)
            {
                currentInteractable.StopInteration();
                currentInteractable = null;
            }
            currentInteractable = collision.GetComponent<IInteractable>();
        }
        else if (collision.CompareTag("Egg"))
        {
            ResourcesManager.Instance.AddEgg(1);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("BreedingZone"))
        {
            isInBreedingArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<IInteractable>() != null && collision.GetComponent<IInteractable>() == currentInteractable )
        {
            currentInteractable.StopInteration();
            currentInteractable = null;
        }
        else if (collision.CompareTag("BreedingZone"))
        {
            isInBreedingArea = false;
        }
    }
}
