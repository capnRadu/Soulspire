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
        string lastEquippedName = PlayerPrefs.GetString("Skin_Equipped", "");

        foreach (var skin in availableSkins)
        {
            bool savedUnlock = PlayerPrefs.GetInt($"Skin_{skin.skinName}_Unlocked", 0) == 1;
            bool isUnlocked = skin.isDefault || savedUnlock;

            var runtimeSkin = new RuntimeSkinData
            {
                skinDefinition = skin,
                isUnlocked = isUnlocked
            };

            runtimeSkins[skin.skinName] = runtimeSkin;

            if (isUnlocked && skin.skinName == lastEquippedName)
            {
                EquipSkinInternal(runtimeSkin);
            }
            else if (EquippedSkin == null && skin.isDefault)
            {
                EquipSkinInternal(runtimeSkin);
            }
        }
    }

    public List<RuntimeSkinData> GetAllSkins()
    {
        return new List<RuntimeSkinData>(runtimeSkins.Values);
    }

    private void EquipSkinInternal(RuntimeSkinData data)
    {
        EquippedSkin = data;
        skinMaterial.SetColor("_BaseColor", data.skinDefinition.color);
    }

    public void EquipSkin(SkinDefinition skinDef)
    {
        if (runtimeSkins.TryGetValue(skinDef.skinName, out RuntimeSkinData data))
        {
            if (data.isUnlocked)
            {
                EquipSkinInternal(data);

                PlayerPrefs.SetString("Skin_Equipped", skinDef.skinName);
                PlayerPrefs.Save();
            }
        }
    }

    public void UnlockSkin(RuntimeSkinData data)
    {
        if (!data.isUnlocked)
        {
            data.isUnlocked = true;

            PlayerPrefs.SetInt($"Skin_{data.skinDefinition.skinName}_Unlocked", 1);
            PlayerPrefs.Save();
        }
    }

    public void PreviewSkin(SkinDefinition skinDef)
    {
        skinMaterial.SetColor("_BaseColor", skinDef.color);
    }
}