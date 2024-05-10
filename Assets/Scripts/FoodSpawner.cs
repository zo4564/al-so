using UnityEngine;

public class FoodSpawner : ObjectSpawner
{
    public GameObject foodPrefab; 

    private void Awake()
    {
        prefab = foodPrefab; 
    }
    private void Start()
    {
        SpawnObjects();
    }
    protected override void HandleSpawnedObject(GameObject obj, int index)
    {
        
        obj.name = "Food" + index;
        obj.layer = 3;
    }
}
