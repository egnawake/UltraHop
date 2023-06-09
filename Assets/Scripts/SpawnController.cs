using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private int enemyLimit = 15;

    private EnemySpawner spawner;
    private IList<Edible> enemies;

    private void Awake()
    {
        enemies = new List<Edible>();
    }

    private void Start()
    {
        spawner = GetComponent<EnemySpawner>();
        spawner.OnSpawn.AddListener(HandleSpawn);
    }

    private void HandleSpawn()
    {
        enemies.Add(spawner.LastSpawnedEnemy);
        spawner.LastSpawnedEnemy.OnEaten.AddListener(HandleEnemyDeath);

        if (enemies.Count >= enemyLimit)
        {
            spawner.enabled = false;
        }
    }

    private void HandleEnemyDeath(Edible e)
    {
        enemies.Remove(e);

        if (enemies.Count < enemyLimit)
        {
            spawner.enabled = true;
        }
    }
}
