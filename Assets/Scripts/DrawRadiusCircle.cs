using UnityEngine;

public class DrawRadiusCircle : MonoBehaviour
{
    private Player player;
    private float radius;
    [SerializeField] private int segments = 50;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        radius = player.DetectionRadius;
    }

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        SetupLineRenderer();
        DrawCircle();
    }

    private void SetupLineRenderer()
    {
        lineRenderer.useWorldSpace = false;
        lineRenderer.positionCount = segments + 1;
        lineRenderer.loop = true;
    }

    private void DrawCircle()
    {
        float angle = 0f;
        float angleStep = (2f * Mathf.PI) / segments;

        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            Vector3 position = new Vector3(x, 0.01f, z);

            lineRenderer.SetPosition(i, position);

            angle += angleStep;
        }
    }
}