using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float healthLossRate;
    [SerializeField] private ResourceBar healthBar;
    [SerializeField] private GameObject gameOverScreen;

    private float health;
    private bool infiniteHealth;

    public void Heal(float amount)
    {
        health = Mathf.Min(health + amount, maxHealth);
    }

    public void Damage(float amount)
    {
        health = Mathf.Max(health - amount, 0f);
    }
    
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
