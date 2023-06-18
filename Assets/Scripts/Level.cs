using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private GoalCombineMode goalCombineMode;
    [SerializeField] private GameObject levelCompleteScreen;
    [SerializeField] private GoalDisplay goalDisplayPrefab;
    [SerializeField] private Transform goalDisplayRoot;

    private IDictionary<IGoal, bool> completedGoals;

    private void Awake()
    {
        completedGoals = new Dictionary<IGoal, bool>();
    }

    private void Start()
    {
        IGoal[] goals = GetComponents<IGoal>();
        foreach (IGoal g in goals)
        {
            completedGoals[g] = false;
            g.OnAchieved.AddListener(HandleGoalAchieved);

            GoalDisplay goalDisplay = Instantiate(goalDisplayPrefab, goalDisplayRoot);
            g.Display = goalDisplay;
        }
    }

    private void HandleGoalAchieved(IGoal goal)
    {
        completedGoals[goal] = true;

        if (goalCombineMode == GoalCombineMode.Any)
        {
            FinishLevel();
            return;
        }

        if (goalCombineMode == GoalCombineMode.All)
        {
            foreach (IGoal g in completedGoals.Keys)
            {
                if (!completedGoals[g]) return;
            }

            FinishLevel();
        }
    }

    private void FinishLevel()
    {
        levelCompleteScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;

        SaveLoad.LevelProgress = 2;
    }
}
