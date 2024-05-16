using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//po³¹czenie nazwy czêœci i sprite do niej
[System.Serializable]
public class BodyPartCostPair
{
    public string bodyPartType;
    public float cost;
}

public class EnergyCalculator : MonoBehaviour
{
    public List<BodyPartCostPair> bodyPartCost;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public float GetBodyPartCost(string bodyPartType)
    {
        foreach (var pair in bodyPartCost)
        {
            if (pair.bodyPartType.Equals(bodyPartType))
            {
                return pair.cost;
            }
        }
        return 0;
    }
}
