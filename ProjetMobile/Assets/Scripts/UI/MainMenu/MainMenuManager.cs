using System;
using UnityEngine;
using Utilities;

public class MainMenuManager : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private RectTransform sagaMapParentRectTr;
    [SerializeField] private RectTransform upgradesParentRectTr;


    private void Start()
    {
        Time.timeScale = 1;
    }

    public void OpenSagaMap()
    {
        sagaMapParentRectTr.UChangeLocalPosition(0.5f, new Vector3(0, 0, 0), CurveType.EaseOutSin);
        upgradesParentRectTr.UChangeLocalPosition(0.5f, new Vector3(500, 0, 0), CurveType.EaseOutSin);
    }

    public void OpenUpgrades()
    {
        sagaMapParentRectTr.UChangeLocalPosition(0.5f, new Vector3(-500, 0, 0), CurveType.EaseOutSin);
        upgradesParentRectTr.UChangeLocalPosition(0.5f, new Vector3(0, 0, 0), CurveType.EaseOutSin);
    }
}
