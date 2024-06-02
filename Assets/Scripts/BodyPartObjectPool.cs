using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BodyPartObjectPool : MonoBehaviour
{
    public GameObject bodyPartPrefab;
    public int poolSize;
    private Queue<GameObject> bodyPartPool = new Queue<GameObject>();

    public GameObject bodyPartHolder;

    private void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bodyPart = Instantiate(bodyPartPrefab);
            bodyPart.SetActive(false);
            bodyPartPool.Enqueue(bodyPart);
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

    public void ReturnBodyPart(GameObject bodyPart)
    {

        bodyPart.name = "returned";
        bodyPart.transform.SetParent(bodyPartHolder.transform);

        bodyPart.TryGetComponent<VisionSensor>(out VisionSensor visionSensor);
        if (visionSensor) Destroy(visionSensor);

        bodyPart.TryGetComponent<Eater>(out Eater eater);
        if (eater) Destroy(eater);

        bodyPart.TryGetComponent<AttackSystem>(out AttackSystem gun);
        if (gun) Destroy(gun);

        bodyPart.TryGetComponent<Producer>(out Producer producer);
        if (producer) Destroy(producer);

        bodyPart.SetActive(false);
        bodyPart.transform.position = Vector3.zero;


        bodyPartPool.Enqueue(bodyPart);
    }
}
