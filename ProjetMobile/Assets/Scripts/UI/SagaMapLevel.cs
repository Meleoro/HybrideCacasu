using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SagaMapLevel : MonoBehaviour
{
    [Header("Private Infos")] 
    private LevelData data;
    private int currentIndex;

    [Header("References")] 
    [SerializeField] private TextMeshProUGUI levelIndexText;
    [SerializeField] private Image[] stars;
    [SerializeField] private SagaMapManager mainScript;
    
    
    public void SetupLevel(LevelData data, int currentIndex, SagaMapManager mainScript)
    {
        this.data = data;
        this.currentIndex = currentIndex;

        this.mainScript = mainScript;
        
        levelIndexText.text = (1 + currentIndex).ToString();
    }


    public void ClickButton()
    {
        DontDestroyOnLoadObject.Instance.levelData = data;
        mainScript.OpenDetails(data);
    }
}
