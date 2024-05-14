using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Producer : MonoBehaviour
{
    public FoodObjectPool foodPool;
    public float productionTime = 10f;
    public Vector3 position;
    public Vector3 direction;
    public float foodDistance;
    // Start is called before the first frame update
    void Start()
    {
        foodPool = FindAnyObjectByType<FoodObjectPool>();
        position = transform.position;
        direction = transform.up;
        foodDistance = Random.Range(5f, 25f);
        StartCoroutine(SpawnFoodOverTime());

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator SpawnFoodOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(productionTime);

                foodDistance = Random.Range(1f, 5f);
                GameObject food = foodPool.GetFood();
                food.transform.position = GetFoodPosition();
                foodPool.HandleFood(food);

        }
    }
    public Vector3 GetFoodPosition()
    {
        return transform.position + direction * foodDistance;;
    }
}
