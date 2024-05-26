using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;

//organizm posiada genom i swój generator, który go buduje
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
    public void WakeUp(string genomCode)
    {
        SetCode(genomCode);
        genom.GenerateGenom(genomCode);
        SpecifyOrganism(genomCode);
    }
    public void SpecifyOrganism(string genomCode)
    {
        // to te¿ chyba tylko za pierwszym razem: SetCode(genomCode);

        genom.SetMutationFactor(10);
        // to powinno byæ tylko przy pierwszej generacji: genom.GenerateGenom(organismCode);
        //genom.Mutate();
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
