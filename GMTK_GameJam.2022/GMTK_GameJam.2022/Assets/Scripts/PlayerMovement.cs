using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int speed = 300;

    [SerializeField]
    private bool isMoving = false;

    [SerializeField]
    private Vector2 playerCoords;

    

    // Start is called before the first frame update
    void Start()
    {
          
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
            return;

        if(Input.GetKeyDown(KeyCode.D) && playerCoords.x != 6)
            StartCoroutine(RotateCube(Vector3.right));
        if (Input.GetKeyDown(KeyCode.A) && playerCoords.x != 0)
            StartCoroutine(RotateCube(Vector3.left));
        if (Input.GetKeyDown(KeyCode.W) && playerCoords.y != 6)
            StartCoroutine(RotateCube(Vector3.forward));
        if (Input.GetKeyDown(KeyCode.S) && playerCoords.y != 0)
            StartCoroutine(RotateCube(Vector3.back));



    }

    private IEnumerator RotateCube(Vector3 direction)
    {
        isMoving = true;

        float remainingAngle = 90;
        Vector3 rotationCenter = transform.position + direction / 2 + Vector3.down / 2;
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);

        while(remainingAngle > 0)
        {
            float rotationAngle = Mathf.Min(Time.deltaTime * speed, remainingAngle);
            transform.RotateAround(rotationCenter, rotationAxis, rotationAngle);
            remainingAngle -= rotationAngle;
            yield return null;
        }

        isMoving = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        playerCoords = other.gameObject.GetComponent<Cell>().GetCoords();
        Debug.Log("Touch!");
        Debug.Log(playerCoords);
    }



}
