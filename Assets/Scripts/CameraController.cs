using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float zoomSpeed = 5f;
    public float minZoom = 20f;
    public float maxZoom = 60f;

    public float followSpeed = 2f; 
    public Transform target;

    void Update()
    {
        // kontrola ruchu kamery
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f).normalized;
        transform.position += moveSpeed * Time.deltaTime * moveDirection;

        if (target != null)
        {
            Vector3 newPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newPosition, followSpeed * Time.deltaTime);
        }

        if (Input.GetMouseButtonDown(0))
        {
            CheckForObjectClick();
        }
    }

    void CheckForObjectClick()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);

        if (hitCollider != null)
        {
            target = hitCollider.transform;

            hitCollider.TryGetComponent<Mover>(out Mover mover);
            if (mover) { followSpeed = mover.moveSpeed; }
        }
        else target = null;
    }

}
