using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganismObjectPool : MonoBehaviour
{
    public GameObject organismPrefab;
    public GameObject bodyPartPrefab;
    public int poolSize;
    private Queue<GameObject> organismPool = new Queue<GameObject>();
    private Queue<GameObject> bodyPartPool = new Queue<GameObject>();

    

    private void Start()
    {
        InitializePool();
    }

    // popraw ¿eby dobrze dzia³a³o jak jest wiêcej organizmów ni¿ wielkosæ poola
    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject organism = Instantiate(organismPrefab);
            organism.SetActive(false);
            organismPool.Enqueue(organism);
        }
        for (int i = 0; i < poolSize * 5; i++)
        {
            GameObject bodyPart = Instantiate(bodyPartPrefab);
            bodyPart.SetActive(false);
            organismPool.Enqueue(bodyPart);
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
    public GameObject GetBodyPart()
    {
        if (bodyPartPool.Count == 0)
        {
            GameObject bodyPart = Instantiate(bodyPartPrefab);
            return bodyPart;
        }
        else
        {
            GameObject bodyPart = bodyPartPool.Dequeue();
            bodyPart.SetActive(true);
            return bodyPart;
        }
    }

    public void ReturnOrganism(GameObject organism)
    {

        organism.GetComponent<Mover>().enabled = false;
        organism.GetComponent<MovementController>().enabled = false;

        foreach (GameObject child in organism.GetComponent<Organism>().bodyParts)
        {
            child.name = "returned";
            ReturnBodyPart(child);
        }

        organism.SetActive(false);
        organism.name = "returned";

        organismPool.Enqueue(organism);

    }
    public void ReturnBodyPart(GameObject bodyPart)
    {

        bodyPart.TryGetComponent<VisionSensor>(out VisionSensor visionSensor);
        if(visionSensor) Destroy(visionSensor);

        bodyPart.TryGetComponent<Eater>(out Eater eater);
        if (eater) Destroy(eater);

        bodyPart.TryGetComponent<AttackSystem>(out AttackSystem gun);
        if (gun) Destroy(gun);

        bodyPart.TryGetComponent<Producer>(out Producer producer);
        if (producer) Destroy(producer);

        bodyPart.SetActive(false);
        bodyPart.transform.position = Vector3.zero;
        

        organismPool.Enqueue(bodyPart);
    }
}
