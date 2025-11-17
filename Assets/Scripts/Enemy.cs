using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform tower;

    private float speed = 10f;
    private float minimumDistanceToTower = 3f;
    private float yPos;

    private void Awake()
    {
        tower = GameObject.FindGameObjectWithTag("Tower").transform;
        yPos = transform.position.y;
    }

    private void Update()
    {
        if (tower != null)
        {
            if (Vector3.Distance(transform.position, tower.position) > minimumDistanceToTower)
            {
                Vector3 direction = (tower.position - transform.position).normalized;
                transform.position += speed * Time.deltaTime * direction;
                transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
            }
        }
    }
}