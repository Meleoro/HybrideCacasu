using UnityEngine;
using UnityEngine.Rendering;
using Utilities;

public class CameraManager : GenericSingletonClass<CameraManager>
{
    [Header("Parameters")] 
    [SerializeField] private float hurtEffectDuration;
    
    [Header("References")] 
    public Camera _camera;
    [SerializeField] private Volume hurtVolume;

    public void DoHurtEffect()
    {
        hurtVolume.UFlashWeight(hurtEffectDuration * 0.3f, 1, hurtEffectDuration * 0.7f, 0, true);
    }
}
