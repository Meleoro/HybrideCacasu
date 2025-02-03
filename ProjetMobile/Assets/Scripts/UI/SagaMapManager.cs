using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SagaMapManager : MonoBehaviour
{
    [Header("Parameters")] 
    [SerializeField] private LevelData[] levels;
    [SerializeField] private float offsetBetweenLevelsY;
    [SerializeField] private SagaMapLevel levelPrefab;

    [Header("Public Infos")] 
    public bool isDoingTransition;

    [Header("References")] 
    [SerializeField] private LevelTransition transitionScript;
    [SerializeField] private RectTransform levelParentTr;
    [SerializeField] private RectTransform detailsParentTr;
    [SerializeField] private TextMeshProUGUI levelName;
    [SerializeField] private TextMeshProUGUI objective1Text;
    [SerializeField] private TextMeshProUGUI objective2Text;
    [SerializeField] private TextMeshProUGUI objective3Text;
    [SerializeField] private Image objective1Image;
    [SerializeField] private Image objective2Image;
    [SerializeField] private Image objective3Image;
    
    
    private void Awake()
    {
        GenerateSagaMap();
    }

    private void GenerateSagaMap()
    {
        Vector2 currentPos = new Vector2(0, offsetBetweenLevelsY);
        
        levelParentTr.sizeDelta = new Vector2(500, offsetBetweenLevelsY + levels.Length * offsetBetweenLevelsY);

        for (int i = 0; i < levels.Length; i++)
        {
            SagaMapLevel newLevel = Instantiate(levelPrefab, levelParentTr);
            newLevel.SetupLevel(levels[i], i, this);
            
            RectTransform levelRectTr = newLevel.GetComponent<RectTransform>();
            levelRectTr.localPosition = new Vector3(currentPos.x, currentPos.y);
            
            currentPos += new Vector2(0, offsetBetweenLevelsY);
        }
    }


    public void OpenDetails(LevelData data)
    {
        detailsParentTr.gameObject.SetActive(true);
        levelName.text = data.levelName;
    }

    public void CloseDetails()
    {
        detailsParentTr.gameObject.SetActive(false);
    }

    public void StartLevel()
    {
        if (isDoingTransition) return;
        
        isDoingTransition = true;
        StartCoroutine(transitionScript.EnterTransitionCoroutine("LevelScene"));
    }
}
