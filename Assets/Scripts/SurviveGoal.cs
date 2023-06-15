using UnityEngine;
using UnityEngine.Events;

public class SurviveGoal : MonoBehaviour, IGoal
{
    [SerializeField] private float surviveTime = 10f;

    private float timer;

    public GoalEvent OnAchieved => onAchieved;

    private void Awake()
    {
        timer = 0;
        onAchieved = new GoalEvent();
    }

    private void Update()
    {
        if (timer >= surviveTime)
        {
            onAchieved.Invoke(this);
        }

        timer += Time.deltaTime;
    }

    private GoalEvent onAchieved;
}
