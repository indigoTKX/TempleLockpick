using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "MaterialsLibrary", menuName = "MaterialsLibrary")]
public class MaterialsLibrary : ScriptableObject
{
    [SerializeField] private List<MaterialData> _materialDatas;

    public Color GetColorForMaterial(PinMaterial material)
    {
        var data = _materialDatas.FirstOrDefault(item => item.Material == material);
        if (data == null)
        {
            Debug.LogError($"Can't find material: {material.ToString()}");
            return Color.white;
        }
        
        return data.Color;
    }

    public Sprite GetLockpickForMaterial(PinMaterial material)
    {
        var data = _materialDatas.FirstOrDefault(item => item.Material == material);
        if (data == null)
        {
            Debug.LogError($"Can't find material: {material.ToString()}");
            return null;
        }
        
        return data.LockpickSprite;
    }
    
    public Material GetSpriteMaterial(PinMaterial material)
    {
        var data = _materialDatas.FirstOrDefault(item => item.Material == material);
        if (data == null)
        {
            Debug.LogError($"Can't find material: {material.ToString()}");
            return null;
        }
        
        return data.SpriteMaterial;
    }
    
    public GameObject GetUiLockpickPrefab(PinMaterial material)
    {
        var data = _materialDatas.FirstOrDefault(item => item.Material == material);
        if (data == null)
        {
            Debug.LogError($"Can't find material: {material.ToString()}");
            return null;
        }
        
        return data.UiLockpickPrefab;
    }

    [Serializable]
    private class MaterialData
    {
        public PinMaterial Material;
        public Color Color;
        public Sprite LockpickSprite;
        public Material SpriteMaterial;
        public GameObject UiLockpickPrefab;
    }
}
