using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressUI : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Image fillableImage;
    [SerializeField] private Image waveMarkerPrefab;
    


    public void GenerateWavesMarkers(LevelData data)
    {
        for (int i = 0; i < data.waves.Length; i++)
        {
            RectTransform newTr = Instantiate(waveMarkerPrefab, fillableImage.rectTransform).rectTransform;
            newTr.localPosition = new Vector3(0, -15, 0);

            float XRatio = data.waves[i].waveWaitDuration / data.levelDuration;
            
            newTr.localPosition = new Vector3(Mathf.Lerp(-fillableImage.rectTransform.rect.width * 0.5f, fillableImage.rectTransform.rect.width * 0.5f, XRatio), 
                -15, 0);
        }
    }
    
    
    public void ActualiseFillImage(float fillRatio)
    {
        fillableImage.fillAmount = fillRatio;
    }
}
