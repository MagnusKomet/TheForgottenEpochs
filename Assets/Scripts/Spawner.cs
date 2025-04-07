using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float minTime = 15f;
    public float maxTime = 30f;
    public float nextSpawnTime;

    // Start is called before the first frame update
    public void Start()
    {
        SetNextSpawnTime();
    }

    // Update is called once per frame
    public void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnObject();
            SetNextSpawnTime();
        }
    }

    public void SetNextSpawnTime()
    {
        nextSpawnTime = Time.time + Random.Range(minTime, maxTime);
    }

    public virtual void SpawnObject()
    {
        Instantiate(objectToSpawn, transform.position, transform.rotation);
    }
}
