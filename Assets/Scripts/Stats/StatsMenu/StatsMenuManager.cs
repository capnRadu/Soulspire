using UnityEngine;

public class StatsMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject statSlotPrefab;
    [SerializeField] private bool isHub;
    [SerializeField] private StatCategory focusedStatsCategory;

    private void OnEnable()
    {
        if (transform.childCount == 0)
        {
            foreach (var stat in StatsManager.Instance.GetAllRuntimeStatsFromCategory(focusedStatsCategory))
            {
                if (!stat.isPurchased)
                {
                    continue;
                }

                GameObject statSlotObj = Instantiate(statSlotPrefab, transform);
                StatSlot statSlot = statSlotObj.GetComponent<StatSlot>();
                statSlot.Initialize(stat, isHub);
            }
        }
    }

    private void OnDestroy()
    {
        if (isHub)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}