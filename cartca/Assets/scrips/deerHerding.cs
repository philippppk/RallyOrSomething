using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deerHerding : MonoBehaviour
{
    private deerAI deer;
    void Start()
    {
        deer = transform.parent.gameObject.GetComponentInChildren<deerAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("deer"))
        {
            if (Random.Range(0, 100) > 60)
            {
                Debug.Log("Going to other deer : )");
                deer.nextNode = other.transform.position;
            }
            
        }
    }
}
