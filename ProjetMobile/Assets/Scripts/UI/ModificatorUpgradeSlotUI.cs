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
    private Cap currentCap;
    private int currentRank;
    private bool isDragged;
    
    [Header("References")] 
    [SerializeField] private Image backImage;
    [SerializeField] private TextMeshProUGUI modificatorNameText;
    [SerializeField] private TextMeshProUGUI modificatorDescText;
    [SerializeField] private Button slotButton;
    [SerializeField] private RectTransform mainTr;
    [SerializeField] private ParticleSystem constantVFX;
    [SerializeField] private ParticleSystem explosionVFX;
    [SerializeField] private RectTransform raysMainRectTr;
    [SerializeField] private RectTransform[] raysRectTr;
    private GetNewModificatorUI mainScript;

    
    private void Start()
    {
        mainScript = GetComponentInParent<GetNewModificatorUI>();
    }


    public void SetCurrentData(ModificatorData data, int rank)
    {
        currentRank = rank;
        
        isDragged = false;
        modificatorNameText.text = data.modificatorName;
        modificatorDescText.text = data.modificatorDescription;
        backImage.color = HUDManager.Instance.ranksColors[rank];
        backImage.color *= new Color(1, 1, 1, 0);

        ParticleSystem.MainModule main = constantVFX.main;
        main.startColor = HUDManager.Instance.ranksColors[rank];
        main = explosionVFX.main;
        main.startColor = HUDManager.Instance.ranksColors[rank];

        currentCap = Instantiate(capPrefab, transform.position - transform.forward * 0.5f, transform.rotation);
        currentCap.SetData(data, rank);
        currentCap.ChangeWantedPos(transform.position - transform.forward * 0.5f);
        
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
        mainTr.UBounce(openAnimDuration * 0.7f, Vector3.one * 1.2f, 
            openAnimDuration * 0.3f, Vector3.one * 1f, CurveType.None, true);
        //backImage.UFadeImage(openAnimDuration * 0.7f, 0.6f, CurveType.None, true);
        backImage.rectTransform.UBounce(openAnimDuration * 0.7f, backImage.rectTransform.localScale * 1.1f, 
            openAnimDuration * 0.3f, backImage.rectTransform.localScale, CurveType.None, true);
        
        constantVFX.Play();
        
        currentCap.transform.rotation = transform.rotation * Quaternion.Euler(-90f, 0, 0f);
        currentCap.ActualiseCap();
        currentCap.DoRotationEntrance(openAnimDuration);
        
        yield return new WaitForSecondsRealtime(openAnimDuration * 0.4f);
        
        StartCoroutine(RaysAppearEffectCoroutine(0.4f));
        
        yield return new WaitForSecondsRealtime(openAnimDuration * 0.3f);
        
        backImage.UFadeImage(openAnimDuration * 0.3f, 0.4f, CurveType.None, true);
        explosionVFX.Play();
        
        yield return new WaitForSecondsRealtime(openAnimDuration * 0.3f);
        
        slotButton.enabled = true;
    }

    public IEnumerator DoCloseAnimCoroutine(bool instant)
    {
        slotButton.enabled = false;
        mainTr.UChangeScale(instant ? 0 : openAnimDuration * 0.9f, new Vector3(0, 0, 0), CurveType.EaseOutBack, true);

        if (!isDragged && !instant)
        {
            currentCap.transform.UChangeScale(0.5f, Vector3.one * 0f, CurveType.EaseOutBack, true);
            Destroy(currentCap.gameObject, 0.5f);
        }
        currentCap = null;
        
        constantVFX.Stop();
        
        if(!instant)
            StartCoroutine(RaysDisappearCoroutine(1f));
        
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
        isDragged = true;
        mainScript.StartDrag(currentCap);
    }

    public void StopDrag()
    {
        isDragged = false;
        currentCap.ChangeWantedPos(transform.position - transform.forward * 0.5f);
    }

    #endregion


    #region Rays

    private Coroutine rayRotationCoroutine;
    public IEnumerator RaysAppearEffectCoroutine(float duration)
    {
        rayRotationCoroutine = StartCoroutine(RaysRotationEffectCoroutine());

        for (int i = 0; i < raysRectTr.Length; i++)
        {
            raysRectTr[i].UBounce(duration * 0.8f, new Vector3(0.25f, 0.25f, 0.25f), duration * 0.2f, new Vector3(0.2f, 0.2f, 0.2f), 
                CurveType.EaseInOutSin, true);
            
            raysRectTr[i].GetComponent<Image>().color = HUDManager.Instance.ranksColors[currentRank];
            
            yield return new WaitForSecondsRealtime(duration / 8);
        }
    }

    private IEnumerator RaysRotationEffectCoroutine()
    {
        while (true)
        {
            raysMainRectTr.rotation *= Quaternion.Euler(new Vector3(0, 0, Time.unscaledDeltaTime * 6f));
            
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator RaysDisappearCoroutine(float duration)
    {
        for (int i = 0; i < raysRectTr.Length; i++)
        {
            raysRectTr[i].UChangeScale(duration, Vector3.zero, CurveType.EaseOutSin, true);
            
            yield return new WaitForSecondsRealtime(duration / 8);
        }
        
        StopCoroutine(rayRotationCoroutine);
    }

    #endregion
}
