using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class GetNewModificatorUISlot : MonoBehaviour
{
    [Header("Parameters")] 
    [SerializeField] private float openAnimDuration;

    [Header("Private Infos")] 
    private ModificatorData currentData;
    private int currentRank;

    [Header("References")] 
    [SerializeField] private Image modificatorImage;
    [SerializeField] private TextMeshProUGUI modificatorNameText;
    [SerializeField] private TextMeshProUGUI modificatorDescText;
    [SerializeField] private Button slotButton;
    private RectTransform mainTr;
    private GetNewModificatorUI mainScript;

    private void Start()
    {
        mainTr = GetComponent<RectTransform>();
        mainScript = GetComponentInParent<GetNewModificatorUI>();
    }


    public void SetCurrentData(ModificatorData data, int rank)
    {
        currentData = data;

        modificatorNameText.text = data.modificatorName;
        modificatorDescText.text = data.modificatorDescription;
        modificatorImage.sprite = data.modificatorSprite;

        currentRank = rank;
        modificatorImage.color = HUDManager.Instance.ranksColors[rank];
    }


    #region Open / Close Animations

    public IEnumerator DoOpenAnimCoroutine()
    {
        mainTr.UChangeScale(openAnimDuration * 0.9f, new Vector3(1, 1, 1));
        mainTr.UChangeLocalRotation(openAnimDuration, Quaternion.Euler(0, 359, 0));
        
        yield return new WaitForSeconds(openAnimDuration);

        slotButton.enabled = true;
    }

    public IEnumerator DoCloseAnimCoroutine(bool instant)
    {
        slotButton.enabled = false;
        mainTr.UChangeScale(instant ? 0 : openAnimDuration * 0.9f, new Vector3(0, 0, 0));
        mainTr.UChangeLocalRotation(instant ? 0 : openAnimDuration, Quaternion.Euler(0, 0, 0));
        
        yield return new WaitForSeconds(instant ? 0 : openAnimDuration);
    }

    #endregion


    #region Player Inputs

    public void OverlaySlot()
    {
        mainTr.UChangeScale(0.1f, new Vector3(1.1f, 1.1f, 1.1f));
    }

    public void QuitOverlaySlot()
    {
        mainTr.UChangeScale(0.1f, new Vector3(1f, 1f, 1f));
    }

    public void ClickSlot()
    {
        mainScript.CloseChoseUpgradeUI();
    }
    
    public void DragSlot()
    {
        mainScript.StartDrag(currentData, currentRank);
    }

    #endregion
}
