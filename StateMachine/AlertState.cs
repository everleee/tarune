using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : IEnemyState
{
    private StatePatternEnemy enemy;
    float searchTimer;

    public AlertState(StatePatternEnemy statePatternEnemy)
    {
        enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        Search();
        Look();
    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void ToAlertState()
    {

    }

    public void ToChaseState()
    {
        searchTimer = 0;
        enemy.currentState = enemy.chaseState;
    }

    public void ToPatrolState()
    {
        searchTimer = 0;
        enemy.currentState = enemy.patrolState;
    }

    public void ToTrackingState()
    {

    }
    public void ToSearchState()
    {

    }

    void Search()
    {
        enemy.indicator.material.color = Color.yellow;
        enemy.navMeshAgent.isStopped = true; //pysäytetään vihollinen

        enemy.transform.Rotate(0, enemy.searchTurnSpeed * Time.deltaTime, 0);
        searchTimer += Time.deltaTime;

        if(searchTimer >= enemy.searchingDuration)
        {
            ToPatrolState();
        }
    }

    void Look()
    {
        //visualisoidaan säde
        Debug.DrawRay(enemy.eye.position, enemy.eye.forward * enemy.sightRange, Color.yellow);
        //raycast silmästä eteenpäin
        RaycastHit hit;
        if (Physics.Raycast(enemy.eye.position, enemy.eye.forward, out hit, enemy.sightRange) && hit.collider.CompareTag("Player"))
        {
            enemy.chaseTarget = hit.transform;
            ToChaseState();
        }
    }
}
