using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class Follower : MonoBehaviour
{
    public GameObject player;
    public Vector3 destination;
    public NavMeshAgent fnavMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        fnavMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        destination = player.transform.position;
    }
}
