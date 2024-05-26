using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReproductionSystem : MonoBehaviour
{
    public int collectedFood;
    public int requiredFood;

    public float reproductionCost;
    //public float mutationFactor = 10f;

    public int generation;
    public OrganismObjectPool organismPool;

    public AttackSystem attackSystem;

    // Start is called before the first frame update
    void Start()
    {
        organismPool = FindAnyObjectByType<OrganismObjectPool>();
        collectedFood = 0;
        //mutationFactor = 90f;
        if (attackSystem)
        {
            attackSystem.SafeReproduce();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Reproduce()
    {
        Genom childGenom = GetComponent<Organism>().genom;
        if (attackSystem) attackSystem.SafeReproduce();

        GameObject newOrganism = organismPool.GetOrganism();

        newOrganism.transform.position = transform.position + Vector3.up;
        newOrganism.layer = 3;
        newOrganism.name = this.name + generation;
        newOrganism.tag = "organism";
        newOrganism.GetComponent<ReproductionSystem>().generation = generation + 1;
        //newOrganism.GetComponent<Genom>().Mutate(mutationFactor);

        newOrganism.GetComponentInChildren<Organism>().WakeUp(childGenom.code);
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
