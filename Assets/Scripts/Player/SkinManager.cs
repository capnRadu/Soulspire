using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RuntimeSkinData
{
    public SkinDefinition skinDefinition;
    public bool isUnlocked;
}

public class SkinManager : MonoBehaviour
{
    public List<SkinDefinition> availableSkins;
    private Dictionary<string, RuntimeSkinData> runtimeSkins = new Dictionary<string, RuntimeSkinData>();

    [SerializeField] private Material skinMaterial;
    public RuntimeSkinData EquippedSkin { get; private set; }

    private void Awake()
    {
        foreach (var skin in availableSkins)
        {
            var runtimeSkin = new RuntimeSkinData
            {
                skinDefinition = skin,
                isUnlocked = skin.isDefault
            };

            runtimeSkins[skin.skinName] = runtimeSkin;

            if (runtimeSkin.isUnlocked && EquippedSkin == null)
            {
                EquipSkin(runtimeSkin.skinDefinition);
                EquippedSkin = runtimeSkin;
            }
        }
    }

    public List<RuntimeSkinData> GetAllSkins()
    {
        return new List<RuntimeSkinData>(runtimeSkins.Values);
    }

    public void EquipSkin(SkinDefinition skinDef)
    {
        if (runtimeSkins.TryGetValue(skinDef.skinName, out RuntimeSkinData data))
        {
            EquippedSkin = data;
            skinMaterial.SetColor("_BaseColor", skinDef.color);
        }
    }

    public void PreviewSkin(SkinDefinition skinDef)
    {
        skinMaterial.SetColor("_BaseColor", skinDef.color);
    }
}