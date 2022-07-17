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


    public SphereCollider bulletCollider;



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

            if (elapsedTime > duration / 2)
                bulletCollider.enabled = true;

            if (elapsedTime > duration)
                Destroy(gameObject);

        }



    }



}
