using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carMiniController : MonoBehaviour
{
    //controls the game modes
    public gameController gc;
    public BoxCollider bc;
    public Rigidbody rb;
    public GameObject eatFX;
    private Vector3 eatSize;
    void Start()
    {
        GetComponent<BoxCollider>();
        eatSize = bc.size += new Vector3(1, 0, 1); ;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && gc.gameMode ==2 )
        {
            flight();
        }
        updateBox();
    }
    
    void updateBox()
    {
        if(gc.gameMode == 1)
        {
            bc.size = eatSize;
        }
    }

    void flight()
    {
        Debug.Log("it workin");
        rb.AddForce(Vector3.up * 1000000);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(gc.gameMode == 1)
        {
            switch (other.tag)
            {
                case "en":
                    Instantiate(eatFX, other.gameObject.transform.position, Quaternion.identity);
                    Destroy(other.gameObject);
                    gc.addTime(1f);
                    gc.updateScore(100);
                    break;
                case "animal":
                    Instantiate(eatFX, other.gameObject.transform.position, Quaternion.identity);
                    Destroy(other.gameObject);
                    gc.addTime(5f);
                    gc.updateScore(100);
                    break;
            }
        }
    }
}
