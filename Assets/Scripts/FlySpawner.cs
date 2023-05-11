using UnityEngine;

public class FlySpawner : MonoBehaviour
{
    [SerializeField] private Vector3 area = new Vector3(20f, 2f, 20f);
    [SerializeField] private Fly flyPrefab;
    [SerializeField] private WaypointTracker flowerTracker;

    private void SpawnFly()
    {
        Vector3 position = RandomPointInBox();
        Fly fly = Instantiate(flyPrefab, position, Quaternion.identity);
        fly.Flowers = flowerTracker.Waypoints;
    }

    private Vector3 RandomPointInBox()
    {
        Vector3 center = transform.position;

        Vector3 point = new Vector3(
            Random.Range(center.x - area.x / 2, center.x + area.x / 2),
            Random.Range(center.y - area.y / 2, center.y + area.y / 2),
            Random.Range(center.z - area.z / 2, center.z + area.z / 2)
        );

        return point;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Spawn Fly"))
        {
            SpawnFly();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, area);
    }
}
