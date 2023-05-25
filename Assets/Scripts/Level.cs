using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] private Edible[] targets;

    private ISet<Edible> toBeEaten;

    private void Start()
    {
        toBeEaten = new HashSet<Edible>(targets);

        foreach (Edible e in targets)
        {
            e.OnEaten.AddListener(HandleTargetEaten);
        }
    }

    private void HandleTargetEaten(Edible target)
    {
        toBeEaten.Remove(target);

        if (toBeEaten.Count <= 0)
        {
            Debug.Log("Level complete!");
            SceneManager.LoadScene("LevelCompletion");
        }
    }
}
