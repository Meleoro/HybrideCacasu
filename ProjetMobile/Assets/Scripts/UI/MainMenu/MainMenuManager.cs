using System;
using UnityEngine;
using Utilities;

public class MainMenuManager : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private RectTransform sagaMapParentRectTr;
    [SerializeField] private RectTransform upgradesParentRectTr;
    [SerializeField] private RectTransform sagaMapIconRectTr;
    [SerializeField] private RectTransform upgradeIconRectTr;
    [SerializeField] private RectTransform selectedPanelIcon;


    private void Start()
    {
        Time.timeScale = 1;
    }

    public void OpenSagaMap()
    {
        sagaMapParentRectTr.UChangeLocalPosition(0.5f, new Vector3(0, 0, 0), CurveType.EaseOutSin);
        upgradesParentRectTr.UChangeLocalPosition(0.5f, new Vector3(500, 0, 0), CurveType.EaseOutSin);
        
        selectedPanelIcon.UChangePosition(0.5f, new Vector3(sagaMapIconRectTr.position.x, selectedPanelIcon.position.y, 
            selectedPanelIcon.position.z), CurveType.EaseOutCubic);
    }

    public void OpenUpgrades()
    {
        sagaMapParentRectTr.UChangeLocalPosition(0.5f, new Vector3(-500, 0, 0), CurveType.EaseOutSin);
        upgradesParentRectTr.UChangeLocalPosition(0.5f, new Vector3(0, 0, 0), CurveType.EaseOutSin);
        
        selectedPanelIcon.UChangePosition(0.5f, new Vector3(upgradeIconRectTr.position.x, selectedPanelIcon.position.y,
            selectedPanelIcon.position.z), CurveType.EaseOutCubic);
    }
}
