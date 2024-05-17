using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReproductionSystem : MonoBehaviour
{
    public int collectedFood;
    public int requiredFood;

    public float reproductionCost;
    public float mutationFactor = 10f;

    public int generation;
    public OrganismObjectPool organismPool;

    // Start is called before the first frame update
    void Start()
    {
        organismPool = FindAnyObjectByType<OrganismObjectPool>();
        collectedFood = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Reproduce()
    {
        Genom childGenom = GetMutatedGenom(GetComponent<Organism>().genom);

        GameObject newOrganism = organismPool.GetOrganism();

        newOrganism.transform.position = transform.position + Vector3.up;
        newOrganism.layer = 3;
        newOrganism.name = this.name + generation;
        newOrganism.tag = "organism";
        newOrganism.GetComponent<ReproductionSystem>().generation = generation + 1;

        newOrganism.GetComponentInChildren<Organism>().SpecifyOrganism(childGenom.code);
    }
    public Genom GetMutatedGenom(Genom parentGenom)
    {
        Genom newGenom = new Genom(parentGenom, mutationFactor);
        return newGenom;
    }
    public bool CheckIfReady()
    {
        if (collectedFood > requiredFood)
        {
            return true;
        }
        return false;
    }
    
}
