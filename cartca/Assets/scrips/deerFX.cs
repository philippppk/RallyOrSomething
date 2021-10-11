using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deerFX : MonoBehaviour
{
    private deerAI ai;
    private GameObject stagMove;
    private Vector3 delta;

    private Transform angle;
    // Start is called before the first frame update
    void Start()
    {
        stagMove = transform.parent.GetChild(1).gameObject;
        ai = transform.parent.gameObject.GetComponentInChildren<deerAI>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = stagMove.transform.position;
        updateVisuals();
    }


    void updateVisuals()
    {
        //transform.rotation = Quaternion.LookRotation();
        //transform.LookAt(ai.nextNode);

        if (ai.escaping == false)
        {
            Vector3 lTargetDir = ai.nextNode - transform.position;
            lTargetDir.y = 0.0f;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lTargetDir), Time.time * .25f);
        }
        else
        {
            Vector3 lTargetDir = ai.dir;
            lTargetDir.y = 0.0f;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lTargetDir), Time.time * .25f);
        }
   
    }
}
