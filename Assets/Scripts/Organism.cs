using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;

//organizm posiada genom i swój generator, który go buduje
public class Organism : MonoBehaviour
{
    public Genom genom;
    public int numberOfCells;
    public OrganismSpriteGenerator spriteGenerator;
    public StaminaSystem staminaSystem;
    public ReproductionSystem reproductionSystem;

    public FoodObjectPool foodPool;
    public OrganismObjectPool organismPool;
    public BodyPartObjectPool bodyPartPool;
    public List<GameObject> bodyParts = new List<GameObject>();
    void Awake()
    {
        foodPool = FindAnyObjectByType<FoodObjectPool>();
        organismPool = FindAnyObjectByType<OrganismObjectPool>();
        bodyPartPool = FindAnyObjectByType<BodyPartObjectPool>();
        spriteGenerator = FindAnyObjectByType<OrganismSpriteGenerator>();
    }
    

    public void WakeUp(string genomCode)
    {
        genom.GenerateGenom(genomCode);
        SpecifyOrganism(genom);
        staminaSystem.Run(genom.CalculateEnergyCost());
        reproductionSystem.UpdateAncestralGenomes(genomCode);
        //GetComponent<Mover>().TrailDelay();

    }
    public void SpecifyOrganism(Genom genom)
    {
        Debug.Log(genom.code);
        spriteGenerator.GenerateBodyObjects(genom, gameObject);
        reproductionSystem.requiredFood = genom.CalculateRequiredFood();
        
    }

    

    public void Die()
    {
        GameObject food = foodPool.GetFood();
        foodPool.HandleFood(food);
        food.transform.position = transform.position;

        foreach (GameObject child in bodyParts)
        {
            bodyPartPool.ReturnBodyPart(child);
        }

        bodyParts.Clear();
        genom.ResetGenom();
        organismPool.ReturnOrganism(this.gameObject);
    }



}
