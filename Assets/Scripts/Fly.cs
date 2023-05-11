using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibGameAI.FSMs;

public class Fly : MonoBehaviour
{

    [SerializeField] private float flowerDectionRange = 1f;
    [SerializeField] private float fearRange = 8f;
    [SerializeField] private float maxTimeAtFlower = 4f;

    [SerializeField] private float timeAtFlower;

    private Transform flower;
    private GameObject player;

    private StateMachine fsm;

    public IReadOnlyList<Transform> Flowers { private get; set; }

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    // Start is called before the first frame update
    private void Start()
    {
        flower = Flowers[0];

        State idleState = new State("Idle", () => Debug.Log("Enter idle state"), Waiting, () => Debug.Log("Exit on idle state"));

        State wonderState = new State("Wonder", () => Debug.Log("Enter wonder state"), MoveToNextFlower, () => Debug.Log("Exit on wonder state"));

        State afraidState = new State("Afraid", () => Debug.Log("Enter afraid state"), Run, () => Debug.Log("Exit on afraid state"));

        Transition Idle2Wonder = new Transition(
            () => timeAtFlower >= maxTimeAtFlower,
            () => Debug.Log("To the next flower"),
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

    private void MoveToNextFlower()
    {
        //mover para a flower mais proxima
    }

    private void Run()
    {
        //usar pathfinding de flower a flower a  tentar fugir do player
    }

    private void Waiting()
    {
        if (timeAtFlower <= maxTimeAtFlower)
            timeAtFlower += Time.deltaTime;
        else
        {
            timeAtFlower = 0;
        }
    }

}
