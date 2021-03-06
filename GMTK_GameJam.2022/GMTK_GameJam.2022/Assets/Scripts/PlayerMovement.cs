using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public AudioManager audioManager;

    public GameObject youLost;



    public TMP_Text numberOfStepsLeft;
    public TMP_Text leftHP;



    public OpponentMovement opponent;
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


    public GameObject gun;

    public GameObject gunStartPos;
    public GameObject gunEndPos;



    private bool isShooting;


    public int lastValue;

    public int stepsLeft = 1;
    public bool playersTurn = true;
    public bool enemysTurn = false;

    private int loseValue = 0;


    public GameObject hitParticles;


    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);

        //if(loseValue >= 4)
        //{
        //    Debug.Log("YOU LOST!!!!!!!!!!!!!");
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //}

        if (currentHP <= 0)
        {
            audioManager.Play("Death");
            youLost.SetActive(true);
            Destroy(gameObject);
        }

        //number of steps text
        numberOfStepsLeft.text = stepsLeft.ToString();
        leftHP.text = currentHP.ToString();



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

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            gun.transform.eulerAngles = new Vector3(0, 90, 0);

            StartCoroutine(RotateCube(Vector3.right));
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            gun.transform.eulerAngles = new Vector3(0, -90, 0);

            StartCoroutine(RotateCube(Vector3.left));
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            gun.transform.eulerAngles = new Vector3(0, 0, 0);

            StartCoroutine(RotateCube(Vector3.forward));
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            gun.transform.eulerAngles = new Vector3(0, 180, 0);

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
                loseValue += 1;
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

        audioManager.Play("CubeRoll");

        isMoving = false;

    }

    private IEnumerator EnemyTurn()
    {
        enemysTurn = true;

        opponent.hasCompletedTurn = false;


        Debug.Log("Enemy's turn!");

        while (!opponent.hasCompletedTurn)
            yield return new WaitForSeconds(1);

        playersTurn = true;
        enemysTurn = false;

        isShooting = false;
        opponent.isShooting = false;


        opponent.stepsLeft = opponent.lastValue;
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
        else if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("PlayerLayerMask"))
        {
            //Debug.Log("BAM");
            hitWall = true;
        }
        else if (other.gameObject.CompareTag("Bullet"))
        {
            Instantiate(hitParticles, transform.position, Quaternion.identity);
            audioManager.Play("BulletHit");
            currentHP -= 1;
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


        GameObject projectileInstance = Instantiate(projectile, gunStartPos.transform.position, new Quaternion(0, 0, 0, 0));

        audioManager.Play("BulletShoot");


        gunEndPos.transform.localPosition = new Vector3(0, 0, lastValue);

        projectileInstance.GetComponent<Projectlile>().endPos = gunEndPos.transform.position;

        projectileInstance.GetComponent<Projectlile>().allowMovement = true;


        yield return null;
    }




}
