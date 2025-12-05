using System.Collections.Generic;
using UnityEngine;

public class SkinsMenuManager : MonoBehaviour
{
    private SkinManager skinManager;
    [SerializeField] private GameObject skinSlotPrefab;
    
    private List<SkinSlot> spawnedSlots = new List<SkinSlot>();
    public RuntimeSkinData CurrentPreview { get; private set; }

    private void Awake()
    {
        skinManager = FindFirstObjectByType<SkinManager>();
    }

    private void OnEnable()
    {
        if (skinManager != null && skinManager.EquippedSkin != null)
        {
            CurrentPreview = skinManager.EquippedSkin;
            RefreshSlots();
        }
    }

    private void OnDisable()
    {
        if (skinManager != null && skinManager.EquippedSkin != null)
        {
            skinManager.EquipSkin(skinManager.EquippedSkin.skinDefinition);
        }
    }

    private void Start()
    {
        foreach (var skin in skinManager.GetAllSkins())
        {
            GameObject slot = Instantiate(skinSlotPrefab, transform);
            SkinSlot skinSlot = slot.GetComponent<SkinSlot>();
            skinSlot.Initialize(skin, skinManager, this);
            spawnedSlots.Add(skinSlot);
        }
    }

    public void EquipSkin(RuntimeSkinData skinData)
    {
        skinManager.EquipSkin(skinData.skinDefinition);

        CurrentPreview = skinData;
        RefreshSlots();
    }

    public void PreviewSkin(RuntimeSkinData skinData)
    {
        skinManager.PreviewSkin(skinData.skinDefinition);

        CurrentPreview = skinData;
        RefreshSlots();
    }

    public void RefreshSlots()
    {
        foreach (var slot in spawnedSlots)
        {
            slot.UpdateUI();
        }
    }
}