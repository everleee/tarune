using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{

    public GameObject player, follower;
    public NavMeshAgent navMeshAgent;
    public Vector3 moveHere;
    public AudioClip ding2;
    public GameObject ding;
    public GameObject follower1, follower2, follower3, follower4;
    public bool spawned;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

        follower1 = Instantiate(follower, new Vector3(7, 1, 2), Quaternion.identity);
        follower2 = Instantiate(follower, new Vector3(7, 1, 4), Quaternion.identity);
        follower3 = Instantiate(follower, new Vector3(7, 1, 6), Quaternion.identity);
        follower4 = Instantiate(follower, new Vector3(7, 1, 8), Quaternion.identity);

        spawned = false;
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.CompareTag("Grid"))
                {
                    moveHere = hit.collider.gameObject.transform.position;
                    spawned = false;
                }

                if (hit.collider.gameObject.CompareTag("Follower"))
                {
                    if (hit.collider.gameObject.GetComponent<NavMeshAgent>().speed == 3)
                    {
                        hit.collider.gameObject.GetComponent<NavMeshAgent>().speed = 7;
                    }
                    else
                    {
                        hit.collider.gameObject.GetComponent<NavMeshAgent>().speed = 3;
                    }
                }
            }

            navMeshAgent.destination = moveHere;
        }

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending)
        {
            MoveFollowers();
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending && follower1.GetComponent<NavMeshAgent>().remainingDistance <= follower1.GetComponent<NavMeshAgent>().stoppingDistance && !follower1.GetComponent<NavMeshAgent>().pathPending
                && follower2.GetComponent<NavMeshAgent>().remainingDistance <= follower2.GetComponent<NavMeshAgent>().stoppingDistance && !follower2.GetComponent<NavMeshAgent>().pathPending && follower3.GetComponent<NavMeshAgent>().remainingDistance <= follower3.GetComponent<NavMeshAgent>().stoppingDistance && !follower3.GetComponent<NavMeshAgent>().pathPending
                && follower4.GetComponent<NavMeshAgent>().remainingDistance <= follower4.GetComponent<NavMeshAgent>().stoppingDistance && !follower4.GetComponent<NavMeshAgent>().pathPending)
            {
                Debug.Log("kaikki paikalla");
                Bell();
                spawned = true;
            }
        }
    }

    public void MoveFollowers()
    {
        follower1.GetComponent<NavMeshAgent>().destination = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1.5f);
        follower2.GetComponent<NavMeshAgent>().destination = new Vector3(transform.position.x - 1.5f, transform.position.y, transform.position.z);
        follower3.GetComponent<NavMeshAgent>().destination = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1.5f);
        follower4.GetComponent<NavMeshAgent>().destination = new Vector3(transform.position.x + 1.5f, transform.position.y, transform.position.z);
    }

    public void Bell()
    {
        if (spawned == false)
        {
            Instantiate(ding);
            Destroy(GameObject.Find("ding(Clone)"), 5);

            spawned = true;
        }
    }
}
