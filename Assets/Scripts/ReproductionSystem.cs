using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReproductionSystem : MonoBehaviour
{
    public int collectedFood;
    public int requiredFood;

    public float reproductionCost;
    //public float mutationFactor = 10f;

    public List<string> ancestralGenomes;
    public int generation;
    public string strain;

    public OrganismObjectPool organismPool;

    public AttackSystem attackSystem;

    // Start is called before the first frame update
    void Start()
    {
        organismPool = FindAnyObjectByType<OrganismObjectPool>();

        collectedFood = 0;

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
        
        newOrganism.GetComponentInChildren<Organism>().WakeUp(childGenom.code);

        ReproductionSystem babyReproductionSystem = newOrganism.GetComponentInChildren<ReproductionSystem>();
        babyReproductionSystem.ancestralGenomes = new List<string>(ancestralGenomes);
        babyReproductionSystem.UpdateAncestralGenomes(childGenom.code);
        babyReproductionSystem.UpdateStrain(strain);

        babyReproductionSystem.generation = generation + 1;

        newOrganism.transform.position = transform.position + Vector3.up;
        newOrganism.layer = 3;
        newOrganism.name = babyReproductionSystem.FindName(name);
        newOrganism.tag = "organism";

        //newOrganism.GetComponent<Genom>().Mutate(mutationFactor);


    }
    public bool CheckIfReady()
    {
        if (collectedFood >= requiredFood)
        {
            return true;
        }
        return false;
    }
    public void UpdateStrain(string parentStrain)
    {
        if (ancestralGenomes.Count >= 2)
        {
            string lastGenome = ancestralGenomes[^1];
            string secondLastGenome = ancestralGenomes[ancestralGenomes.Count - 2];

            string processedLastGenome = ProcessGenomeCode(lastGenome);
            string processedSecondLastGenome = ProcessGenomeCode(secondLastGenome);

            if (processedLastGenome.Equals(processedSecondLastGenome))
            {
                strain = parentStrain;
            }
            else
            {
                strain = NewStrain();
            }
        }
    }

    private string ProcessGenomeCode(string genomeCode)
    {
        int lIndex = genomeCode.IndexOf('l');
        if (lIndex == -1)
        {
            return genomeCode; 
        }

        string partBeforeL = genomeCode.Substring(0, lIndex);

        int nextSeparatorIndex = genomeCode.IndexOf('#', lIndex + 1);
        string partAfterL = nextSeparatorIndex == -1 ? string.Empty : genomeCode.Substring(nextSeparatorIndex);

        return partBeforeL + partAfterL;
    }

    public void UpdateAncestralGenomes(string genom)
    {
        ancestralGenomes ??= new List<string>();
        ancestralGenomes.Add(genom);
    }

    public string NewStrain()
    {
        string strain = "";
        for(int i = 0; i < 2; i++)
        {
            string s1 = "qwertyuiopasdfghjklzxcvbnm";
            int randomLetter1 = Random.Range(0, 26);
            strain += s1[randomLetter1].ToString();

        }
        generation = 0;
        return strain;
    }
    public string FindName(string parentName)
    {
        string newName = parentName.Split("-")[0];
        newName += "-" + strain.ToString();
        return newName;

    }
    public void SetFirstGeneration(string speciesName, string speciesGenomCode)
    {
        collectedFood = 0;
        strain = "al";
        generation = 0;

        string genomCode = GetComponent<Organism>().genom.code;
        string processedGenomeCode = ProcessGenomeCode(genomCode);


        if (!processedGenomeCode.Equals(speciesGenomCode))
        {
            strain = NewStrain();
        }
        name = speciesName + "-" + strain.ToString();
    }

    private string TruncateAtCharacter(string value, char delimiter)
    {
        int index = value.IndexOf(delimiter);
        return index == -1 ? value : value.Substring(0, index);
    }
    public void Reset()
    {
        ancestralGenomes = new List<string>();
        generation = 0;
        strain = "al";
        collectedFood = 0;
    }

}
