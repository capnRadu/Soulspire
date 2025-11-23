using UnityEngine;

public class StatsMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject statSlotPrefab;
    [SerializeField] private bool isHub;
    [SerializeField] private StatCategory focusedStatsCategory;

    private void Start()
    {
        foreach (var stat in StatsManager.Instance.GetAllRuntimeStatsFromCategory(focusedStatsCategory))
        {
            GameObject statSlotObj = Instantiate(statSlotPrefab, transform);
            StatSlot statSlot = statSlotObj.GetComponent<StatSlot>();
            statSlot.Initialize(stat, isHub);
        }
    }
}