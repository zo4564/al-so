using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float zoomSpeed = 5f;
    public float minZoom = 20f;
    public float maxZoom = 60f;

    public float followSpeed = 2f; 
    public Transform target;
    public StaminaSystem staminaSystem;

    public TextMeshProUGUI organismName;
    public TextMeshProUGUI generation;
    public TextMeshProUGUI stamina;
    public GameObject organismPanel;
    public string genome;
    public TextMeshProUGUI copyButtonText;

    void Update()
    {
        // kontrola ruchu kamery
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f).normalized;
        transform.position += moveSpeed * Time.deltaTime * moveDirection;

        if (target)
        {
            Vector3 newPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newPosition, followSpeed * Time.deltaTime);
            stamina.text = staminaSystem.currentStamina.ToString();
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
            hitCollider.TryGetComponent<Organism>(out Organism organism);
            if(organism)
            {
                organismName.text = organism.gameObject.name;
                generation.text = organism.reproductionSystem.generation.ToString();
                staminaSystem = organism.staminaSystem;
                genome = organism.genom.code;
                copyButtonText.text = "copy genome";
            }
        }
        else target = null;
    }
    public void CopyNameToClipboard()
    {
        GUIUtility.systemCopyBuffer = genome;
        Debug.Log("copied: " + genome);
        copyButtonText.text = "copied!";
        
    }

}
