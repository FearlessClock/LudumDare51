using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Interation();
    void StopInteration();

    int GetPriority();
    void ShowOutline(bool show);
}
