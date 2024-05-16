using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    public float cooldownTime = 1f;
    private bool raycastingEnabled = true;

    public FoodObjectPool foodPool;
    public float rayLength = 3f;
    public LayerMask detectionLayer = 3;
    // Start is called before the first frame update
    void Start()
    {
        foodPool = FindAnyObjectByType<FoodObjectPool>();
    }

    // Update is called once per frame
    void Update()
    {
        Detect();

    }
    void Detect()
    {

        if (raycastingEnabled)
        {
            Vector3 direction1 = transform.right;
            Vector3 startPosition1 = transform.position - transform.right + transform.up;

            //Vector3 direction2 = transform.up;
            //Vector3 startPosition2 = transform.position;

            Debug.DrawRay(startPosition1, direction1 * rayLength, Color.blue);
            //Debug.DrawRay(startPosition2, direction2 * rayLength, Color.blue);

            RaycastHit2D hit1 = Physics2D.Raycast(startPosition1, direction1, rayLength, detectionLayer);
            //RaycastHit2D hit2 = Physics2D.Raycast(startPosition1, direction1, rayLength, detectionLayer);

            HandleCollision(hit1);
            //HandleCollision(hit2);


        }
    }
    IEnumerator RaycastCooldown()
    {
        yield return new WaitForSeconds(cooldownTime);

        raycastingEnabled = true;
    }
    public void HandleCollision(RaycastHit2D hit)
    {
        if (hit.collider != null)
        {
            Attack(hit.collider.gameObject);
        }
    }
    public void Attack(GameObject target)
    {
        Vector3 targetPosition = target.transform.position;
        GameObject food = foodPool.GetFood();
        foodPool.HandleFood(food);
        food.transform.position = targetPosition;
        Destroy(target);
        raycastingEnabled = false;
        StartCoroutine(RaycastCooldown());
    }
}
