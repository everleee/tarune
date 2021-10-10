using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Grid : MonoBehaviour
{
    public Camera main;
    public GameObject duplicator;
    public GameObject cube;
    public GameObject player;
    public Vector3 start;
    public LayerMask layer;
    public Material mat1, mat2;
    MeshRenderer m_Renderer;

    // Start is called before the first frame update
    void Start()
    {
        
        start = duplicator.transform.position;
        m_Renderer = GetComponent<MeshRenderer>();

        for (int z = 0; z < 10; z++)
        {
            for (int x = 0; x < 10; x++)
            {
                GameObject cubeInstance = Instantiate(cube, duplicator.transform.position, Quaternion.identity);
                duplicator.transform.position = new Vector3(duplicator.transform.position.x + 1.1f, duplicator.transform.position.y, duplicator.transform.position.z);
            }

            duplicator.transform.position = new Vector3(duplicator.transform.position.x - 11, duplicator.transform.position.y, duplicator.transform.position.z + 1.1f);
        }

        GameObject playerInstance = Instantiate(player, new Vector3(0, 0.5f, 0), Quaternion.identity);
    }
}
