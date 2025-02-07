using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Sprite fullStarSprite;
    [SerializeField] private Sprite emptyStarSprite;

    [Header("Public Infos")] 
    public bool isDoingTransition;

    [Header("Private Infos")] 
    private List<SagaMapLevel> levelScripts;

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
    
    
    private void Start()
    {
        StartCoroutine(DoOnceCoroutine());
    }

    private IEnumerator DoOnceCoroutine()
    {
        yield return null;
        
        GenerateSagaMap();
        DontDestroyOnLoadObject.Instance.OnSaveLoad += ActualiseLevels;
    }

    private void GenerateSagaMap()
    {
        Vector2 currentPos = new Vector2(0, offsetBetweenLevelsY);
        levelScripts = new();
        levelParentTr.sizeDelta = new Vector2(500, offsetBetweenLevelsY + levels.Length * offsetBetweenLevelsY);

        for (int i = 0; i < levels.Length; i++)
        {
            SagaMapLevel newLevel = Instantiate(levelPrefab, levelParentTr);
            newLevel.SetupLevel(levels[i], i, this);
            levelScripts.Add(newLevel);
            
            RectTransform levelRectTr = newLevel.GetComponent<RectTransform>();
            levelRectTr.localPosition = new Vector3(currentPos.x, currentPos.y);
            
            currentPos += new Vector2(0, offsetBetweenLevelsY);
        }
    }


    public void ActualiseLevels()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            levelScripts[i].SetupLevel(levels[i], i, this);
        }
    }

    public void OpenDetails(LevelData data, int levelIndex)
    {
        detailsParentTr.gameObject.SetActive(true);
        levelName.text = data.levelName;

        objective1Text.text = "SURVIVE " + data.durationObjectives[0] + " SECONDS";
        objective2Text.text = "SURVIVE " + data.durationObjectives[1] + " SECONDS";
        objective3Text.text = "SURVIVE " + data.durationObjectives[2] + " SECONDS";

        objective1Image.sprite = DontDestroyOnLoadObject.Instance.wonObjectives[levelIndex * 3] ? fullStarSprite : emptyStarSprite;
        objective2Image.sprite = DontDestroyOnLoadObject.Instance.wonObjectives[1 + levelIndex * 3] ? fullStarSprite : emptyStarSprite;
        objective3Image.sprite = DontDestroyOnLoadObject.Instance.wonObjectives[2 + levelIndex * 3] ? fullStarSprite : emptyStarSprite;
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
