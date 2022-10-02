using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MenuLinker : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button optionButton;
    [SerializeField] Button quitButton;

    [SerializeField] private GameObject mainScreen;
    [SerializeField] private GameObject optionScreen;

    [SerializeField] private AudioMixer audioMixer;

    private bool isOption;

    void Start()
    {
        startButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(GameManager.Instance.CallStartPlay);
        quitButton.onClick.AddListener(GameManager.Instance.CallQuit);
    }
    
    public void OptionsChangeScreen()
    {
        if (isOption)
        {
            optionScreen.SetActive(false);
            mainScreen.SetActive(true);
        }
        else
        {
            optionScreen.SetActive(true);
            mainScreen.SetActive(false);
        }
        
        isOption = !isOption;
    }

    public void changeOSTSlider(float lefloat)
    {
        audioMixer.SetFloat("OST", Mathf.Log10(lefloat) * 20);
    }
    
    public void changeSFXSlider(float lefloat)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(lefloat) * 20);
    }
    
}
