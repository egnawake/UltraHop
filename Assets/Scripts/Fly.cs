using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibGameAI.FSMs;
using Random = UnityEngine.Random;

public class Fly : MonoBehaviour
{

    [SerializeField] private float flowerDectionRange = 1f;
    [SerializeField] private float fearRange = 8f;
    [SerializeField] private float maxTimeAtFlower = 4f;

    [SerializeField] private float timeAtFlower;
    [SerializeField] private float maxSpeed;
    [SerializeField] private bool isNatural;

    private Transform flower;
    private Transform homeFlower;
    private GameObject player;
    private Vector3 direction;
    private Rigidbody rb;

    private StateMachine fsm;

    public IReadOnlyList<Transform> Flowers { private get; set; }

    private void Awake()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    private void Start()
    {

        PickNextFlower();

        homeFlower = Flowers[Flowers.Count - 1];

        State idleState = new State("Idle", () => Debug.Log("Enter idle state"), Waiting, () => Debug.Log("Exit on idle state"));

        State wonderState = new State("Wonder", () => Debug.Log("Enter wonder state"), MoveToNextFlower, () => Debug.Log("Exit on wonder state"));

        State afraidState = new State("Afraid", () => Debug.Log("Enter afraid state"), Run, () => Debug.Log("Exit on afraid state"));

        Transition Idle2Wonder = new Transition(
            () => timeAtFlower >= maxTimeAtFlower,
            () => LeavingIddleState(),
            wonderState
            );

        idleState.AddTransition(Idle2Wonder);

        Transition Wonder2Idle = new Transition(
            () => (transform.position - flower.transform.position).magnitude < flowerDectionRange,
            () => Debug.Log("Arrived at flower"),
            idleState
            );

        wonderState.AddTransition(Wonder2Idle);

        Transition Wonder2Afraid = new Transition(
            () => (transform.position - player.transform.position).magnitude < fearRange,
            () => Debug.Log("You're too close!"),
            afraidState
            );

        wonderState.AddTransition(Wonder2Afraid);

        Transition Afraid2Wonder = new Transition(
            () => (transform.position - player.transform.position).magnitude > fearRange,
            () => Debug.Log("I'm safe, next flower!"),
            wonderState
            );

        afraidState.AddTransition(Afraid2Wonder);

        fsm = new StateMachine(wonderState);

    }

    // Update is called once per frame
    private void Update()
    {

        Action actionToDo = fsm.Update();
        actionToDo?.Invoke();

    }

    private void LeavingIddleState()
    {
        timeAtFlower = 0;
        PickNextFlower();
    }

    private void MoveToNextFlower()
    {
        if (!isNatural)
        {
            float distance = float.MaxValue;

            foreach (Transform target in Flowers)
            {
                float distanceToFlower;

                distanceToFlower = (transform.position - target.transform.position).magnitude;

                if (distanceToFlower < distance)
                {
                    distance = distanceToFlower;
                    flower = target;
                }
            }
        }
        Move();
    }

    private void PickNextFlower()
    {
        flower = Flowers[Random.Range(0, Flowers.Count)];
    }

    private void Run()
    {
        direction = homeFlower.transform.position - gameObject.transform.position;
        rb.velocity = direction.normalized * maxSpeed;
    }

    private void Waiting()
    {
            timeAtFlower += Time.deltaTime;
    }

    private void Move()
    {
        direction = flower.transform.position - gameObject.transform.position;
        rb.velocity = direction.normalized * maxSpeed;
    }
}
