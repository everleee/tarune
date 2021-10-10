using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IEnemyState
{
    private StatePatternEnemy enemy;

    public ChaseState(StatePatternEnemy statePatternEnemy)
    {
        enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        Chase();
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

    }

    public void ToPatrolState()
    {
        enemy.currentState = enemy.patrolState;
    }

    public void ToTrackingState()
    {
        enemy.currentState = enemy.trackingState;
    }

    public void ToSearchState()
    {

    }

    void Chase()
    {
        enemy.indicator.material.color = Color.red;
        enemy.navMeshAgent.destination = enemy.chaseTarget.position;
        enemy.navMeshAgent.isStopped = false;
    }

    void Look()
    {

        Vector3 enemyToTarget = enemy.chaseTarget.position - enemy.eye.position;

        //visualisoidaan säde
        Debug.DrawRay(enemy.eye.position, enemyToTarget, Color.red);
        //raycast silmästä eteenpäin
        RaycastHit hit;
        if (Physics.Raycast(enemy.eye.position, enemyToTarget, out hit, enemy.sightRange) && hit.collider.CompareTag("Player"))
        {
            enemy.chaseTarget = hit.transform;
            
        }
        else
        {
            //kohde on hävinnyt näkökentästä tai on tarpeeksi kaukana
            ToTrackingState();
        }
    }
}
