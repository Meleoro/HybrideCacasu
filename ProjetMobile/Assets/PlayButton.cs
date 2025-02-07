using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public LevelTransition transitionScript;

    public void DoTransition()
    {
        StartCoroutine(transitionScript.EnterTransitionCoroutine("MainMenu"));
    }
}
