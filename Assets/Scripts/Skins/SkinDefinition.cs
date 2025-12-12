using UnityEngine;

[CreateAssetMenu(fileName = "New Skin", menuName = "Skins/Skin Definition")]
public class SkinDefinition : ScriptableObject
{
    public string skinName;
    public Sprite icon;
    public Color color;
    public int cost;
    public bool isDefault;
}