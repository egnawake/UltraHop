using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private GameObject levelCompleteScreen;

    private void Start()
    {
        IGoal goal = GetComponent<IGoal>();
        goal.OnAchieved.AddListener(HandleGoalAchieved);
    }

    private void HandleGoalAchieved()
    {
        levelCompleteScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }
}
