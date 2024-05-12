using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eater : MonoBehaviour
{
    public float cooldownTime = 1f;
    private bool raycastingEnabled = true;

    public float rayLength = 2f;
    public LayerMask detectionLayer = 3;
    // Start is called before the first frame update
    void Start()
    {
        
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
            Vector3 direction = transform.right;
            Vector3 startPosition = transform.position - transform.right + transform.up;


            Debug.DrawRay(startPosition, direction * rayLength, Color.red);

            RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, rayLength, detectionLayer);

            if (hit.collider != null)
            {
                Destroy(hit.collider.gameObject);
                Eat();
                raycastingEnabled = false;
                StartCoroutine(RaycastCooldown());
            }

            
        }
    }
    IEnumerator RaycastCooldown()
    {
        yield return new WaitForSeconds(cooldownTime);

        raycastingEnabled = true;
    }
    public void Eat()
    {

    }
}
