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
    [SerializeField] private Image[] starsImages;



    public IEnumerator DisplayLoseCoroutine()
    {
        float score = GameManager.Instance.levelData.softCurrencyWon * GameManager.Instance.currentTimer / GameManager.Instance.levelData.levelDuration;
        mainText.text = "GAME OVER";
        
        for (int i = 0; i < starsImages.Length; i++)
        {
            if (GameManager.Instance.levelData.durationObjectives[i] > GameManager.Instance.currentTimer) continue;

            starsImages[i].sprite = fullStarSprite;
            starsImages[i].rectTransform.UBounce(0.2f, starsImages[i].rectTransform.localScale * 1.4f, 
                0.4f, starsImages[i].rectTransform.localScale);
            
            yield return new WaitForSeconds(0.5f);
        }
        
    }

    public IEnumerator DisplayWinCoroutine()
    {
        float score = GameManager.Instance.levelData.softCurrencyWon;
        mainText.text = "YOU WON";
        
        for (int i = 0; i < starsImages.Length; i++)
        {
            starsImages[i].sprite = fullStarSprite;
            starsImages[i].rectTransform.UBounce(0.2f, starsImages[i].rectTransform.localScale * 1.4f, 
                0.4f, starsImages[i].rectTransform.localScale);
            
            yield return new WaitForSeconds(0.5f);
        }
        
    }
}
