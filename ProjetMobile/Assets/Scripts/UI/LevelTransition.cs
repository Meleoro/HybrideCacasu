using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class LevelTransition : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private RectTransform upImage;
    [SerializeField] private RectTransform downImage;


    public IEnumerator EnterTransitionCoroutine(string levelName)
    {
        upImage.UChangeLocalPosition(0.75f, new Vector3(0, 500, 0), CurveType.EaseInSin, true);
        downImage.UChangeLocalPosition(0.75f, new Vector3(0, -500, 0), CurveType.EaseInSin, true);
        
        yield return new WaitForSecondsRealtime(1f);

        SceneManager.LoadScene(levelName);
    }
    
    public void ExitTransitionCoroutine()
    {
        upImage.localPosition = new Vector3(0, 500, 0);
        downImage.localPosition = new Vector3(0, -500, 0);
        
        upImage.UChangeLocalPosition(1f, new Vector3(0, 1200, 0), CurveType.EaseInSin, true);
        downImage.UChangeLocalPosition(1f, new Vector3(0, -1200, 0), CurveType.EaseInSin, true);
    }
}
