using UnityEngine;
using UnityEngine.Events;

public class Edible : MonoBehaviour
{
    [SerializeField] private float healthRegenerated = 10f;

    public float HealthRegenerated => healthRegenerated;
    public EatenEvent OnEaten => onEaten;

    public void BeEaten()
    {
        onEaten.Invoke(this);
        Destroy(gameObject);
    }

    private void Awake()
    {
        onEaten = new EatenEvent();
    }

    private void OnDestroy()
    {
        onEaten.RemoveAllListeners();
    }

    private EatenEvent onEaten;
}
