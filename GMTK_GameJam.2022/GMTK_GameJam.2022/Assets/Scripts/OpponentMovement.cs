using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentMovement : MonoBehaviour
{
    public GameObject projectile;


    private int maxHP = 3;
    public int currentHP;




    public int speed = 300;

    [SerializeField]
    private bool isMoving = false;
    [SerializeField]
    private bool hitWall = false;


    [SerializeField]
    private Vector2 playerCoords;


    private int lastKeyPressed;

    public GameObject gun;

    public GameObject gunStartPos;
    public GameObject gunEndPos;



    public bool isShooting;


    public int lastValue;

    public int stepsLeft = 1;
    //public bool playersTurn = true;
    //public bool enemysTurn = false;

    public bool hasCompletedTurn = true;



    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHP <= 0)
            Destroy(gameObject);

        if (isMoving)
            return;

        if (hasCompletedTurn)
            return;



        if (stepsLeft < 1 && !isShooting)
        {
            StartCoroutine(Shoot());
            //playersTurn = false;
        }

        if (hasCompletedTurn)
            return;

        //if (!playersTurn)
        //{
        //    if (!enemysTurn)
        //    {
        //        StartCoroutine(EnemyTurn());
        //    }
        //    return;
        //}


        int moveDir = Random.Range(1, 4);


        switch (moveDir)
        {
            case 1:
                {
                    gun.transform.eulerAngles = new Vector3(0, 90, 0);

                    StartCoroutine(RotateCube(Vector3.right));
                    break;
                }
            case 2:
                {
                    gun.transform.eulerAngles = new Vector3(0, -90, 0);

                    StartCoroutine(RotateCube(Vector3.left));
                    break;
                }
            case 3:
                {
                    gun.transform.eulerAngles = new Vector3(0, 0, 0);

                    StartCoroutine(RotateCube(Vector3.forward));
                    break;
                }
            case 4:
                {
                    gun.transform.eulerAngles = new Vector3(0, 180, 0);

                    StartCoroutine(RotateCube(Vector3.back));
                    break;
                }
            default:
                {
                    gun.transform.eulerAngles = new Vector3(0, 90, 0);

                    StartCoroutine(RotateCube(Vector3.right));
                    break;
                }
        }


    }




    private IEnumerator RotateCube(Vector3 direction)
    {
        isMoving = true;

        yield return new WaitForSeconds(Random.Range(1, 2));


        float remainingAngle = 90;
        Vector3 rotationCenter = transform.position + direction / 2 + Vector3.down / 2;
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);


        //start roll

        while (remainingAngle > 0)
        {
            if (hitWall)
            {
                break;
            }
            //Debug.Log("yes");


            float rotationAngle = Mathf.Min(Time.deltaTime * speed, remainingAngle);
            transform.RotateAround(rotationCenter, rotationAxis, rotationAngle);
            remainingAngle -= rotationAngle;
            yield return null;
        }

        //if touched a wall reverse roll

        if (hitWall)
        {
            rotationAxis = Vector3.Cross(Vector3.up, -direction);

            while (remainingAngle < 90)
            {
                //Debug.Log("no");

                float rotationAngle = Mathf.Min(Time.deltaTime * speed, remainingAngle);
                transform.RotateAround(rotationCenter, rotationAxis, rotationAngle);
                remainingAngle += rotationAngle;
                yield return null;
            }

            //snap to position to avoid weird situations

            var rot = transform.eulerAngles;
            rot.x = Mathf.Round(rot.x / 90) * 90;
            rot.y = Mathf.Round(rot.y / 90) * 90;
            rot.z = Mathf.Round(rot.z / 90) * 90;
            transform.eulerAngles = rot;

            var pos = transform.position;
            pos.x = Mathf.Round(pos.x / 1) * 1;
            pos.y = Mathf.Round(pos.y / 1) * 1;
            pos.z = Mathf.Round(pos.z / 1) * 1;
            transform.position = pos;


        }

        if (!hitWall)
            stepsLeft -= 1;

        hitWall = false;


        //yield return null;
        yield return new WaitForEndOfFrame();


        CastRayValueCheck();
        //Debug.Log(lastValue);


        yield return new WaitForEndOfFrame();


        gun.transform.position = new Vector3(playerCoords.x, 1.01f, playerCoords.y);

        isMoving = false;

    }



    public IEnumerator Shoot()
    {
        isShooting = true;
        Debug.Log("PIU!!!");


        GameObject projectileInstance = Instantiate(projectile, gunStartPos.transform.position, new Quaternion(0, 0, 0, 0));

        gunEndPos.transform.localPosition = new Vector3(0, 0, lastValue);

        projectileInstance.GetComponent<Projectlile>().endPos = gunEndPos.transform.position;

        projectileInstance.GetComponent<Projectlile>().allowMovement = true;


        hasCompletedTurn = true;


        yield return null;
    }



    public void CastRayValueCheck()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.up, out hit, 15f, ~9))
        {
            Debug.DrawRay(transform.position, Vector3.up * 100, Color.yellow);

            lastValue = hit.collider.gameObject.GetComponent<QuadValue>().quadValue;

            //Debug.Log("hit");
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.up * 100, Color.green);
        }
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cell"))
        {
            playerCoords = other.gameObject.GetComponent<Cell>().GetCoords();
            //Debug.Log("Touch!");
            //Debug.Log(playerCoords);
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            //Debug.Log("BAM");
            hitWall = true;
        }
        else if (other.gameObject.CompareTag("Bullet"))
        {
            currentHP -= 1;
        }



    }
}
