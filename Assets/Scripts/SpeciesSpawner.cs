using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpeciesSpawner : ObjectSpawner
{
    public SpeciesManager speciesManager;
    public GameObject speciesManagerPrefab;
    public GameObject organismPrefab;

    public OrganismObjectPool organismPool;

    private void Awake()
    {
        Physics2D.queriesStartInColliders = false;
        organismPool = FindAnyObjectByType<OrganismObjectPool>();

        speciesManager = FindObjectOfType<SpeciesManager>();
        if (!speciesManager)
        {
            speciesManager = Instantiate(speciesManagerPrefab).GetComponent<SpeciesManager>();
            speciesManager.AddDefaultSpecies();
        }
        organismPool.organismPrefab = organismPrefab; 
    }
    private void Start()
    {
        SpawnSpecies();
    }
    void SpawnSpecies()
    {
        List<Species> speciesToSpawn = speciesManager.createdSpecies;
        foreach (Species species in speciesToSpawn)
        {
            int speciesCount = species.Count;
            string speciesGenomCode = species.GenomCode;
            string speciesName = species.SpeciesName;

            for (int i = 0; i < speciesCount; i++)
            {
                GameObject newOrganism = organismPool.GetOrganism();
                newOrganism.transform.position = GetRandomPosition();
                newOrganism.layer = 3;
                newOrganism.tag = "organism";


                Organism organism = newOrganism.GetComponentInChildren<Organism>();
                //organism.SetCode(speciesGenomCode);
                //organism.genom.GenerateGenom(speciesGenomCode);
                //organism.SpecifyOrganism(speciesGenomCode);
                organism.WakeUp(speciesGenomCode);
                newOrganism.GetComponent<ReproductionSystem>().SetFirstGeneration(speciesName, speciesGenomCode);
                
            }
        }
    }

}
