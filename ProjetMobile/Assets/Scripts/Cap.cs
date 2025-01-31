using System;
using UnityEngine;

public class Cap : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private Material[] capRankMaterials;
    [SerializeField] private ModificatorData capModificatorData;

    [Header("Private Infos")] 
    private Vector3 wantedPos;
    private Quaternion wantedRot;

    [Header("References")] 
    [SerializeField] private MeshRenderer meshRenderer;


    private void Start()
    {
        wantedRot = Quaternion.identity;
    }

    public void SetData(ModificatorData data, int rank)
    {
        capModificatorData = data;
        meshRenderer.materials[0] = capRankMaterials[rank];
    }
    
    
    public void ChangeWantedPos(Vector3 newPos)
    {
        wantedPos = newPos;
    }

    public void ChangeWantedRot(Quaternion newRot)
    {
        wantedRot = newRot;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, wantedPos, Time.unscaledDeltaTime * 8f);
        transform.rotation = Quaternion.Lerp(transform.rotation, wantedRot, Time.unscaledDeltaTime * 7.5f);
    }
}
