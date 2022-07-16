using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadValue : MonoBehaviour
{

    public int quadValue;



    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Getting...");
        if (other.gameObject.CompareTag("ValueChecker"))
            other.gameObject.GetComponent<ValueChecker>().lastValue = quadValue;
    }

}
