using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//species manager zapisze utworzone gatunki i przeka�e je do symulacji
public class SpeciesManager : MonoBehaviour
{
    public List<Species> createdSpecies;
    public int idCounter;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        createdSpecies = new List<Species>();
        idCounter = 0;
    }

    public void AddSpecies(string speciesName, string genomCode, int count, bool moving, bool armor)
    {
        Species newSpecies = new(idCounter, speciesName, genomCode, count, moving, armor);
        createdSpecies.Add(newSpecies);
        Debug.Log("added species: " + createdSpecies[idCounter]);
        idCounter++;
    }
}