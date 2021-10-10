using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    private StatePatternEnemy enemy;
    private int nextWayPoint;

    public PatrolState(StatePatternEnemy statePatternEnemy) //konstruktori funktio on aina saman niminen kuin luokan nimi
    {
        enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        Patrol();
        Look();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            ToAlertState();
        }
    }

    public void ToAlertState()
    {
        enemy.currentState = enemy.alertState;
    }

    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
    }

    public void ToPatrolState()
    {
        
    }

    public void ToTrackingState()
    {

    }

    public void ToSearchState()
    {

    }

    void Patrol()
    {
        enemy.indicator.material.color = Color.green;
        enemy.navMeshAgent.destination = enemy.wayPoints[nextWayPoint].position;
        enemy.navMeshAgent.isStopped = false;

        if(enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance && !enemy.navMeshAgent.pathPending) //tarkastetaan onko päästy kohteeseen
        {
            //reitti lopussa!
            nextWayPoint = (nextWayPoint + 1) % enemy.wayPoints.Length; //jakojäännös (esim. jos waypoint 2 jaetaan 3 eli taulukon pituudella, jäljelle jää 1. Jos 3/3 = 0 eli aloitetaan alusta
        }

    }

    void Look()
    {
        //visualisoidaan säde
        Debug.DrawRay(enemy.eye.position, enemy.eye.forward * enemy.sightRange, Color.green);
        //raycast silmästä eteenpäin
        RaycastHit hit;
        if(Physics.Raycast(enemy.eye.position, enemy.eye.forward, out hit, enemy.sightRange) && hit.collider.CompareTag("Player"))
        {
            enemy.chaseTarget = hit.transform;
            ToChaseState();
        }
    }
}
