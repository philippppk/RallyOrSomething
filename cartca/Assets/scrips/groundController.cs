using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundController : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject dirtVFX;
    public GameObject rightWheel;
    public GameObject leftWheel;
    private ParticleSystem spark;
    private ParticleSystem spark2;
    private GameObject vfx;
    private GameObject vfx2;

    private bool onDirt = false;
    // Start is called before the first frame update
    void Start()
    {
        vfx = Instantiate(dirtVFX, leftWheel.transform.position, Quaternion.identity);
        vfx2 = Instantiate(dirtVFX, rightWheel.transform.position, Quaternion.identity);
        spark = vfx.GetComponent<ParticleSystem>();
        spark2 = vfx2.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
       if (onDirt == true)
        {
            vfx.gameObject.transform.position = leftWheel.transform.position;
            vfx2.gameObject.transform.position = rightWheel.transform.position;

        }else if(onDirt == false)
        {
            spark.Play();
            spark2.Play();
        }
        Debug.Log(rb.velocity);
        if (Mathf.Abs(rb.velocity.x) <= 2.0f && Mathf.Abs(rb.velocity.y) <= 2.0f)
        {
            var emission = spark.emission;
            emission.rateOverTime = 0.1f;
            var emission2 = spark2.emission;
            emission2.rateOverTime = 0.1f;
        }
        else
        {
            var emission = spark.emission;
            emission.rateOverTime = 72.19f;
            var emission2 = spark2.emission;
            emission2.rateOverTime = 72.19f;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("dirt"))
        {
            onDirt = true;
            Debug.Log("on");
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("dirt"))
        {
            onDirt = false;
            Debug.Log("off");
        }
    }
}
