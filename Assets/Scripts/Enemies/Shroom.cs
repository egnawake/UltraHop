using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibGameAI.FSMs;

public class Shroom : MonoBehaviour
{

    [SerializeField] private float attackRange = 10f;
    [SerializeField] private float fearRange = 4f;
    [SerializeField] private float reloadSpeed = 4f;

    [SerializeField] private float timeSinceReload;
    [SerializeField] private bool veryScared = false;

    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject model;

    private GameObject player;

    private StateMachine fsm;

    private void Awake()
    {
        player = GameObject.Find("Player");

    }

    // Start is called before the first frame update
    private void Start()
    {

        State idleState = new State("Idle", () => Debug.Log("Enter idle state"), null, () => Debug.Log("Exit on idle state"));

        State attackState = new State("Attack", () => Debug.Log("Enter attack state"), AttackPlayer, () => Debug.Log("Exit on attack state"));

        State afraidState = new State("Afraid", () => Debug.Log("Enter afraid state"), Hide, () => Debug.Log("Exit on afraid state"));

        Transition Idle2Attack = new Transition(
            () => (transform.position - player.transform.position).magnitude < attackRange,
            () => Debug.Log("See player"),
            attackState
            );

        idleState.AddTransition(Idle2Attack);

        Transition Attack2Idle = new Transition(
            () => (transform.position - player.transform.position).magnitude > attackRange,
            () => Debug.Log("Don't see player"),
            idleState
            );

        attackState.AddTransition(Attack2Idle);

        Transition Attack2Afraid = new Transition(
            () => (transform.position - player.transform.position).magnitude < fearRange,
            () => FMODUnity.RuntimeManager.PlayOneShot("event:/ShroomFear", transform.position),
            afraidState
            );

        attackState.AddTransition(Attack2Afraid);

        Transition Afraid2Attack = new Transition(
            () => (transform.position - player.transform.position).magnitude > fearRange && !veryScared,
            () => model.SetActive(true),
            attackState
            );

        afraidState.AddTransition(Afraid2Attack);

        Transition Afraid2Idle = new Transition(
            () => (transform.position - player.transform.position).magnitude > attackRange && veryScared,
            () => model.SetActive(true),
            idleState
            );

        afraidState.AddTransition(Afraid2Idle);

        fsm = new StateMachine(idleState);

    }

    // Update is called once per frame
    private void Update()
    {

        Action actionToDo = fsm.Update();
        actionToDo?.Invoke();

    }

    private void AttackPlayer()
    {
        if (timeSinceReload <= reloadSpeed)
            timeSinceReload += Time.deltaTime;
        else
        {
            Instantiate(bullet, transform.position, transform.rotation);
            FMODUnity.RuntimeManager.PlayOneShot("event:/EnemyAttack", transform.position);
            timeSinceReload = 0;
        }
    }

    private void Hide()
    {
        model.SetActive(false);
    }

}