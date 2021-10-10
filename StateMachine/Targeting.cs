using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    public GameObject host;

    private void OnTriggerEnter(Collider other)
    {

        if(other.CompareTag("Player"))
        {
            host.GetComponent<TurretEnemy>().Shoot();
            
        }
        
    }
}
