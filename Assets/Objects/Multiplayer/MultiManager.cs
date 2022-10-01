using Rewired;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MultiManager : MonoBehaviour
{
    private int playersConnected = 0;
    private Dictionary<int, int> PlayerToID; // Key = Player Number, Value = Rewired.Player Id
    private Dictionary<int, int> ControllerToPlayer; // Key = Rewired.Controller Id, Value = player Number

    private void Awake()
    {
        PlayerToID = new Dictionary<int, int>();
        ControllerToPlayer = new Dictionary<int, int>();

        DontDestroyOnLoad(this);
        ReInput.ControllerConnectedEvent += OnControllerConnect;
        ReInput.ControllerPreDisconnectEvent += OnControllerDisconnect;

        for (int i = 0; i < ReInput.controllers.controllerCount; i++)
            AddPlayer(i);
    }

    private void Update()
    {

    }

    private void OnDisable()
    {
        ReInput.ControllerConnectedEvent -= OnControllerConnect;
        ReInput.ControllerPreDisconnectEvent -= OnControllerDisconnect;
    }

    private void OnControllerConnect(ControllerStatusChangedEventArgs status)
    {
        AddPlayer(status.controllerId);
    }

    private void AddPlayer(int controllerId)
    {
        if (playersConnected > 3)
            return;

        (int n, int id) = GetPlayerIDFromControllerID(controllerId);
        if (id < 0)
            return;

        ControllerToPlayer.Add(controllerId, n);
        PlayerToID.Add(n, id);
        playersConnected++;

        Debug.Log($"Connected Player {n} (ID: {id}, CTRL: {controllerId})");
    }

    private void OnControllerDisconnect(ControllerStatusChangedEventArgs status)
    {
        int n = GetPlayerNumber(status.controllerId);
        if (n < 0)
            return;

        Debug.Log($"Disconnecting Player {n} (ID: {PlayerToID[n]}, CTRL: {status.controllerId}) . . .");
        ControllerToPlayer.Remove(status.controllerId);
        PlayerToID.Remove(n);
        playersConnected--;
    }

    private int HasControllerID(Player player)
    {
        for (int i = 0; i < ReInput.controllers.controllerCount; i++)
        {
            Controller controller = ReInput.controllers.GetController(ControllerType.Joystick, i);
            if (controller != null && controller == player.controllers.GetController(ControllerType.Joystick, i))
                return i;
        }
        return -1;
    }

    private (int, int) GetPlayerIDFromControllerID(int id)
    {
        for (int i = 0; i < ReInput.players.playerCount; i++)
        {
            Player p = ReInput.players.GetPlayer(i);
            if (HasControllerID(p) == id)
                return (i, id);
        }
        return (-1, -1);
    }

    private int GetPlayerNumber(int controllerNumber) => ControllerToPlayer.ContainsKey(controllerNumber) ? ControllerToPlayer[controllerNumber] : -1;

    public int GetPlayerID(int playerNumber) => PlayerToID.ContainsKey(playerNumber) ? PlayerToID[playerNumber] : -1;
}

