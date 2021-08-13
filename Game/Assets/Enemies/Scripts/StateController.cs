using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour
{
    public EnemyConstants constants;


    [Header ("Enemy State Trackers")]
    public State initialState;
    public State currentState;
    public State remainState;
    [HideInInspector] public bool transitionStateChanged = false;
    public float aggro = 0;
    public float blinded = 0;
    public float taunted = 0;
    public float lastAttack = 0;
    public float timeSince = 0;

    [Header ("Enemy Components")]
    public Transform eyes;
    public Transform hands;


    [Header ("Enemy Navigation Behaviour")]
    [HideInInspector] public List<Transform> wayPointList;  
    [HideInInspector] public Vector3 lastSeen;
    [HideInInspector] public Vector3 lastPosition; // to check if stuck
    [HideInInspector] public float stuckTime; // to check if stuck
    [HideInInspector] public int nextWayPoint;
    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Animator animator;
    

    private bool aiActive = true;
    private SpriteRenderer sprite;
    private Vector3 initialHandPos;

    void Awake()
    {
        sprite = this.gameObject.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.angularSpeed = 0; // prevent rotation while moving
        initialHandPos = hands.localPosition;
        animator = GetComponentInChildren<Animator>();
        Debug.Log(animator);
        lastAttack=Time.time;
    }

    void OnEnable()
    {
        currentState = initialState;
    }

    void Update()
    {
        // Debug.Log(navMeshAgent.velocity);
        // Debug.Log(navMeshAgent.velocity.x);
        FlipSprite();

        if (!aiActive) return;
        animator.SetFloat("xSpeed", Mathf.Abs(navMeshAgent.velocity.x));
        currentState.UpdateState(this);
        aggro = Mathf.Max(0, aggro-Time.deltaTime);
        taunted = Mathf.Max(0, taunted-Time.deltaTime);
        blinded = Mathf.Max(0, blinded-Time.deltaTime);
    }
    private void FlipSprite()
    {
        if (navMeshAgent.velocity.x < -0.01) {
            sprite.flipX = true;
            hands.localPosition = new Vector3(-initialHandPos.x, initialHandPos.y, initialHandPos.z);
        } else {
            sprite.flipX = false;
            hands.localPosition = initialHandPos;
        }
    }

    public void TransitionToState(State nextState)
    {
        if (nextState != remainState) {
            currentState = nextState;
            transitionStateChanged = true;
        }
    }

    public void Aggro(Transform target) { //normal torchlight
        aggro = 0.5f;
        if (chaseTarget==null) {
            chaseTarget = target;
        }
    }

    public void InflictTaunted(Transform target) { //tank torchlight
        taunted = 0.5f;
        chaseTarget = target;
    }

    public void InflictBlind() { // dps torchlight
        blinded = 0.5f;
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        timeSince = Time.time - lastAttack;
        if (Time.time - lastAttack >= duration)
        {
            lastAttack = Time.time;
            return true;
        } else
        {
            return false;
        }
    }

    public void SetWaypointList(List<Transform> waypoints)
    {
        wayPointList = waypoints;
    }

    public void SetLastSeen(Transform investigationPoint) 
    {
        lastSeen = investigationPoint.position;
    }
}
