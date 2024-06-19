using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DesignerOrganismGenerator : MonoBehaviour
{
    public GenomeValidator validator;
    public TMP_InputField inputGenomeField;
    public TMP_InputField inputNameField;
    public SpeciesManager speciesManager;
    public OrganismCounter counter;
    // Start is called before the first frame update
    void Start()
    {
        speciesManager = FindAnyObjectByType<SpeciesManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CheckGenome()
    {
        if (validator)
        {
            if (validator.ValidateInput(inputGenomeField.text))
            {
                AddSpeciesFromGenome();
                inputGenomeField.text = string.Empty;
            }
            else
            {
                inputGenomeField.text = string.Empty;
                Debug.Log("incorrect genome format");
            }
        }
        
    }
   
    public void AddSpeciesFromGenome() 
    {
        
        speciesManager.AddSpecies(inputNameField.text, inputGenomeField.text, counter.count);

    }
}
