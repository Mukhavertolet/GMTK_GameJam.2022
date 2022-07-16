using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public GameObject cell;

    public Vector2 coords;
    public string type;




    private void Awake()
    {
        cell = gameObject;

        
    }


    // Start is called before the first frame update
    void Start()
    {
        coords = new Vector2(cell.transform.position.x, cell.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
