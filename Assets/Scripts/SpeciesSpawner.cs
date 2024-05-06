using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//generator organizmów
public class SpeciesSpawner : MonoBehaviour
{
    public GameObject objectPrefab;
    public int numberOfObjects;
    public float maxDistanceFromCenter;
    public SpeciesManager speciesManager;

    private void Awake()
    {
        speciesManager = FindObjectOfType<SpeciesManager>();
        
    }
    void Start()
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
                GameObject newOrganism = Instantiate(objectPrefab, transform.position, Quaternion.identity);
                newOrganism.transform.position = GetRandomPosition();
                newOrganism.name = speciesName;
                newOrganism.GetComponentInChildren<Organism>().SpecifyOrganism(speciesGenomCode);                
            }
        }
    }

    void GenerateObjects()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            GameObject newOrganism = Instantiate(objectPrefab, transform.position, Quaternion.identity);
            newOrganism.transform.position = GetRandomPosition();
        }
    }

    Vector3 GetRandomPosition()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(0f, maxDistanceFromCenter);
        return transform.position + (Vector3)randomDirection * randomDistance;
    }
}
