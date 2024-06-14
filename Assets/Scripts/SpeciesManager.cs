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

    public void AddSpecies(string speciesName, string genomCode, int count, bool moving, bool defender, float speed)
    {
        Species newSpecies = new(idCounter, speciesName, genomCode, count, moving, defender, speed);
        createdSpecies.Add(newSpecies);
        //Debug.Log("added species: " + createdSpecies[idCounter]);
        idCounter++;
    }
    public void AddDefaultSpecies()
    {
        createdSpecies = new List<Species>();
        Species newSpecies = new(idCounter, "bambik", "#e0(3.00, 3.00)#e0(3.00, -3.00)#m0(0.00, 5.00)#l0(4.00, 300.00)", 25, true, false, 4);
        createdSpecies.Add(newSpecies);
        //Debug.Log("added species: " + createdSpecies[idCounter]);
        idCounter++;
    }
}