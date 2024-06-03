using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[System.Serializable]
public class SimulationData
{
    public List<SnapshotData> snapshots = new List<SnapshotData>();
}

[System.Serializable]
public class SnapshotData
{
    public float time;
    public List<SpeciesData> speciesDataList;
}

[System.Serializable]
public class SpeciesData
{
    public string speciesName;
    public int aliveCount;
}
public class DataCollector : MonoBehaviour
{
    public List<GameObject> organisms; 
    public float sampleInterval = 3f;

    public SimulationData simulationData = new SimulationData();

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
        snapshotData.speciesDataList = new List<SpeciesData>();

        
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
            }
        }

        foreach (var speciesCount in speciesCounts)
        {
            SpeciesData speciesData = new SpeciesData
            {
                speciesName = speciesCount.Key,
                aliveCount = speciesCount.Value
            };
            snapshotData.speciesDataList.Add(speciesData);
        }

        simulationData.snapshots.Add(snapshotData);
    }

    public void EndSimulation()
    {
        string jsonData = JsonUtility.ToJson(simulationData, true);

        string filePath = Path.Combine(Application.persistentDataPath, "simulationData.json");
        SaveDataToFile(jsonData, filePath);
    }

    private void SaveDataToFile(string jsonData, string filePath)
    {
        File.WriteAllText(filePath, jsonData);
    }
}
