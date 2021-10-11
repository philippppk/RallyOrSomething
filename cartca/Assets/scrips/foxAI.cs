using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foxAI : MonoBehaviour
{

    private Rigidbody rb;
    private GameObject player;
    private animalSpawn spawner;
    public GameObject deer;
    public Vector3 nextNode;
    public Vector3 dir;
    public bool escaping;

    public float spd;
    // Start is called before the first frame update
    void Start()
    {
        spawner = GameObject.Find("animalSpawners").GetComponent<animalSpawn>();
        player = GameObject.Find("Trans Am");
        rb = GetComponent<Rigidbody>();
        getNextPos();
    }

    // Update is called once per frame
    void Update()
    {
        move();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            getNextPos();
        }

        if (nextNode.x - 1 <= transform.position.x && transform.position.x <= nextNode.x + 1)
        {
            getNextPos();
        }

        if (transform.position.y < -3)
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            spawner.spawnAnimal(2);
            Destroy(transform.parent.GetChild(0).gameObject);
            Destroy(transform.parent.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            spawner.spawnAnimal(2);
            Destroy(transform.parent.GetChild(0).gameObject);
            Destroy(transform.parent.gameObject);
        }
    }

    void move()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > 30)
        {
            escaping = false;
            dir = nextNode - transform.position;
            dir = dir.normalized;
            rb.AddForce(dir * 15);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, 5);
        }
        else
        {
            runAway();
        }
    }

    void runAway()
    {
        escaping = true;
        dir = -(player.transform.position - transform.position);
        dir = dir.normalized;
        rb.AddForce(dir * 25);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, 15);
    }
    void getNextPos()
    {
        nextNode = transform.position + RandomPointOnCircleEdge(10);
    }
    private Vector3 RandomPointOnCircleEdge(float radius)
    {
        var vector2 = Random.insideUnitCircle.normalized * radius;
        return new Vector3(vector2.x, 0, vector2.y);
    }
}
