using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitDestroy : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
