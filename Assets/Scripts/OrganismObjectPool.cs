using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganismObjectPool : MonoBehaviour
{
    public GameObject organismPrefab;
    public int poolSize;
    private Queue<GameObject> organismPool = new Queue<GameObject>();

    public BodyPartObjectPool bodyPartPool;

    

    private void Start()
    {
        InitializePool();
    }

    // TODO: popraw ¿eby dobrze dzia³a³o jak jest wiêcej organizmów ni¿ wielkosæ poola
    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject organism = Instantiate(organismPrefab);
            organism.SetActive(false);
            organismPool.Enqueue(organism);
        }
    }

    public GameObject GetOrganism()
    {
        if (organismPool.Count == 0)
        {
            GameObject organism = Instantiate(organismPrefab);
            return organism;
        }
        else
        {
            GameObject organism = organismPool.Dequeue();
            organism.SetActive(true);
            return organism;
        }
    }

    public void ReturnOrganism(GameObject organism)
    {

        organism.GetComponent<Mover>().enabled = false;
        organism.GetComponent<MovementController>().enabled = false;

        

        organism.SetActive(false);
        organism.name = "returned";


        organismPool.Enqueue(organism);

    }
    
}
