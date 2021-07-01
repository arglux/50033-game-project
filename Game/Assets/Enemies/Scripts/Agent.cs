using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour {

    // TODO: split this out to State.cs
    private enum State {
        Roaming,
        Chasing
    }

    [SerializeField] Transform target;
    private NavMeshAgent agent;
    private Vector3 startingPosition;
    private Vector3 roamPosition;
    private State state;

    private void Awake() {
        state = State.Roaming;
    }

    // Start is called before the first frame update
    private void Start() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        startingPosition = transform.position;
        roamPosition = GetRandomPosition();
    }

    // Update is called once per frame
    private void Update() {
        // TODO: split out to stateController
        switch (state) {
            default: 
                break;

            case State.Roaming:
                // TODO: split this out to RandomRoam.cs
                agent.SetDestination(roamPosition);
                float reachedPositionDistance = 2f;
                if (Vector3.Distance(transform.position, roamPosition) < reachedPositionDistance) roamPosition = GetRandomPosition();
                FindTarget();
                break;

            case State.Chasing:
                Chase(target);
                Attack();
                break;
        }
    }

    // TODO: split these out to a child of Action.cs
    private Vector3 GetRandomPosition() {
        Vector3 randomVector = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
        return startingPosition + randomVector * Random.Range(2f, 2f);
    }

    private void Chase(Transform target) {
        agent.SetDestination(target.position);

        float stopChaseRange = 8f;
        if (Vector3.Distance(transform.position, target.position) > stopChaseRange) state = State.Roaming;
    }

    private void FindTarget() {
        float targetRange = 3f;
        if (Vector3.Distance(transform.position, target.position) < targetRange) state = State.Chasing;
    }

    // TODO: expand this to have actual attack projectiles spwaning
    private void Attack() {
        float attackRange = 2f;
        if (Vector3.Distance(transform.position, target.position) < attackRange) Debug.Log("Attacking!");
    }
}