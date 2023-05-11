using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float healthLossRate;
    [SerializeField] private ResourceBar healthBar;
    [SerializeField] private GameObject gameOverScreen;

    private float health;

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        // Update health
        health = health - healthLossRate * Time.deltaTime;

        healthBar.SetFill(health / maxHealth);

        // Game over condition
        if (health == 0)
        {
            gameOverScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
