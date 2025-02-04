using System;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private LevelTransition transitionScript;
    [SerializeField] private RectTransform mainTrPause;


    private void Start()
    {
        mainTrPause.gameObject.SetActive(false);
    }

    
    public void StartPause()
    {
        mainTrPause.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void Return()
    {
        mainTrPause.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    
    public void Restart()
    {
        StartCoroutine(transitionScript.EnterTransitionCoroutine("LevelScene"));
    }
    
    public void BackToMainMenu()
    {
        StartCoroutine(transitionScript.EnterTransitionCoroutine("MainMenu"));
    }
}
