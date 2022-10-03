using HelperScripts.EventSystem;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : PersistentSingleton<GameManager>
{
    private bool isPaused = false;
    public bool IsPaused { get => isPaused; }
    private Coroutine loadingCoroutine = null;
    
    [SerializeField] GameObject fade;

    [SerializeField] private EventObjectScriptable win;
    [SerializeField] private EventObjectScriptable defeat;

    private void Start()
    {
        if(fade) fade.SetActive(true);
        win.AddListener(WinGame);
        defeat.AddListener(LoseGame);
    }

    public void GameStart()
    {

        TimeManager.SetTimeScale(1);
        // Init necessary Game Settings..
    }

    public void LoseGame(object obj)
    {
        TimeManager.SetTimeScale(0);
        FoxManager.Instance.DeleteAllFoxes();
        SceneManager.LoadSceneAsync("Defeat", LoadSceneMode.Additive);
    }

    public void WinGame(object obj)
    {
        Debug.Log("Victory");
        TimeManager.SetTimeScale(0);
        FoxManager.Instance.DeleteAllFoxes();
        SceneManager.LoadSceneAsync("Victory", LoadSceneMode.Additive);
    }

    public void Pause(bool on)
    {
        isPaused = on;
        TimeManager.SetTimeScale(on ? .1f : 1f);
    }

    public void VictoryScreen() { }

    // Button Functions
    public void CallStartPlay() => LoadScene("GameScene", GameStart);
    public void CallQuit() => QuitGame();
    public void BackToMenu() => LoadScene("MainMenu");

    // Load
    public void LoadScene(string name) => SceneManager.LoadScene(name);

    /// <summary>
    /// Waits for scene loading, then calls Action
    /// </summary>
    public void LoadScene(string name, Action then)
    {
        if(loadingCoroutine != null)
            StopCoroutine(loadingCoroutine);
        loadingCoroutine = StartCoroutine(AfterLoading(name, then));
    }
    
    private IEnumerator AfterLoading(string name, Action then)
    {
        LoadScene(name);

        while (SceneManager.GetActiveScene().name != name)
            yield return new WaitForEndOfFrame();

        then?.Invoke();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
