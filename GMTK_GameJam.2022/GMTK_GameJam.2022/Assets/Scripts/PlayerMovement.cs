using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int speed = 300;

    [SerializeField]
    private bool isMoving = false;
    [SerializeField]
    private bool hitWall = false;


    [SerializeField]
    private Vector2 playerCoords;


    public ValueChecker valueChecker;

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
        if (isMoving)
            return;

        if (stepsLeft < 1)
            playersTurn = false;

        if (!playersTurn && !enemysTurn)
        {
            StartCoroutine(EnemyTurn());
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

        if(!hitWall)
            stepsLeft -= 1;

        hitWall = false;


        //yield return null;
        //valueChecker.transform.position = new Vector3(playerCoords.x, 1.5f, playerCoords.y); yield return null;
        //Debug.Log(valueChecker.lastValue); yield return null;
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

        stepsLeft = 1;

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



}
