using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpeciesSpawner : ObjectSpawner
{
    public SpeciesManager speciesManager;
    public GameObject speciesManagerPrefab;
    public GameObject organismPrefab;

    private void Awake()
    {
        Physics2D.queriesStartInColliders = false;
        speciesManager = FindObjectOfType<SpeciesManager>();
        if (!speciesManager)
        {
            speciesManager = Instantiate(speciesManagerPrefab).GetComponent<SpeciesManager>();
            speciesManager.AddDefaultSpecies();
        }
        prefab = organismPrefab; 
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
                GameObject newOrganism = Instantiate(prefab, transform.position, Quaternion.identity);
                newOrganism.transform.position = GetRandomPosition();
                newOrganism.layer = 3;
                newOrganism.name = speciesName;
                newOrganism.GetComponentInChildren<Organism>().SpecifyOrganism(speciesGenomCode);
                
                newOrganism.tag = "organism";
            }
        }
    }

}
