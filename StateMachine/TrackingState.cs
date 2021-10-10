using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingState : IEnemyState
{
    private StatePatternEnemy enemy;
    

    public TrackingState(StatePatternEnemy statePatternEnemy) //konstruktori funktio on aina saman niminen kuin luokan nimi
    {
        enemy = statePatternEnemy;
        
    }

    public void UpdateState()
    {
        Track();
        Look();
    }

    public void OnTriggerEnter(Collider other)
    {
      
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
        enemy.currentState = enemy.patrolState;
    }

    public void ToTrackingState()
    {

    }

    public void ToSearchState()
    {
        enemy.randomLocations[0].position = newRand();
        Debug.Log(enemy.randomLocations[0].position);
        enemy.randomLocations[1].position = newRand();
        Debug.Log(enemy.randomLocations[1].position);
        enemy.randomLocations[2].position = newRand();
        Debug.Log(enemy.randomLocations[2].position);
        enemy.randomLocations[3].position = newRand();
        Debug.Log(enemy.randomLocations[3].position);
        enemy.currentState = enemy.searchState;
    }

    void Track()
    {
        enemy.indicator.material.color = Color.blue;

        if(enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance && !enemy.navMeshAgent.pathPending)
        {
            ToSearchState();
        }
    }

    void Look()
    {
        //visualisoidaan säde
        Debug.DrawRay(enemy.eye.position, enemy.eye.forward * enemy.sightRange, Color.blue);
        //raycast silmästä eteenpäin
        RaycastHit hit;
        if (Physics.Raycast(enemy.eye.position, enemy.eye.forward, out hit, enemy.sightRange) && hit.collider.CompareTag("Player"))
        {
            enemy.chaseTarget = hit.transform;
            ToChaseState();
        }
    }

    public Vector3 newRand()
    {
        return new Vector3(Random.Range(enemy.transform.position.x - 10, enemy.transform.position.x + 10), enemy.transform.position.y, (Random.Range(enemy.transform.position.z - 10, enemy.transform.position.z + 10)));
    }

}