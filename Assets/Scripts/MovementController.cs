using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public List<VisionSensor> visionSensors;
    public Mover mover;
    public List<Vector3> targets;

    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponent<Mover>();
        visionSensors = new List<VisionSensor>();

        VisionSensor[] foundSensors = GetComponentsInChildren<VisionSensor>();

        visionSensors.AddRange(foundSensors);
    }

    // Update is called once per frame
    void Update()
    {
        if(targets.Count == 0)
        {
            mover.RandomMove();
        }
        else 
        {
            mover.targetPosition = targets[0];
            mover.MoveToPosition();
            
            if(mover.IsOnTargetPosition())
            {
                targets.Remove(targets[0]);
            }
        }
    }
}
