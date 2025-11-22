using UnityEngine;

public class StatsMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject statSlotPrefab;
    [SerializeField] private bool isHub;
    [SerializeField] private StatCategory focusedStatsCategory;

    private void OnEnable()
    {
        foreach (var stat in StatsManager.Instance.GetAllRuntimeStatsFromCategory(focusedStatsCategory))
        {
            GameObject statSlotObj = Instantiate(statSlotPrefab, transform);
            StatSlot statSlot = statSlotObj.GetComponent<StatSlot>();
            statSlot.Initialize(stat, isHub);
        }
    }

    private void OnDisable()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}