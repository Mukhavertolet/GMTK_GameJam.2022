using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectlile : MonoBehaviour
{
    public AnimationCurve curve;

    public Vector3 startPos;
    public Vector3 endPos;

    public float duration = 1f;
    private float elapsedTime;

    public float parabolaHeight;

    public bool allowMovement = false;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("HEY HEY!!");
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (allowMovement)
        {
            elapsedTime += Time.deltaTime;

            float percentageComplete = elapsedTime / duration;

            Vector3 pos = Vector3.Lerp(startPos, endPos, percentageComplete);

            pos.y += curve.Evaluate(percentageComplete);

            transform.position = pos;


            //elapsedTime += Time.deltaTime;
            //float percentageComplete = elapsedTime / duration;

            //transform.position.Set(transform.position.x, transform.position.y + curve.Evaluate(percentageComplete), transform.position.z);


            //transform.position = Vector3.Lerp(startPos, endPos, percentageComplete);
        }


        //transform.position += new Vector3(Mathf.Lerp(startPos.x, endPos.x, speed * Time.deltaTime), Mathf.Lerp(transform.position.y, parabolaHeight, speed * Time.deltaTime), Mathf.Lerp(startPos.z, endPos.z, speed * Time.deltaTime));

        //transform.position += Vector3.SmoothDamp(transform.position, endPos, ref )

    }



}
