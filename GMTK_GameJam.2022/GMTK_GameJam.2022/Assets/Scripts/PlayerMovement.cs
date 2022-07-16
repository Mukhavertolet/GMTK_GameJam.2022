using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W) && transform.position.x != 0)
        {
            transform.Translate(-1, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.A) && transform.position.z != 0)
        {
            transform.Translate(0, 0, -1);
        }
        if (Input.GetKeyDown(KeyCode.S) && transform.position.x != 6)
        {
            transform.Translate(1, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.D) && transform.position.z != 6)
        {
            transform.Translate(0, 0, 1);
        }
    }
}
