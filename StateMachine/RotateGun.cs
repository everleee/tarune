using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGun : MonoBehaviour
{
    public float currentAngle;
    public float startAngle;
    public bool rotating; // Onko ase kääntymässä
    public float rotateDuration;
    public float counter;
    private float _xAngle;
    public float xAngle
    {
        get { return _xAngle; }
        set
        {
            Debug.Log("Uusi kulma johon käännytään on: " + value);
            startAngle = transform.localRotation.eulerAngles.x; // Kulma missä tykki on nyt
            _xAngle = value;
            rotating = true;
            counter = 0;

        }

    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter > rotateDuration && rotating == true)
        {
            // Tässä kohtaa piippu osoittaa oikeaan kulmaan. Tästä pitää laittaa info TurretEnemylle, että ollaan saatu käännnetty ase oikein. 
            rotating = false;
        }
        currentAngle = Mathf.LerpAngle(startAngle, xAngle, counter / rotateDuration);
        transform.localEulerAngles = new Vector3(currentAngle, 0, 0);


    }
}
