using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadValue : MonoBehaviour
{

    public int quadValue;

    //public void CastRayValueCheck()
    //{
    //    RaycastHit hit;

    //    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, 15f, ~10))
    //    {
    //        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
    //        Debug.Log("hit");
    //    }
    //    else
    //    {
    //        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 100, Color.green);
    //    }
    //}

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.H))
    //    {
    //        CastRayValueCheck();
    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("ValueChecker"))
    //    {
    //        Debug.Log("Getting...");
    //        other.gameObject.GetComponent<ValueChecker>().lastValue = quadValue;
    //    }
    //}


    //public void CastRayCheck()
    //{

    //}

}
