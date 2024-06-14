using UnityEngine;

using System.Collections;
using UnityEngine.UI;

public class FoodSpawner : ObjectSpawner
{
    public FoodObjectPool foodPool;
    public int spawnedFood = 0;
    public int maxFoodCapacity = 1000;
    public float feedingTime = 50f;
    public int feedingAmount = 10;
    public Slider foodSlider;

    private void Start()
    {
        StartCoroutine(SpawnFoodOverTime());
    }
    IEnumerator SpawnFoodOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(feedingTime);
            for(int i = 0; i < feedingAmount; i++)
            {
                GameObject food = foodPool.GetFood();
                food.transform.position = GetRandomPosition();
                foodPool.HandleFood(food);
                    
            }
        }
    }
    public void SetFeedingAmout()
    {
        feedingAmount = (int)foodSlider.value;
    }
}
