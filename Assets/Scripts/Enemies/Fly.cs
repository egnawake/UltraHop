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

    [SerializeField] private float stateHoldTime = 1.5f;

    private Transform flower;
    private Transform homeFlower;
    private GameObject player;
    private Vector3 direction;
    private Rigidbody rb;
    private float holdTimer = 0f;

    private FMOD.Studio.EventInstance flyBuzz;
    private FMOD.Studio.EventInstance flyWings;

    private StateMachine fsm;

    public IReadOnlyList<Transform> Flowers { private get; set; }

    private void Awake()
    {
        player = GameObject.Find("Player");

        rb = GetComponent<Rigidbody>();

        flyBuzz = FMODUnity.RuntimeManager.CreateInstance("event:/FlyBzz");
        flyWings = FMODUnity.RuntimeManager.CreateInstance("event:/FlyWings");
    }

    // Start is called before the first frame update
    private void Start()
    {

        flyBuzz.start();
        flyWings.start();

        PickNextFlower();

        homeFlower = Flowers[Flowers.Count - 1];

        State idleState = new State("Idle", null, Waiting, null);

        State wonderState = new State("Wonder", null, MoveToNextFlower, null);

        Action afraidActions = ResetHoldTimer;
        afraidActions += SetRunDirection;
        State afraidState = new State("Afraid", afraidActions, Run, null);

        Transition Idle2Wonder = new Transition(
            () => timeAtFlower >= maxTimeAtFlower,
            () => LeavingIddleState(),
            wonderState
            );

        idleState.AddTransition(Idle2Wonder);

        Transition Wonder2Idle = new Transition(
            () => (transform.position - flower.transform.position).magnitude < flowerDectionRange,
            StopMoving,
            idleState
            );

        wonderState.AddTransition(Wonder2Idle);

        Transition Wonder2Afraid = new Transition(
            ShouldRun,
            null,
            afraidState
            );

        wonderState.AddTransition(Wonder2Afraid);

        Transition Afraid2Wonder = new Transition(
            ShouldWander,
            null,
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
        rb.velocity = direction.normalized * maxSpeed;

        holdTimer += Time.deltaTime;
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

    private bool ShouldWander()
    {
        bool playerIsAway = (transform.position - player.transform.position).magnitude > fearRange;
        bool minTimeHasPassed = holdTimer > stateHoldTime;

        return minTimeHasPassed && playerIsAway;
    }

    private bool ShouldRun()
    {
        bool playerIsNearby = (transform.position - player.transform.position).magnitude <= fearRange;

        return playerIsNearby;
    }

    private void StopMoving()
    {
        rb.velocity = Vector3.zero;
    }

    private void ResetHoldTimer()
    {
        holdTimer = 0f;
    }

    private void SetRunDirection()
    {
        Vector3 dir = transform.position - player.transform.position;
        float dot = Vector3.Dot(dir, Vector3.up);

        if (dot < 0)
        {
            dir = new Vector3(dir.x, 0f, dir.z).normalized;
        }

        direction = dir;
    }
}
