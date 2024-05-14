using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public float expirationTime;
    public FoodObjectPool foodPool;

    void Start()
    {
        foodPool = FindAnyObjectByType<FoodObjectPool>();
        expirationTime = Random.Range(3f, 20f);
        StartCoroutine(DestroyAfterTime());
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(expirationTime);
        foodPool.ReturnFood(this.gameObject);
    }



}
