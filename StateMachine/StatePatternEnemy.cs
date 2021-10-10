using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StatePatternEnemy : MonoBehaviour
{
    public float searchTurnSpeed; //Alert tilan kääntymisnopeus
    public float searchingDuration; //Kauanko etsii Alert tilassa
    public float sightRange; //Kuinka kauas vihollinen näkee missä tahansa tilassa
    public Transform[] wayPoints; //Patroltilan waypointit taulukko
    public Transform[] randomLocations;
    public Transform eye; //silmä, josta läheteään näkösäde(raycast)
    public MeshRenderer indicator; //vihollisen päällä oleva laatikko, jonka väri muuttuu tilan mukaan (Patrol = vihreä, Alert = keltainen, Chase = punainen)

    [HideInInspector]
    public Transform chaseTarget; //Chase tilassa tallennetaan jahdattava kohde tähän muuttujaan

    [HideInInspector]
    public IEnemyState currentState;

    [HideInInspector]
    public PatrolState patrolState;

    [HideInInspector]
    public AlertState alertState;

    [HideInInspector]
    public ChaseState chaseState;

    [HideInInspector]
    public TrackingState trackingState;

    [HideInInspector]
    public SearchState searchState;

    [HideInInspector]
    public NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Awake()
    {
        patrolState = new PatrolState(this);
        alertState = new AlertState(this);
        chaseState = new ChaseState(this);
        trackingState = new TrackingState(this);
        searchState = new SearchState(this);

        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        currentState = patrolState;
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState();
    }

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }
}
