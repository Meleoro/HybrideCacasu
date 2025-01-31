using System;
using System.Collections;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class GetNewModificatorUISlot : MonoBehaviour
{
    [Header("Parameters")] 
    [SerializeField] private float openAnimDuration;
    [SerializeField] private Cap capPrefab;

    [Header("Private Infos")] 
    private ModificatorData currentData;
    private int currentRank;
    private Cap currentCap;
    
    [Header("References")] 
    [SerializeField] private Image modificatorImage;
    [SerializeField] private Image backImage;
    [SerializeField] private TextMeshProUGUI modificatorNameText;
    [SerializeField] private TextMeshProUGUI modificatorDescText;
    [SerializeField] private Button slotButton;
    [SerializeField] private RectTransform mainTr;
    [SerializeField] private ParticleSystem[] vfxs;
    private GetNewModificatorUI mainScript;

    
    private void Start()
    {
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
        backImage.color = HUDManager.Instance.ranksColors[rank];
        backImage.color *= new Color(1, 1, 1, 0.2f);

        for (int i = 0; i < vfxs.Length; i++)
        {
            ParticleSystem.MainModule main = vfxs[i].main;
            main.startColor = HUDManager.Instance.ranksColors[rank];
        }

        currentCap = Instantiate(capPrefab, transform.position, transform.rotation);
        currentCap.SetData(data, rank);
        currentCap.ChangeWantedPos(transform.position);
        currentCap.ChangeWantedRot(transform.rotation * Quaternion.Euler(-90f, 0, 0f));
        
        ActualiseDescription(data, rank);
    }

    private void ActualiseDescription(ModificatorData data, int rank)
    {
        string baseText = data.modificatorDescription;
        string finalText = "";

        for (int i = 0; i < baseText.Length; i++)
        {
            if (baseText[i] == '{')
            {
                if(data.isPercentDisplay)
                    finalText += 100 * data.modificatorImpacts[rank];
                
                else
                    finalText += data.modificatorImpacts[rank];
                
                i += 2;
            }
            else
            {
                finalText += baseText[i];
            }
        }


        modificatorDescText.text = finalText;
    }
    


    #region Open / Close Animations

    public IEnumerator DoOpenAnimCoroutine()
    {
        mainTr.UBounce(openAnimDuration * 0.8f, Vector3.one * 1.4f, openAnimDuration * 0.2f, Vector3.one * 1f, CurveType.None, true);
        mainTr.UChangeLocalRotation(openAnimDuration, Quaternion.Euler(0, 359, 0), CurveType.None, true);
        
        for (int i = 0; i < vfxs.Length; i++)
        {
            vfxs[i].Play();
        }
        
        yield return new WaitForSeconds(openAnimDuration);

        slotButton.enabled = true;
    }

    public IEnumerator DoCloseAnimCoroutine(bool instant)
    {
        slotButton.enabled = false;
        mainTr.UChangeScale(instant ? 0 : openAnimDuration * 0.9f, new Vector3(0, 0, 0), CurveType.EaseOutBack, true);
        mainTr.UChangeLocalRotation(instant ? 0 : openAnimDuration, Quaternion.Euler(0, 0, 0), CurveType.None, true);
        
        for (int i = 0; i < vfxs.Length; i++)
        {
            vfxs[i].Stop();
        }
        
        yield return new WaitForSeconds(instant ? 0.01f : openAnimDuration);
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
