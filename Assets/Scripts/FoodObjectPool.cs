using System.Collections.Generic;
using UnityEngine;

public class FoodObjectPool : MonoBehaviour
{
    public GameObject foodPrefab;
    public int poolSize = 20;
    private Queue<GameObject> foodPool = new Queue<GameObject>();

    private void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject food = Instantiate(foodPrefab);
            food.SetActive(false);
            foodPool.Enqueue(food);
        }
    }

    public GameObject GetFood()
    {
        if (foodPool.Count == 0)
        {
            GameObject food = Instantiate(foodPrefab);
            return food;
        }
        else
        {
            GameObject food = foodPool.Dequeue();
            food.SetActive(true);
            return food;
        }
    }
    public void HandleFood(GameObject food)
    {
        food.name = "Food";
        food.layer = 6;
        food.tag = "food";
    }

    public void ReturnFood(GameObject food)
    {
        food.SetActive(false);
        foodPool.Enqueue(food);
    }
}
