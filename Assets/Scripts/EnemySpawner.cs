using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Edible enemyPrefab;
    [SerializeField] private Vector3 area = new Vector3(20f, 2f, 20f);
    [SerializeField] private float maxSpawnTime;

    public Edible LastSpawnedEnemy => lastSpawnedEnemy;
    public EatenEvent OnSpawn => onSpawn;

    protected Edible lastSpawnedEnemy;

    private float spawnTimer = 0;

    private void Awake()
    {
        onSpawn = new EatenEvent();
    }

    protected virtual void Spawn()
    {
        Vector3 position = RandomPointInBox();
        lastSpawnedEnemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        onSpawn.Invoke(lastSpawnedEnemy);
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
        if (spawnTimer >= maxSpawnTime)
        {
            Spawn();
            spawnTimer = 0;
        }
        spawnTimer += Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, area);
    }

    private EatenEvent onSpawn;
}
