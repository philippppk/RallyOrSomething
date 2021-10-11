using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killAfterTime : MonoBehaviour
{
    public float timer;
    // destroys game object after an amount of time
    void Start()
    {
        timer = 5f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            Destroy(gameObject);
        }
    }
}
