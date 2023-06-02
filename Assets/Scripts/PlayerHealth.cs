using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float healthLossRate;
    [SerializeField] private ResourceBar healthBar;
    [SerializeField] private GameObject gameOverScreen;

    private float health;
    private bool infiniteHealth;

    private void Start()
    {
        health = maxHealth;
        infiniteHealth = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Toggle Infinite Health"))
        {
            if (!infiniteHealth)
            {
                health = maxHealth;
                healthBar.SetFill(1f);
            }

            infiniteHealth = !infiniteHealth;
        }

        if (!infiniteHealth)
        {
            UpdateHealth();
        }
    }

    private void UpdateHealth()
    {
        // Update health
        health = health - healthLossRate * Time.deltaTime;

        healthBar.SetFill(health / maxHealth);

        // Game over condition
        if (health <= 0)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            gameOverScreen.SetActive(true);
        }
    }
}
