using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Player player;
    [SerializeField] private float attackRange = .2f;
    [SerializeField] private LayerMask enemyMask;
    private Vector3 direction;
    private Vector3 previousDirection;
    [SerializeField] private BoolVariable isStunned;

    public void InitPlayer()
    {
        player = GetComponent<PlayerHandler>().player;

    }

    void Update()
    {
        if (player != null && !isStunned.value)
        {
            direction = new Vector3(player.GetAxis("Horizontal"), player.GetAxis("Vertical"));

            if (direction.sqrMagnitude > .2f)
            {
                previousDirection = direction;
            }
            if (player.GetButton("Attack"))
            {
                RaycastHit2D hit = Physics2D.CircleCast(transform.position + previousDirection.normalized, attackRange, previousDirection, .01f, enemyMask);
                if (hit.collider != null)
                {
                    Debug.Log("attack");
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + previousDirection.normalized, attackRange);
    }
}
