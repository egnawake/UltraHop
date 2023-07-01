using UnityEngine;
using UnityEngine.Events;

public class SurviveGoal : MonoBehaviour, IGoal
{
    [SerializeField] private float surviveTime = 10f;

    private float timer;

    public GoalEvent OnAchieved => onAchieved;
    public GoalDisplay Display
    {
        set
        {
            display = value;
            display.SetInfo($"Survive for {surviveTime} seconds!");
            display.SetProgress("0");
        }
    }

    private GoalDisplay display;

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

        display.SetProgress(Mathf.Floor(timer).ToString());
    }

    private GoalEvent onAchieved;
}
