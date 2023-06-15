using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DefeatEnemiesGoal : MonoBehaviour, IGoal
{
    [SerializeField] private string enemyTag;
    [SerializeField] private int enemiesToKill = 5;

    private EnemySpawner[] spawners;
    private IDictionary<string, int> enemyKillCounters;

    public GoalEvent OnAchieved => onAchieved;

    private void Awake()
    {
        enemyKillCounters = new Dictionary<string, int>();
        onAchieved = new GoalEvent();
    }

    private void Start()
    {
        spawners = GetComponentsInChildren<EnemySpawner>();
        foreach (EnemySpawner s in spawners)
        {
            s.OnSpawn.AddListener(HandleEnemySpawn);
        }
    }

    private void HandleEnemyDeath(Edible e)
    {
        string id = e.EdibleTag;

        if (!enemyKillCounters.ContainsKey(id))
            enemyKillCounters[id] = 0;
        enemyKillCounters[id] += 1;

        Debug.Log($"Enemy killed: <{id}>: {enemyKillCounters[id]}");

        if (enemyKillCounters.ContainsKey(enemyTag)
            && enemyKillCounters[enemyTag] >= enemiesToKill)
        {
            onAchieved.Invoke(this);
        }
    }

    private void HandleEnemySpawn(Edible e)
    {
        e.OnEaten.AddListener(HandleEnemyDeath);
    }

    private GoalEvent onAchieved;
}
