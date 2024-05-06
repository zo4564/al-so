using UnityEngine;
using UnityEngine.EventSystems;

public class DropTarget : MonoBehaviour, IDropHandler
{
    public float radius = 2f; // Radius for snapping
    public Vector2 center; // Center of the circular body
    public float snapThreshold = 1f; // Threshold for snapping

    private void Start()
    {
        center = transform.position; // Initial center position
    }

    public void OnDrop(PointerEventData eventData)
    {
        var draggedObject = eventData.pointerDrag;

        if (draggedObject != null)
        {
            // Get the drop position in world space
            Vector2 dropPosition = Camera.main.ScreenToWorldPoint(eventData.position);

            // Calculate the distance to the circular body's center
            float distance = Vector2.Distance(dropPosition, center);

            // If within the threshold, apply the snapping logic
            if (distance <= snapThreshold)
            {
                // Calculate the direction vector from center to drop position
                Vector2 direction = (dropPosition - center).normalized;

                // Calculate the point on the circle at the given radius
                Vector2 snapPosition = center + direction * radius;

                // Snap the dragged object to this position
                draggedObject.transform.position = snapPosition;

                Debug.Log($"Snapped to: {snapPosition}"); // Debug information
            }
        }
    }
}
