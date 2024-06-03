using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionSensor : MonoBehaviour
{
    public float cooldownTime = 0.3f;
    private bool raycastingEnabled = true;

    public float detectionRange = 30f;
    public LayerMask detectionLayer = 3; 
    public int numRays = 4; 
    public float minAngle = -25f; 
    public float maxAngle = 25f;


    public MovementController controller;
    private void Start()
    {
        controller = GetComponentInParent<MovementController>();
        
        
    }
    void Update()
    {
        Detect();
    }

    void Detect()
    {
        float angleStep = (maxAngle - minAngle) / (numRays - 1); 
        float currentAngle = minAngle;

        if (raycastingEnabled)
        {
            for (int i = 0; i < numRays; i++)
            {
                float relativeAngle = currentAngle + transform.eulerAngles.z;
                Quaternion rotation = Quaternion.Euler(0, 0, relativeAngle);
                Vector3 direction = rotation * Vector3.up;

                Vector3 startPosition = transform.position;

                //Debug.DrawRay(startPosition, direction * detectionRange, Color.yellow);

                RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, detectionRange, detectionLayer);

                if (hit.collider != null)
                {
                    if (!controller.targets.Contains(hit.transform.position))
                    {
                        controller.targets.Add(hit.transform.position);
                    }
                    
                    raycastingEnabled = false;
                    StartCoroutine(RaycastCooldown());
                }

                currentAngle += angleStep;
            }
        }
    }
    IEnumerator RaycastCooldown()
    {
        yield return new WaitForSeconds(cooldownTime);

        raycastingEnabled = true;
    }
}
