using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public GameObject cell;

    public Vector2 coords;
    public string type;

    public GameObject wall;



    private void Awake()
    {
        cell = gameObject;
    }


    // Start is called before the first frame update
    void Start()
    {
        coords = new Vector2(cell.transform.position.x, cell.transform.position.z);

        if (coords == new Vector2(0, 6) || coords == new Vector2(6, 0))
            return;

        int wallChance = Random.Range(1, 7);
        if(wallChance == 1)
        {
            Instantiate(wall, new Vector3(coords.x, 0.75f, coords.y), Quaternion.identity);
        }


    }


    public Vector2 GetCoords()
    {
        return coords;
    }



}
