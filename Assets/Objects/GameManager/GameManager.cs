using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : PersistentSingleton<GameManager>
{
    private bool isPaused = false;

    public void GameStart()
    {

    }

    public void EndGame()
    {

    }

    public void Pause(bool on)
    {
        TimeManager.SetTimeScale(on ? .1f : 1f);
    }

    public void VictoryScreen() { }

    public void BackToMenu() => LoadScene("MainMenu");

    public void LoadScene(string name) => SceneManager.LoadScene(name);
}
