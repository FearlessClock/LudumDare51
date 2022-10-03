using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : Singleton<PauseManager>
{
    [SerializeField] private GameObject pauseMenu;
    
    public void setPause()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void backToGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void quitToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
