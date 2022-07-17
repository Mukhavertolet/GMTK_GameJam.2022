using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public TMP_Text numberOfStepsLeft;




    public GameObject projectile;


    public int speed = 300;

    [SerializeField]
    private bool isMoving = false;
    [SerializeField]
    private bool hitWall = false;


    [SerializeField]
    private Vector2 playerCoords;

    //private Vector2 previousPos;
    //private Vector2 currentPos;

    private int lastKeyPressed;

    public GameObject gun;

    public GameObject gunStartPos;
    public GameObject gunEndPos;



    private bool isShooting;


    //public ValueChecker valueChecker;

    public int lastValue;

    public int stepsLeft = 1;
    public bool playersTurn = true;
    public bool enemysTurn = false;


    // Start is called before the first frame update
    void Start()
    {
        //currentPos = new Vector2(0, 6);
        //valueChecker.transform.position = new Vector3(playerCoords.x, 1.5f, playerCoords.y);
        //valueChecker.transform.position = new Vector3(playerCoords.x, 10f, playerCoords.y);


    }

    // Update is called once per frame
    void Update()
    {
        //number of steps text
        numberOfStepsLeft.text = stepsLeft.ToString();


        if (isMoving)
            return;

        if (stepsLeft < 1 && !isShooting)
        {
            StartCoroutine(Shoot());
            playersTurn = false;
        }

        if (!playersTurn)
        {
            if (!enemysTurn)
            {
                StartCoroutine(EnemyTurn());
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            gun.transform.eulerAngles = new Vector3(0, 90, 0);

            lastKeyPressed = 1;
            StartCoroutine(RotateCube(Vector3.right));
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            gun.transform.eulerAngles = new Vector3(0, -90, 0);

            lastKeyPressed = 2;
            StartCoroutine(RotateCube(Vector3.left));
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            gun.transform.eulerAngles = new Vector3(0, 0, 0);

            lastKeyPressed = 3;
            StartCoroutine(RotateCube(Vector3.forward));
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            gun.transform.eulerAngles = new Vector3(0, 180, 0);

            lastKeyPressed = 4;
            StartCoroutine(RotateCube(Vector3.back));
        }

        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    CastRayValueCheck();
        //}

    }

    private IEnumerator RotateCube(Vector3 direction)
    {
        isMoving = true;

        float remainingAngle = 90;
        Vector3 rotationCenter = transform.position + direction / 2 + Vector3.down / 2;
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);


        //start roll

        while (remainingAngle > 0)
        {
            if (hitWall)
            {
                //hitWall = false;
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



        //previousPos = currentPos;
        //currentPos = playerCoords;

        //Debug.Log("previous" + previousPos);
        //Debug.Log("current" + currentPos);

        gun.transform.position = new Vector3(playerCoords.x, 1.01f, playerCoords.y);

        isMoving = false;

    }

    private IEnumerator EnemyTurn()
    {
        enemysTurn = true;

        Debug.Log("Enemy's turn!");

        yield return new WaitForSeconds(2);

        playersTurn = true;
        enemysTurn = false;

        isShooting = false;


        stepsLeft = lastValue;

        Debug.Log("Player's turn!");
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

    public IEnumerator Shoot()
    {
        isShooting = true;
        Debug.Log("PIU!!!");


        Vector3 shootDirection;

        //switch (lastKeyPressed)
        //{
        //    case 1:
        //        {
        //            shootDirection = Vector3.right;
        //            break;
        //        }
        //    case 2:
        //        {
        //            shootDirection = Vector3.left;
        //            break;
        //        }
        //    case 3:
        //        {
        //            shootDirection = Vector3.forward;
        //            break;
        //        }
        //    case 4:
        //        {
        //            shootDirection = Vector3.back;
        //            break;
        //        }
        //    default:
        //        {
        //            shootDirection = Vector3.down;
        //            break;
        //        }
        //}

        //Vector3 shootDirection = new Vector3(currentPos.x - previousPos.x, 0, currentPos.y - previousPos.y); //currentPos - previousPos;

        //Debug.Log(shootDirection);

        GameObject projectileInstance = Instantiate(projectile, gunStartPos.transform.position, new Quaternion(0, 0, 0, 0));

        gunEndPos.transform.localPosition = new Vector3(0, 0, lastValue);

        projectileInstance.GetComponent<Projectlile>().endPos = gunEndPos.transform.position;

        projectileInstance.GetComponent<Projectlile>().allowMovement = true;


        yield return null;
    }




}
