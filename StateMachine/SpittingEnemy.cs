using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpittingEnemy : MonoBehaviour
{
    //This script is for a patrolling enemy, that rotates to face the player once inside it's trigger, then spits. If player exits the trigger, it waits a bit and continues to patrol.

    //BUG: if player is going fast in and out of the trigger, more spit is being instantiated than it's supposed to - there for the script is still 'work in progress'

    public GameObject target, spit, spitSpawn;
    public Transform[] patrolSpots;
    private int nextSpot;
    public float rotationSpeed, spitSpeed;
    private NavMeshAgent navMeshAgent;
    private bool rdyToSpit, inCollider;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        navMeshAgent.isStopped = false;
        nextSpot = 0;
        target = GameObject.Find("Player");
        rdyToSpit = false;
        inCollider = false;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyPatrol();
        if (navMeshAgent.isStopped == true && rdyToSpit == true) //starting the coroutine to spit, if nav mesh agent is stopped and the enemy is ready to spit
        {
            StartCoroutine(Spit());
        }
    }

    private void OnTriggerEnter(Collider other) //when Player is entering the enemies trigger, it sets booleans ready to spit and inside the collider as true
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player enters the collider!");
            rdyToSpit = true;
            inCollider = true;
        }
    }

    private void OnTriggerStay(Collider other) //Checkin the tag of the trigger gameobject. When player stays in the enemies collider nav mesh agant is stopped and we're calling the rotate function
    {
        if (other.gameObject.CompareTag("Player"))
        {
            navMeshAgent.isStopped = true;
            Rotate();
        }
    }

    private void OnTriggerExit(Collider other) //When player exits the enemys collider the WaitTime coroutine is called and in collider is set to false
    {
        if (other.gameObject.CompareTag("Player"))
        {
            rdyToSpit = false;
            StartCoroutine(WaitTime(2f));
            inCollider = false;
        }
    }

    private void EnemyPatrol() //Enemy patrols between two points, nextSpot is changed once path remaining distance is less than 0.5 seconds
    {
        navMeshAgent.destination = patrolSpots[nextSpot].position;
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            Debug.Log("patrolling");
            if (nextSpot == 0)
            {
                nextSpot = 1;
            }
            else
            {
                nextSpot = 0;
            }
        }     
    }

    IEnumerator WaitTime(float waitTime) // This coroutine waits for a while and then activates the nav mesh agent to patrol again if the player isnt in the collider
    {
        Debug.Log("Waiting the given time.");
        yield return new WaitForSecondsRealtime(waitTime);
        if (navMeshAgent.isStopped == true && inCollider == false)
        {
            navMeshAgent.isStopped = false;
        }
    }

    private void Rotate() //Taking the correct rotation and then changing the rotation smoothly with Slerp
    {
        var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    IEnumerator Spit() //once the spit is called, the ready to spit is set to false so that it wont called again in Update function until the coroutine has ended
    {
        rdyToSpit = false;
        yield return new WaitForSecondsRealtime(2f); // this is here already, in order to give the player a bit of time to react to enemies agro(aka going inside it's trigger)
        if (inCollider) //if the player is still in the collider, only then it will spit
        {
            GameObject projectileInstance = Instantiate(spit, spitSpawn.transform.position, Quaternion.identity);
            projectileInstance.GetComponent<Rigidbody>().velocity = spitSpawn.transform.forward * spitSpeed;
            
            rdyToSpit = true;
        }
    }
}
