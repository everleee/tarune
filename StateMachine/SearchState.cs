using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : IEnemyState
{
    private StatePatternEnemy enemy;
    private int nextRandLocation;

    public SearchState(StatePatternEnemy statePatternEnemy)
    {
        enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        Look();
        RandomSearch();
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
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
        nextRandLocation = 0;
        enemy.currentState = enemy.chaseState;
    }

    public void ToPatrolState()
    {
        nextRandLocation = 0;
        enemy.currentState = enemy.patrolState;
    }

    public void ToSearchState()
    {
        
    }

    public void ToTrackingState()
    {
        
    }

    public void RandomSearch()
    {
        
        enemy.indicator.material.color = Color.cyan;
        enemy.navMeshAgent.destination = enemy.randomLocations[nextRandLocation].position;
        enemy.navMeshAgent.isStopped = false;

        if (enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance && !enemy.navMeshAgent.pathPending) //tarkastetaan onko päästy kohteeseen
        {

            Debug.Log("päästy kohteeseen");
            Debug.Log("kohde oli " + enemy.randomLocations[nextRandLocation]);
            //reitti lopussa!
            if (nextRandLocation < (enemy.randomLocations.Length - 1))
            {
                nextRandLocation++;
            }
            else
            {
                ToPatrolState();
            }
        }
       
    }

    void Look()
    {
        //visualisoidaan säde
        Debug.DrawRay(enemy.eye.position, enemy.eye.forward * enemy.sightRange, Color.cyan);
        //raycast silmästä eteenpäin
        RaycastHit hit;
        if (Physics.Raycast(enemy.eye.position, enemy.eye.forward, out hit, enemy.sightRange) && hit.collider.CompareTag("Player"))
        {
            enemy.chaseTarget = hit.transform;
            ToChaseState();
        }
    }

}
