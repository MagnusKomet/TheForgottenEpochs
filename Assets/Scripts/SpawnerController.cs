using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float minTime = 15f;
    public float maxTime = 30f;
    public float nextSpawnTime;
    public bool justOneSpawn = true;
    GameObject lastSpawnedObject;

    // Start is called before the first frame update
    public void Start()
    {
        SpawnObject();
        SetNextSpawnTime();
    }

    // Update is called once per frame
    public void Update()
    {
        if (Time.time >= nextSpawnTime && lastSpawnedObject == null || Time.time >= nextSpawnTime * 3 && lastSpawnedObject != null)
        {
            if (lastSpawnedObject == null || !justOneSpawn)
            {
                SpawnObject();
                SetNextSpawnTime();
            }
        }
    }

    public void SetNextSpawnTime()
    {
        nextSpawnTime = Time.time + Random.Range(minTime, maxTime);
    }

    public virtual void SpawnObject()
    {
        lastSpawnedObject = Instantiate(objectToSpawn, transform.position, transform.rotation);
    }
}
