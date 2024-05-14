using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//species manager zapisze utworzone gatunki i przeka¿e je do symulacji
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

    public void AddSpecies(string speciesName, string genomCode, int count, bool moving, bool defender)
    {
        Species newSpecies = new(idCounter, speciesName, genomCode, count, moving, defender);
        createdSpecies.Add(newSpecies);
        //Debug.Log("added species: " + createdSpecies[idCounter]);
        idCounter++;
    }
    public void AddDefaultSpecies()
    {
        createdSpecies = new List<Species>();
        Species newSpecies = new(idCounter, "bambik", "#g0(-3, 3)#e0(3, 3)#e0(-3, -3)#e0(3, -3)#m0(0, 5)#l0(0, 0)#a0(0, 0)", 2, true, true);
        createdSpecies.Add(newSpecies);
        //Debug.Log("added species: " + createdSpecies[idCounter]);
        idCounter++;
    }
}