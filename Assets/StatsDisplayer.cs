using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsDisplayer : MonoBehaviour
{
    public DataCollector dataCollector;
    public TextMeshProUGUI aliveCount;
    public TextMeshProUGUI speciesCount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(dataCollector)
        {
            aliveCount.text = dataCollector.aliveOrganisms.ToString();
            speciesCount.text = dataCollector.speciesCount.ToString();
        }
       
    }
}
