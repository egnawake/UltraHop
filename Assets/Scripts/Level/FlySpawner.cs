using UnityEngine;

public class FlySpawner : EnemySpawner
{
    [SerializeField] private WaypointTracker waypointTracker;

    protected override void Spawn()
    {
        base.Spawn();
        Fly fly = lastSpawnedEnemy.GetComponent<Fly>();
        fly.Flowers = waypointTracker.Waypoints;
    }
}
