using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public TMP_Text numberOfStepsLeft;





    public int speed = 300;

    [SerializeField]
    private bool isMoving = false;
    [SerializeField]
    private bool hitWall = false;


    [SerializeField]
    private Vector2 playerCoords;


    //public ValueChecker valueChecker;

    public int lastValue;

    public int stepsLeft = 1;
    public bool playersTurn = true;
    public bool enemysTurn = false;


    // Start is called before the first frame update
    void Start()
    {

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

        if (stepsLeft < 1)
        {
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
            StartCoroutine(RotateCube(Vector3.right));
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(RotateCube(Vector3.left));
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(RotateCube(Vector3.forward));
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
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


        //valueChecker.transform.position = new Vector3(playerCoords.x, 10, playerCoords.y);

        isMoving = false;

    }

    private IEnumerator EnemyTurn()
    {
        enemysTurn = true;

        Debug.Log("Enemy's turn!");

        yield return new WaitForSeconds(2);

        playersTurn = true;
        enemysTurn = false;

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






}
