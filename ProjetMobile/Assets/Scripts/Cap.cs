using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cap : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private Material[] capRankMaterials;

    [Header("Public Infos")] 
    public ModificatorData capModificatorData;
    public int capRank;
    
    [Header("Private Infos")] 
    private Vector3 wantedPos;
    private Quaternion wantedRot;

    [Header("References")] 
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private SpriteRenderer iconSpriteRenderer;


    private void Start()
    {
        wantedRot = Quaternion.identity;
    }

    public void SetData(ModificatorData data, int rank)
    {
        meshRenderer.materials[0] = capRankMaterials[rank];
        iconSpriteRenderer.sprite = data.modificatorSprite;
        
        capModificatorData = data;
        capRank = rank;
    }

    public void ActualiseCap()
    {
        Material[] materials = meshRenderer.materials;
        materials[0] = capRankMaterials[capRank];
        meshRenderer.SetMaterials(materials.ToList());
    }
    
    
    public void ChangeWantedPos(Vector3 newPos)
    {
        wantedPos = newPos;
    }

    public void ChangeWantedRot(Quaternion newRot)
    {
        wantedRot = newRot;
    }

    public IEnumerator DoRotationEntranceCoroutine()
    {
        float timer = 0;

        while (timer < 0.5f)
        {
            timer += Time.unscaledDeltaTime;
            wantedRot.eulerAngles = new Vector3(wantedRot.eulerAngles.x, wantedRot.eulerAngles.y, Mathf.Lerp(0, 360*2, timer / 0.5f));
            transform.rotation = Quaternion.Lerp(transform.rotation, wantedRot, Time.unscaledDeltaTime * 7.5f);
            
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, wantedPos, Time.unscaledDeltaTime * 8f);
        transform.rotation = Quaternion.Lerp(transform.rotation, wantedRot, Time.unscaledDeltaTime * 7.5f);
    }
}
