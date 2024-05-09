using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectSpawner : MonoBehaviour
{
    public int numberOfObjects = 100;
    public float maxDistanceFromCenter = 100f;
    protected GameObject prefab;

    protected abstract void HandleSpawnedObject(GameObject obj, int index); 

    void Start()
    {
    }

    protected void SpawnObjects()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            GameObject newObject = Instantiate(prefab, transform.position, Quaternion.identity);
            newObject.transform.position = GetRandomPosition();
            HandleSpawnedObject(newObject, i);
        }
    }

    protected Vector3 GetRandomPosition()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(0f, maxDistanceFromCenter);
        return transform.position + (Vector3)randomDirection * randomDistance;
    }
}
