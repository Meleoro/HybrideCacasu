using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class EndScreen : MonoBehaviour
{
    [Header("Parameters")] 
    [SerializeField] private Sprite fullStarSprite;
    
    [Header("References")] 
    [SerializeField] private TextMeshProUGUI mainText;
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private Image[] starsImages;
    [SerializeField] private TextMeshProUGUI[] objectivesText;
    [SerializeField] private LevelTransition levelTransitionScript;


    public void Restart()
    {
        StartCoroutine(levelTransitionScript.EnterTransitionCoroutine("LevelScene"));
    }

    public void MainMenu()
    {
        StartCoroutine(levelTransitionScript.EnterTransitionCoroutine("MainMenu"));
    }
    
    
    public IEnumerator DisplayLoseCoroutine()
    {
        AppearEffect(1.5f);

        mainText.text = "GAME OVER";
        for(int i = 0; i < objectivesText.Length; i++)
        {
            objectivesText[i].text = "SURVIVE " + GameManager.Instance.levelData.durationObjectives[i] + " SECONDS";
        }
        
        yield return new WaitForSecondsRealtime(1f);
        
        int score = (int)(GameManager.Instance.levelData.softCurrencyWon * GameManager.Instance.currentTimer / GameManager.Instance.levelData.levelDuration);
        
        for (int i = 0; i < starsImages.Length; i++)
        {
            if (GameManager.Instance.levelData.durationObjectives[i] > GameManager.Instance.currentTimer) continue;

            starsImages[i].sprite = fullStarSprite;
            starsImages[i].rectTransform.UBounce(0.2f, starsImages[i].rectTransform.localScale * 1.4f, 
                0.4f, starsImages[i].rectTransform.localScale);
            
            yield return new WaitForSecondsRealtime(0.5f);
        }
        
        StartCoroutine(CurrencyFeelCoroutine(score, 2f));
    }

    public IEnumerator DisplayWinCoroutine()
    {
        AppearEffect(1.5f);
        
        for(int i = 0; i < objectivesText.Length; i++)
        {
            objectivesText[i].text = "SURVIVE " + GameManager.Instance.levelData.durationObjectives[i] + " SECONDS";
        }
        mainText.text = "YOU WON";
        
        yield return new WaitForSecondsRealtime(1f);
        
        int score = GameManager.Instance.levelData.softCurrencyWon;
        
        for (int i = 0; i < starsImages.Length; i++)
        {
            starsImages[i].sprite = fullStarSprite;
            starsImages[i].rectTransform.UBounce(0.2f, starsImages[i].rectTransform.localScale * 1.4f, 
                0.4f, starsImages[i].rectTransform.localScale, CurveType.None, true);
            
            yield return new WaitForSecondsRealtime(0.5f);
        }

        StartCoroutine(CurrencyFeelCoroutine(score, 2f));
    }


    private void AppearEffect(float duration)
    {
        transform.localScale = Vector3.one * 10f;
        
        transform.UChangeScale(duration, Vector3.one, CurveType.EaseOutBack, true);
    }
    
    private IEnumerator CurrencyFeelCoroutine(int wonCurrency, float duration)
    {
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;

            currencyText.text = "+ " + (int)(wonCurrency * (timer / duration));
            
            yield return new WaitForEndOfFrame();
        }
        
        currencyText.text = "+ " + wonCurrency;
    }
}
