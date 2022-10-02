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

    [SerializeField] private Transform swordTransform;
    [SerializeField] private AnimationCurve swordSlice;
    [SerializeField] private bool swordFollowDirection;
    [SerializeField] private bool castFollowSword;
    private Coroutine swordRoutine = null;
    private Animator animator;

    private Vector3 attackPos;

    public void InitPlayer()
    {
        player = GetComponent<PlayerHandler>().player;
        isStunned = GetComponent<PlayerHandler>().isStunned;
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

            if (player.GetButton("Attack") || Input.GetKeyDown(KeyCode.K))
            {
                PlaySwordAnimation();
                if (!castFollowSword)
                    CastAttackSphere(previousDirection);
            }
        }
    }

    private void CastAttackSphere(Vector3 dir)
    {
        //
        attackPos = transform.position + dir.normalized;
        //
        RaycastHit2D hit = Physics2D.CircleCast(transform.position + dir.normalized, attackRange, dir, .01f, enemyMask);
        if (hit.collider != null)
        {
            hit.collider.GetComponent<HealthController>().Hit(1);
        }
    }

    private void PlaySwordAnimation()
    {
        if (swordRoutine != null)
            return;
        swordRoutine = StartCoroutine(SwordAnimation());
    }

    private IEnumerator SwordAnimation()
    {
        swordTransform.gameObject.SetActive(true);

        float r = Mathf.Sign(Random.Range(-1f, 1f));
        float offset = Vector2.SignedAngle(Vector2.left, previousDirection);

        float length = swordSlice[swordSlice.length - 1].time;
        float timer = 0;
        while (timer < length)
        {
            if (swordFollowDirection)
                offset = Vector2.SignedAngle(Vector2.left, previousDirection);

            float angle = offset + r * swordSlice.Evaluate(timer);

            if (castFollowSword)
                CastAttackSphere(Quaternion.Euler(0, 0, angle) * Vector2.left);

            swordTransform.rotation = Quaternion.Euler(0, 0, angle);
            yield return new WaitForEndOfFrame();
            timer += TimeManager.deltaTime;
        }

        swordRoutine = null;
        swordTransform.rotation = Quaternion.identity;
        swordTransform.gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        if(swordRoutine != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPos, attackRange);
        }
    }
}
