using UnityEngine;

public class StatsTreeManager : MonoBehaviour
{
    [SerializeField] private GameObject unlockSlotPrefab;
    [SerializeField] private StatCategory focusedStatsCategory;

    private void Start()
    {
        foreach (var stat in StatsManager.Instance.GetAllRuntimeStatsFromCategory(focusedStatsCategory))
        {
            GameObject treeSlotObj = Instantiate(unlockSlotPrefab, transform);
            StatTreeSlot slot = treeSlotObj.GetComponent<StatTreeSlot>();
            slot.Initialize(stat);
        }
    }
}