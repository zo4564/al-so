using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;

//organizm posiada genom i sw�j generator, kt�ry go buduje
public class Organism : MonoBehaviour
{
    public Genom genom;
    public string organismCode; 
    public int numberOfCells;
    public OrganismSpriteGenerator spriteGenerator;
    public StaminaSystem staminaSystem;
    public ReproductionSystem reproductionSystem;

    public FoodObjectPool foodPool;
    public OrganismObjectPool organismPool;
    public List<GameObject> bodyParts = new List<GameObject>();
    void Start()
    {
        foodPool = FindAnyObjectByType<FoodObjectPool>();
        organismPool = FindAnyObjectByType<OrganismObjectPool>();
    }
    
    public void SetCode(string genomCode)
    {
        organismCode = genomCode;
    }
    public void InitializeGenom()
    {
        
        //Debug.Log(genom);
    }

    public void GenerateOrganism()
    {
        spriteGenerator = FindAnyObjectByType<OrganismSpriteGenerator>();
        
        spriteGenerator.GenerateBodyObjects(genom, gameObject);
        
    }
    public void SpecifyOrganism(string genomCode)
    {
        SetCode(genomCode);

        genom.SetMutationFactor(10);
        genom.GenerateGenom(organismCode);

        GenerateOrganism();

        reproductionSystem.requiredFood = genom.CalculateRequiredFood();
        
    }
    
    public void Die()
    {
        GameObject food = foodPool.GetFood();
        foodPool.HandleFood(food);
        food.transform.position = transform.position;
        organismPool.ReturnOrganism(gameObject);
    }



}
