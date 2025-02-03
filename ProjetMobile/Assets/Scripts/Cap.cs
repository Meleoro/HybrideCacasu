using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;

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
    private bool disableRot;

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

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, wantedPos, Time.unscaledDeltaTime * 8f);
    }


    #region Feel Functions
    
    public void DoRotationEntrance(float duration)
    {
        transform.localScale = new Vector3(0, 0, 0);
        
        transform.UBounce(duration * 0.7f, Vector3.one * 1.4f, duration * 0.3f, Vector3.one, CurveType.EaseOutBack, true);
        transform.UChangeRotation(duration * 1f, transform.eulerAngles + new Vector3(0, 0, 720), CurveType.EaseOutCubic, true);
        disableRot = true;
    }
    
    public void FuseCaps()
    {
        transform.UChangeRotation(0.5f, transform.eulerAngles + new Vector3(720, 0, 0), CurveType.EaseOutCubic, true);
        transform.UBounce(0.15f, transform.localScale * 2f, 0.35f, transform.localScale, CurveType.None, true);
    }
    
    #endregion
}
