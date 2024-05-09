using System.Collections.Generic;
using UnityEngine;

public class SpeciesSpawner : ObjectSpawner
{
    public SpeciesManager speciesManager;
    public GameObject organismPrefab;

    private void Awake()
    {
        speciesManager = FindObjectOfType<SpeciesManager>();
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
                newOrganism.name = speciesName;
                newOrganism.GetComponentInChildren<Organism>().SpecifyOrganism(speciesGenomCode);
            }
        }
    }

    protected override void HandleSpawnedObject(GameObject obj, int index)
    {
        if (speciesManager.createdSpecies.Count > 0)
        {
            int speciesIndex = index % speciesManager.createdSpecies.Count;
            var species = speciesManager.createdSpecies[speciesIndex];

            obj.name = species.SpeciesName; 
            obj.GetComponentInChildren<Organism>().SpecifyOrganism(species.GenomCode);
        }
    }
}
