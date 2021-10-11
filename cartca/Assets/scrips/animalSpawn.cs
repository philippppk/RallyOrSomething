using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animalSpawn : MonoBehaviour
{
    public GameObject[] spawner;
    public GameObject deer;
    public GameObject fox;
    private GameObject animalToSpawn;
    void Start()
    {
        int x = 1;
        foreach (Transform child in transform)
        {
            spawner[x-1] = child.gameObject;
            x++;
        }
    }

    public void spawnAnimal(int animal)
    {
        switch (animal)
        {
            case 1:
                animalToSpawn = deer;
                break;
            case 2:
                animalToSpawn = fox;
                break;
        }
        int x = Random.Range(0, 22);
        Debug.Log("Spawning: " + animal.ToString() + " at spawer " + x);
        Instantiate(animalToSpawn, spawner[x].gameObject.transform.position,Quaternion.identity);
    }
}
