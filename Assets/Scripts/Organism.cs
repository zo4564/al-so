using System.Collections;
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

    public FoodObjectPool foodPool;
    public OrganismObjectPool organismPool;
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
        genom = new Genom(organismCode);
        //Debug.Log(genom);
    }

    public void GenerateOrganism()
    {
        spriteGenerator = FindAnyObjectByType<OrganismSpriteGenerator>();
        
        spriteGenerator.GenerateBodyObjects(genom, this.gameObject);
        
    }
    public void SpecifyOrganism(string genomCode)
    {
        SetCode(genomCode);

        InitializeGenom();
        GenerateOrganism();
        
    }
    
    public void Die()
    {
        GameObject food = foodPool.GetFood();
        foodPool.HandleFood(food);
        food.transform.position = transform.position;
        organismPool.ReturnOrganism(this.gameObject);
    }


}
