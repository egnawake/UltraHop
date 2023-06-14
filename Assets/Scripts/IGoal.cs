using UnityEngine.Events;

public interface IGoal
{
    UnityEvent OnAchieved { get; }
}
