using System;
using TMPro;
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
    [SerializeField] private TextMeshProUGUI softCurrencyText;


    private void Start()
    {
        Time.timeScale = 1;
        softCurrencyText.text = DontDestroyOnLoadObject.Instance.ownedSoftCurrency.ToString();
    }

    public void OpenSagaMap()
    {
        sagaMapParentRectTr.UChangeLocalPosition(0.25f, new Vector3(0, 0, 0), CurveType.EaseOutCubic);
        upgradesParentRectTr.UChangeLocalPosition(0.25f, new Vector3(500, 0, 0), CurveType.EaseOutCubic);
        
        selectedPanelIcon.UChangePosition(0.25f, new Vector3(sagaMapIconRectTr.position.x, selectedPanelIcon.position.y, 
            selectedPanelIcon.position.z), CurveType.EaseOutCubic);
    }

    public void OpenUpgrades()
    {
        sagaMapParentRectTr.UChangeLocalPosition(0.25f, new Vector3(-500, 0, 0), CurveType.EaseOutCubic);
        upgradesParentRectTr.UChangeLocalPosition(0.25f, new Vector3(0, 0, 0), CurveType.EaseOutCubic);
        
        selectedPanelIcon.UChangePosition(0.25f, new Vector3(upgradeIconRectTr.position.x, selectedPanelIcon.position.y,
            selectedPanelIcon.position.z), CurveType.EaseOutCubic);
    }
}
