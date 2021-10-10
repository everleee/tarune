using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : MonoBehaviour
{

    public GameObject targetLocation;
    public GameObject ammo;
    public GameObject ammoSpawn;
    public GameObject gunRotator;
    public float force; // Tykki ampuu aina samalla voimakkuudella, mutta se osaa muuttaa kulman oikeaksi. 
    public Vector3 gravity;
    private int angleMultiplier;


    // Start is called before the first frame update
    void Start()
    {
        gravity = Physics.gravity;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.LookAt(new Vector3(targetLocation.transform.position.x, targetLocation.transform.position.y, targetLocation.transform.position.z));
    }

    public void Shoot()
    {
        StartCoroutine(ShootBalls());

    }



    IEnumerator ShootBalls()
    {
        Debug.Log("Pelaaja on alueella. Ammutaan pallo");

        Vector3[] direction = HitTargetBySpeed(ammoSpawn.transform.position, targetLocation.transform.position, gravity, force);

        if (gameObject.transform.position.z < targetLocation.transform.position.z)
        {
            angleMultiplier = -1;
        }
        else
        {
            angleMultiplier = 1;
        }


        gunRotator.GetComponent<RotateGun>().xAngle = Mathf.Atan(direction[0].y / direction[0].z) * Mathf.Rad2Deg * angleMultiplier;

        // tähän kohtaan pitäisi saada odotus, että tykki on ehtinyt kääntyä oikeaan kulmaan. Miten se tehdään?
        // Coroutinessa on mahdollisuus odottaa

        yield return new WaitUntil(() => gunRotator.GetComponent<RotateGun>().rotating == false);

        GameObject projectile = Instantiate(ammo, ammoSpawn.transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody>().AddRelativeForce(direction[0], ForceMode.Impulse);

        yield return new WaitForSeconds(1);

        gunRotator.GetComponent<RotateGun>().xAngle = Mathf.Atan(direction[1].y / direction[1].z) * Mathf.Rad2Deg * angleMultiplier;

        yield return new WaitUntil(() => gunRotator.GetComponent<RotateGun>().rotating == false);

        GameObject projectile2 = Instantiate(ammo, ammoSpawn.transform.position, Quaternion.identity);
        projectile2.GetComponent<Rigidbody>().AddRelativeForce(direction[1], ForceMode.Impulse);


    }

    public Vector3[] HitTargetBySpeed(Vector3 startPosition, Vector3 targetPosition, Vector3 gravityBase, float launchSpeed)
    {

        Vector3 AtoB = targetPosition - startPosition;

        Vector3 horizontal = GetHorizontalVector(AtoB, gravityBase, startPosition);
        float horizontalDistance = horizontal.magnitude;


        Vector3 vertical = GetVerticalVector(AtoB, gravityBase, startPosition);
        float verticalDistance = vertical.magnitude * Mathf.Sign(Vector3.Dot(vertical, -gravityBase)); // Kertolaskun pitää tietää, onko ylös vai alas, eli onko kerroin 1 vai -1


        float x2 = horizontalDistance * horizontalDistance;
        float v2 = launchSpeed * launchSpeed;
        float v4 = launchSpeed * launchSpeed * launchSpeed * launchSpeed;

        float gravMag = gravityBase.magnitude;

        // LAUNCH TEST. Mikäli Launchtest (float) on positiivinen, niin silloin on olemassa mahdollisuus osua kohteeseen annetulla forcen voimalla. Jos Launchtest on negatiivinen
        // Ei ole mitään mahdollisuutta osua kohteeseen vaikka ammuttaisiin 45 asteen kulmassa. 

        float launchTest = v4 - (gravMag * ((gravMag * x2) + (2 * verticalDistance)));

        Debug.Log("LAUNCHTEST: " + launchTest);

        Vector3[] launch = new Vector3[2];

        if (launchTest < 0)
        {
            Debug.Log("Ei voida osua maaliin. Ammutaan kuitenkin 45 asteen kulmassa");
            launch[0] = (horizontal.normalized * launchSpeed * Mathf.Cos(45.0f * Mathf.Deg2Rad)) - (gravityBase.normalized * launchSpeed * Mathf.Sin(45.0f * Mathf.Deg2Rad));
            launch[1] = (horizontal.normalized * launchSpeed * Mathf.Cos(45.0f * Mathf.Deg2Rad)) - (gravityBase.normalized * launchSpeed * Mathf.Sin(45.0f * Mathf.Deg2Rad));

        }
        else
        {
            Debug.Log("Voidaan osua kohteeseen. Lasketaan kulmat, joissa ammutaan. ");
            float[] tanAngle = new float[2];

            tanAngle[0] = (v2 - Mathf.Sqrt(v4 - gravMag * ((gravMag * x2) + (2 * verticalDistance * v2)))) / (gravMag * horizontalDistance);
            tanAngle[1] = (v2 + Mathf.Sqrt(v4 - gravMag * ((gravMag * x2) + (2 * verticalDistance * v2)))) / (gravMag * horizontalDistance);


            // Lasketaan kulmat
            float[] finalAngle = new float[2];

            finalAngle[0] = Mathf.Atan(tanAngle[0]);
            finalAngle[1] = Mathf.Atan(tanAngle[1]);

            Debug.Log("Kulmat joihin tykki ampuu ovat: " + finalAngle[0] * Mathf.Rad2Deg + " ja " + finalAngle[1] * Mathf.Rad2Deg);

            launch[0] = (horizontal.normalized * launchSpeed * Mathf.Cos(finalAngle[0])) - (gravityBase.normalized * launchSpeed * Mathf.Sin(finalAngle[0]));
            launch[1] = (horizontal.normalized * launchSpeed * Mathf.Cos(finalAngle[1])) - (gravityBase.normalized * launchSpeed * Mathf.Sin(finalAngle[1]));



        }

        return launch;

    }

    public Vector3 GetHorizontalVector(Vector3 AtoB, Vector3 gravityBase, Vector3 startPos)
    {
        Vector3 output;
        // laskua....
        Vector3 perpendicular = Vector3.Cross(AtoB, gravityBase); // Nyt perpendicular osottaa nenään
        perpendicular = Vector3.Cross(gravityBase, perpendicular); // Nyt perpendicular osoittaa horizontaaliseen suuntaan.
        output = Vector3.Project(AtoB, perpendicular); // output on horizontaalinen vektori kohteeseen. 
        Debug.DrawRay(startPos, output, Color.blue, 10f);
        return output;
    }

    public Vector3 GetVerticalVector(Vector3 AtoB, Vector3 gravityBase, Vector3 startPos)
    {
        Vector3 output;
        output = Vector3.Project(AtoB, gravityBase);
        Debug.DrawRay(startPos, output, Color.green, 10f);
        return output;
    }





}
