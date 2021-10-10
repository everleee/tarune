using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    [HideInInspector] public Renderer rend;
    public Material mat1, mat2;

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
    }

    // Update is called once per frame
    private void Update()
    {
        gameObject.GetComponent<Renderer>().material = mat1;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.CompareTag("Grid"))
            {
                hit.collider.gameObject.GetComponent<Renderer>().material = mat2;
            }
        }
    }

}
