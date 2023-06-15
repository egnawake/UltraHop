using UnityEngine.Events;

public interface IGoal
{
    GoalEvent OnAchieved { get; }
}
