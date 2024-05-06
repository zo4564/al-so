using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class DragSource : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject dragPrefab; // Prefab to instantiate for dragging
    private GameObject dragObject;
    private RectTransform canvasTransform;
    public Transform target;
    public Vector2 targetPosition;
    public float snapThreshold = 1f;
    public float radius = 2f;

    private void Start()
    {
        canvasTransform = GetComponentInParent<Canvas>().transform as RectTransform;
        target = FindAnyObjectByType<DropTarget>().GetComponentInParent<Transform>();
        targetPosition = Camera.main.ScreenToWorldPoint(target.position);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragObject = Instantiate(dragPrefab, canvasTransform);
        dragObject.transform.position = eventData.position;
        Vector2 dragPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        float distance = Vector2.Distance(dragPosition, targetPosition);
        if (distance <= snapThreshold)
        {
            Vector2 direction = (dragPosition - targetPosition).normalized;
            Vector2 snapPosition = targetPosition + direction * radius;
            dragObject.transform.position = snapPosition;
            Debug.Log($"Snapped to: {snapPosition}"); 
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragObject != null)
        {
            dragObject.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Allow the drop event to complete before destroying the drag object
        StartCoroutine(DestroyAfterDelay(dragObject, 0.1f)); // Delay for a short time to ensure event completion
    }

    private IEnumerator DestroyAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(obj);
    }
}
