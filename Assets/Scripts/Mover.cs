using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.enabled = true;
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        targetPosition = GetRandomPosition(transform.position, randomDirection);
        targetRotation = GetTargetRotation(targetPosition);
        nextDirectionChangeTime = Time.time + GetRandomInterval();
    }
    private void Update()
    {
        Move();
    }

    public void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);


        if ((Vector2)transform.position == targetPosition)
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

