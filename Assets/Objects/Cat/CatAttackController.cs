using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAttackController : MonoBehaviour
{
    [SerializeField] private float enemySeeRange = 10;
    [SerializeField] private float hitDistance = 10;
    [SerializeField] private float hitCooldownTime = 10;
    private CatGoalHandler catGoalHandler = null;
    private CatEquipment catEquipment = null;
    private float hitCooldownTimer = 0;
    private bool hasAttacked = false;
    private GameObject currentTarget = null;
    int checkWait = 300;

    private void Awake()
    {
        catEquipment = GetComponent<CatEquipment>();
        catGoalHandler = GetComponent<CatGoalHandler>();
    }

    private void Update()
    {
        if(catEquipment && catEquipment.HasEquipment)
        {
            checkWait--;
            if (checkWait <= 0 && !currentTarget)
            {
                checkWait = 300;
                List<GameObject> closeFoxes = new List<GameObject>();
                for (int i = 0; i < FoxManager.Instance.foxes.Count; i++)
                {
                    if (FoxManager.Instance.foxes[i] != null && (FoxManager.Instance.foxes[i].transform.position - this.transform.position).sqrMagnitude < enemySeeRange)
                    {
                        closeFoxes.Add(FoxManager.Instance.foxes[i]);
                    }
                }
                if (closeFoxes.Count > 0)
                {
                    currentTarget = closeFoxes[Random.Range(0, closeFoxes.Count)];
                    catGoalHandler.AddGoal(new FollowEnemyGoal(currentTarget.transform));
                }
            }
            else if (checkWait <= 0)
            {
                checkWait = 300;
            }
            if (currentTarget)
            {
                if (hasAttacked)
                {
                    hitCooldownTimer -= TimeManager.deltaTime;
                    if (hitCooldownTimer < 0)
                    {
                        hasAttacked = false;
                    }
                }

                if (currentTarget && (currentTarget.transform.position - this.transform.position).sqrMagnitude < hitDistance && !hasAttacked)
                {
                    hitCooldownTimer = hitCooldownTime;
                    hasAttacked = true;
                    if (currentTarget.GetComponent<HealthController>().Hit(1))
                    {
                        catGoalHandler.OnGoalDone();
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, enemySeeRange);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(this.transform.position, hitDistance);
    }
}
