using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foxFX : MonoBehaviour
{
    private foxAI fox;
    private GameObject foxmove;
    void Start()
    {
        foxmove = transform.parent.GetChild(1).gameObject;
        fox = transform.parent.gameObject.GetComponentInChildren<foxAI>();
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = foxmove.transform.position;
        updateVisuals();
    }


    void updateVisuals()
    {
        //transform.rotation = Quaternion.LookRotation();
        //transform.LookAt(ai.nextNode);

        if (fox.escaping == false)
        {
            Vector3 lTargetDir = fox.nextNode - transform.position;
            lTargetDir.y = 0.0f;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lTargetDir), Time.time * .5f);
        }
        else
        {
            Vector3 lTargetDir = fox.dir;
            lTargetDir.y = 0.0f;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lTargetDir), Time.time * .5f);
        }

    }
}
