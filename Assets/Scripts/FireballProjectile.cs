using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    [SerializeField] private float duration = 1.0f;
    [SerializeField] private float arcHeight = 5.0f;
    [SerializeField] private AnimationCurve speedCurve = AnimationCurve.Linear(0, 0, 1, 1);

    private Transform target;
    private Vector3 startPos;
    private Vector3 controlPoint;
    private float journeyTimer = 0f;

    [SerializeField] private int damage = 5;

    public void Initialize(Transform enemyTarget)
    {
        target = enemyTarget;
        startPos = transform.position;
    }

    private void Update()
    {
        if (target == null)
        {
            Arrive();
            return;
        }

        ProjectileTrajectoryAndMovement();
    }

    private void ProjectileTrajectoryAndMovement()
    {
        Vector3 currentTargetPos = target.position;
        Vector3 midPoint = (startPos + currentTargetPos) / 2f;
        controlPoint = midPoint + 2f * arcHeight * Vector3.up;

        if (journeyTimer < duration)
        {
            journeyTimer += Time.deltaTime;

            float linearT = journeyTimer / duration;
            float t = speedCurve.Evaluate(linearT);

            transform.position = CalculateQuadraticBezier(t, startPos, controlPoint, currentTargetPos);
        }
        else
        {
            transform.position = currentTargetPos;
            Arrive();
        }
    }

    private Vector3 CalculateQuadraticBezier(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 position = (uu * p0) + (2 * u * t * p1) + (tt * p2);

        return position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.TryGetComponent<Health>(out Health healthComponent);

        if (healthComponent != null)
        {
            healthComponent.TakeDamage(damage);
            Arrive();
        }
    }

    private void Arrive()
    {
        Destroy(gameObject);
    }
}