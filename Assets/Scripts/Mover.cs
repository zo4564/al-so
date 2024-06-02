using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//mover odpowiada za poruszanie siê organizmu
public class Mover : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float rotationSpeed = 300f;

    public float minDistanceFromCenter = 2f;
    public float maxDistanceFromCenter = 8f;

    public float minChangeDirectionInterval = 0.3f;
    public float maxChangeDirectionInterval = 5f;

    public Vector2 targetPosition;
    public Quaternion targetRotation;
    public float nextDirectionChangeTime;
    public TrailRenderer trailRenderer;

    private Vector3 lastPosition;
    private float stuckThreshold;
    public bool isStuck = false;
    public float stuckTime;
    private void Start()
    {

    }
    private void OnEnable()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.enabled = false;
        TrailDelay();
        
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        targetPosition = GetRandomPosition(transform.position, randomDirection);
        targetRotation = GetTargetRotation(targetPosition);
        nextDirectionChangeTime = Time.time + GetRandomInterval();

        stuckThreshold = moveSpeed * 1.5f;
        StartCoroutine(CheckForStuckCoroutine());
    }
    IEnumerator TrailRendererStartCooldown()
    {
        yield return new WaitForSeconds(2);

        trailRenderer.enabled = true;
    }
    private void Update()
    {
    }
    public void RandomMove()
    {
        MoveToPosition();


        if (IsOnTargetPosition())
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            targetPosition = GetRandomPosition(transform.position, randomDirection);
            targetRotation = GetTargetRotation(targetPosition);
        }
        if (Time.time >= nextDirectionChangeTime)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            targetPosition = GetRandomPosition(transform.position, randomDirection);
            targetRotation = GetTargetRotation(targetPosition);
            nextDirectionChangeTime = Time.time + GetRandomInterval();
        }

    }
    public void TrailDelay()
    {
        StartCoroutine(TrailRendererStartCooldown());
    }
    public void CheckForStuck()
    {
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);

        if (distanceMoved < stuckThreshold)
        {
            isStuck = true;
        }
        lastPosition = transform.position;
    }
    IEnumerator CheckForStuckCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f * Random.Range(0.7f, 1.3f));
            CheckForStuck();
        }
    }
    public bool IsOnTargetPosition()
    {
        
        if ((Vector2)transform.position == targetPosition)
        {
            return true;
        }

        return false;
    }

    public void MoveToPosition()
    {
        targetRotation = GetTargetRotation(targetPosition);

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    }
    
    Vector2 GetDirectionInFront(float minAngle, float maxAngle)
    {
        float angle = Mathf.Atan2(targetPosition.y - transform.position.y, targetPosition.x - transform.position.x) * Mathf.Rad2Deg;
        float angleDifference = Mathf.DeltaAngle(transform.eulerAngles.z, angle);

        if (Mathf.Abs(angleDifference) < maxAngle)
        {
            return (targetPosition - (Vector2)transform.position).normalized;
        }

        float newAngle = Mathf.Clamp(angle, transform.eulerAngles.z - maxAngle, transform.eulerAngles.z + maxAngle);
        Vector2 newDirection = new Vector2(Mathf.Cos(newAngle * Mathf.Deg2Rad), Mathf.Sin(newAngle * Mathf.Deg2Rad));

        return newDirection.normalized;
    }
    Vector2 GetRandomPosition(Vector3 myPosition, Vector2 direction)
    {
        float randomDistance = Random.Range(0f, maxDistanceFromCenter);
        return myPosition + (Vector3)direction * randomDistance;
    }
    Quaternion GetTargetRotation(Vector2 targetPosition)
    {
        float angle = Mathf.Atan2(transform.position.y - targetPosition.y, transform.position.x - targetPosition.x) * Mathf.Rad2Deg;
        angle += 90f;
        return Quaternion.Euler(0f, 0f, angle);
    }
    private float GetRandomInterval()
    {
        return Random.Range(minChangeDirectionInterval, maxChangeDirectionInterval);
    }
}

