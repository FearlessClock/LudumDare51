using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerHandler playerPrefab;
    [SerializeField] private BoolVariable[] isPlayersStunned;

    private void Start()
    {
        for (int i = 0; i < MultiManager.Instance.PlayersNumber; i++)
        {
            PlayerHandler player = Instantiate<PlayerHandler>(playerPrefab, transform.position, Quaternion.identity);
            player.PlayerInit(MultiManager.Instance.GetPlayerID(i), isPlayersStunned[i]);
        }
    }
}
