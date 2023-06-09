using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private float timeAlive;
    [SerializeField] private float damage = 5f;

    private GameObject player;
    private PlayerHealth playerHealth;
    private Vector3 direction;
    private Rigidbody rb;

    void Awake()
    {
        player = GameObject.Find("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        direction = player.transform.position - gameObject.transform.position;
        rb = GetComponent<Rigidbody>();
        rb.velocity = direction.normalized * speed;
    }

    private void Update()
    {

        if (timeAlive <= lifeTime)
            timeAlive += Time.deltaTime;
        else
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == player)
        {
            Destroy(gameObject);
            Debug.Log("Hit player");
            playerHealth.Damage(damage);
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Hit something");
        }

    }
}
