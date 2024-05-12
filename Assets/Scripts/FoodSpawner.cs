using UnityEngine;

using System.Collections;

public class FoodSpawner : ObjectSpawner
{
    public GameObject foodPrefab;
    public int spawnedFood = 0;
    public int maxFoodCapacity = 1000;
    public float feedingTime = 5f;

    private void Awake()
    {
        prefab = foodPrefab; 
    }
    private void Start()
    {
        StartCoroutine(SpawnFoodOverTime());
    }
    protected override void HandleSpawnedObject(GameObject obj, int index)
    {
        
        obj.name = "Food" + index;
        obj.layer = 3;
    }
    IEnumerator SpawnFoodOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(feedingTime * Random.Range(0.8f, 1.2f));
            SpawnObjects();
        }
    }
}
