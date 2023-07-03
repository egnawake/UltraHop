using UnityEngine;
using UnityEngine.Events;

public class Edible : MonoBehaviour
{
    [SerializeField] private string edibleTag;
    [SerializeField] private float healthRegenerated = 10f;

    public float HealthRegenerated => healthRegenerated;
    public EatenEvent OnEaten => onEaten;
    public string EdibleTag => edibleTag;

    private Animator m_Animator;

    public void BeEaten()
    {
        m_Animator.SetTrigger("Hit");
        onEaten.Invoke(this);
        FMODUnity.RuntimeManager.PlayOneShot("event:/ShroomDeath", transform.position);
        //FMODUnity.RuntimeManager.PlayOneShot("event:/FlyDeath", transform.position);
        Destroy(gameObject);
    }
    
    private void Awake()
    {
        onEaten = new EatenEvent();

        m_Animator = gameObject.GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        onEaten.RemoveAllListeners();
    }

    private EatenEvent onEaten;
}
