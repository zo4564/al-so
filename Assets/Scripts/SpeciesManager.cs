using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

//species manager zapisze utworzone gatunki i przeka¿e je do symulacji
public class SpeciesManager : MonoBehaviour
{
    public List<Species> createdSpecies;
    public int idCounter;

    public TextMeshProUGUI savedSpecies;

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
        DisplayListOfSpecies();
    }
    public void AddDefaultSpecies()
    {
        createdSpecies = new List<Species>();
        Species newSpecies = new(idCounter, "bambik", "#e0(3.00, 3.00)#e0(3.00, -3.00)#m0(0.00, 5.00)#l0(4.00, 300.00)", 25, true, false, 4);
        createdSpecies.Add(newSpecies);
        //Debug.Log("added species: " + createdSpecies[idCounter]);
        idCounter++;
        DisplayListOfSpecies();
    }
    public void AddSpecies(string speciesName, string genomCode,  int count)
    {
        createdSpecies = new List<Species>();

        Species newSpecies = new(idCounter, speciesName, genomCode, count, ThereIsNoPart('l', genomCode), ThereIsNoPart('a', genomCode), FindSpeed(genomCode));
        createdSpecies.Add(newSpecies);
        Debug.Log("added species: " + createdSpecies[idCounter]);
        Debug.Log(createdSpecies[idCounter].GenomCode);
        idCounter++;
        DisplayListOfSpecies();
    }
    private bool ThereIsNoPart(char bodyPart, string genomCode)
    {
        bool partIsThere = true;
        string[] parts = genomCode.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string part in parts)
        {
            if (part[0].Equals(bodyPart))
                partIsThere = false;
            
        }
        return partIsThere;
    }
    private float FindSpeed(string genomCode)
    {
        float speed = 0f;
        string[] parts = genomCode.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
        //Debug.Log(parts.Length);

        foreach (string part in parts)
        {

            char bodyPart = part[0];

            int startIndex = part.IndexOf('(');
            int endIndex = part.IndexOf(')');

            if (startIndex >= 0 && endIndex > startIndex)
            {
                string coordinateString = part.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();
                string[] coordinates = coordinateString.Split(',');
                string coordX = coordinates[0].Trim();

                if (bodyPart.Equals('l'))
                {
                    return float.Parse(coordX, CultureInfo.InvariantCulture);
                }

            }

        }
        return speed;
    }
    public void DisplayListOfSpecies()
    {
        string textToDisplay = "Your species: ";
        if(savedSpecies)
        {
            
            foreach (Species species in createdSpecies)
            {
                textToDisplay += species.ToString();
                textToDisplay += "\n";
            }
        
        savedSpecies.text = textToDisplay;
        }
    }
}