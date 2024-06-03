using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Messenger : MonoBehaviour
{
    public DataCollector dataManager;
    // Start is called before the first frame update
    void Awake()
    {
        dataManager = FindAnyObjectByType<DataCollector>();
        dataManager.organisms.Add(gameObject);
    }

    private void OnDestroy()
    {
        if(dataManager)
        {
            dataManager.organisms.Remove(gameObject);

        }
    }
}
