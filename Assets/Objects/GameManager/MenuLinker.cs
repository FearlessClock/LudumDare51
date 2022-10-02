using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuLinker : MonoBehaviour
{
    [SerializeField] Button startButton;

    void Start()
    {
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(GameManager.Instance.CallStartPlay);
    }

    public void loadscne()
    {
        GameManager.Instance.CallStartPlay();
    }
}
