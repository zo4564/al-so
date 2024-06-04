using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[System.Serializable]
public class SimulationCountData
{
    public List<SnapshotData> snapshots = new List<SnapshotData>();
}

public class SimulationGenomeData
{
    public List<SpeciesGenomeData> genomes = new List<SpeciesGenomeData>();
}

[System.Serializable]
public class SnapshotData
{
    public float time;
    public List<SpeciesCountData> speciesDataList;
}

[System.Serializable]
public class SpeciesCountData
{
    public string speciesName;
    public int aliveCount;
}
[System.Serializable]
public class SpeciesGenomeData
{
    public string speciesName;
    public string genome;
}
public class DataCollector : MonoBehaviour
{
    public List<GameObject> organisms; 
    public float sampleInterval = 3f;
    public SimulationCountData simulationCountData = new SimulationCountData();

    public SimulationGenomeData simulationGenomeData = new SimulationGenomeData();


    void Start()
    {
        StartCoroutine(CollectDataCoroutine());
    }

    private IEnumerator CollectDataCoroutine()
    {
        while (true)
        {
            CollectData();
            yield return new WaitForSeconds(sampleInterval);
        }
    }

    private void CollectData()
    {
        SnapshotData snapshotData = new SnapshotData();
        snapshotData.time = Time.time;
        snapshotData.speciesDataList = new List<SpeciesCountData>();

        Dictionary<string, string> speciesGenomes = new Dictionary<string, string>();
        Dictionary<string, int> speciesCounts = new Dictionary<string, int>();

        foreach (GameObject organism in organisms)
        {
            if (organism.activeInHierarchy)
            {
                string speciesName = organism.name;
                if (speciesCounts.ContainsKey(speciesName))
                {
                    speciesCounts[speciesName]++;
                }
                else
                {
                    speciesCounts[speciesName] = 1;
                }

                if (!speciesGenomes.ContainsKey(speciesName))
                {
                    speciesGenomes[speciesName] = organism.GetComponent<Genom>().code;
                }
            }
        }

        foreach (var speciesCount in speciesCounts)
        {
            SpeciesCountData speciesData = new SpeciesCountData
            {
                speciesName = speciesCount.Key,
                aliveCount = speciesCount.Value
            };
            snapshotData.speciesDataList.Add(speciesData);
        }

        simulationCountData.snapshots.Add(snapshotData);

        HashSet<string> trackedSpecies = new HashSet<string>(simulationGenomeData.genomes.Select(g => g.speciesName));

        foreach (var speciesGenome in speciesGenomes)
        {
            if (!trackedSpecies.Contains(speciesGenome.Key))
            {
                SpeciesGenomeData genomeData = new SpeciesGenomeData
                {
                    speciesName = speciesGenome.Key,
                    genome = speciesGenome.Value
                };
                simulationGenomeData.genomes.Add(genomeData);
                trackedSpecies.Add(speciesGenome.Key);
            }
        }

        foreach (var speciesGenome in simulationGenomeData.genomes)
        {
            Debug.Log(speciesGenome.speciesName + ": " + speciesGenome.genome);
        }
    }


    public void EndSimulation()
    {

        string jsonSimData = JsonUtility.ToJson(simulationCountData, true);
        string jsonGenData = JsonUtility.ToJson(simulationGenomeData, true);


        string filePath = Path.Combine(Application.persistentDataPath, "simulationCountData.json");
        SaveDataToFile(jsonSimData, filePath);
        filePath = Path.Combine(Application.persistentDataPath, "simulationGenomeData.json");
        SaveDataToFile(jsonGenData, filePath);
    }

    private void SaveDataToFile(string jsonData, string filePath)
    {
        File.WriteAllText(filePath, jsonData);
    }
}
