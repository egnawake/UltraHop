using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHp : MonoBehaviour

{
    [SerializeField] private float maxHp;
    [SerializeField] private float hpLossRate;
    [SerializeField] private ResourceBar hpBar;
    [SerializeField] private GameObject gameoverScreen;

     private float currentHp;

    void Start()
    {
        currentHp = maxHp;

    }

    // Update HP
    void Update()
    {
        currentHp = currentHp - hpLossRate * Time.deltaTime;

        hpBar.SetFill(currentHp / maxHp);

        // Game over condition
        if (currentHp == 0)
        {
            gameoverScreen.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
        }
    }
}
