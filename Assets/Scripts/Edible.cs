using UnityEngine;
using UnityEngine.Events;

public class Edible : MonoBehaviour
{
    [SerializeField] private string edibleTag;
    [SerializeField] private float healthRegenerated = 10f;

    public float HealthRegenerated => healthRegenerated;
    public EatenEvent OnEaten => onEaten;
    public string EdibleTag => edibleTag;

    public void BeEaten()
    {
        onEaten.Invoke(this);
        FMODUnity.RuntimeManager.PlayOneShot("event:/ShroomDeath", transform.position);
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
