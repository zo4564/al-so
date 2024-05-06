using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

//body creator odpowiada za wy�wietlenie i zbudowanie organizmu przez usera, korzysta z selektora cz�ci cia�a i tworzony organizm wysy�a do species menagera 
public class BodyCreator : MonoBehaviour, IPointerClickHandler
{
    public SpeciesManager speciesManager;
    public BodyElementSelector selector; 

    public RectTransform uiElementPrefab; 
    public RectTransform parentCanvas;
    public RectTransform bodyCreatorRectTransform; 

    public Vector2 bodyCenter;
    public float radius = 50f;

    public int joints;
    public bool moving;
    public bool defender;
    public string genomCode;
    public void Start()
    {
        speciesManager = FindObjectOfType<SpeciesManager>();
        selector = FindObjectOfType<BodyElementSelector>();
        bodyCenter = bodyCreatorRectTransform.position;

        defender = false;
        moving = false;
        joints = 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        uiElementPrefab = selector.selectedElementPrefab.GetComponent<RectTransform>();
        if (uiElementPrefab != null && parentCanvas != null && !selector.destroy)
        {
            //oblicz pozycje
            Vector2 clickPosition = eventData.position;
            Vector2 direction = clickPosition - bodyCenter;

            //ustaw nazwy i obs�u� jointy
            string bodyPart = PrepareBodyPart(selector.selectedElementPrefab.name);


            //stw�rz element i go ustaw
            Vector2 constrainedPosition = bodyCenter + (direction.normalized * radius);
            RectTransform newUIElement = CreateBodyPart(bodyPart, constrainedPosition, direction);


            //zaktualizuj genom
            Vector2 realPosition = (direction.normalized * radius) / (10 * parentCanvas.localScale.x);
            AddToGenom(bodyPart, realPosition);
        }
    }
    
    public string PrepareBodyPart(string bodyPart)
    {
        if (selector.selectedElementPrefab.name == "jointBase")
        {
            radius = parentCanvas.localScale.x * 120f;
            joints++;
            bodyPart += joints;

        }
        else
        {
            radius = parentCanvas.localScale.x * 50f;
        }
        return bodyPart;
    }
    public RectTransform CreateBodyPart(string bodyPart, Vector3 constrainedPosition, Vector2 direction)
    {
        //stw�rz element, nazwij i dodaj metody
        RectTransform newUIElement = Instantiate(uiElementPrefab, parentCanvas);
        newUIElement.name = bodyPart + "Clone";
        AddClickDestroy(newUIElement);
        if (selector.selectedElementPrefab.name == "jointBase")
        {
            newUIElement.GetComponentInChildren<Image>().enabled = true;
            newUIElement.GetComponentInChildren<JointCreator>().jointPosition = constrainedPosition;
        }

        //ustaw pozycje, skale i rotacje
        newUIElement.position = constrainedPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        newUIElement.rotation = Quaternion.Euler(0, 0, angle - 90);
        newUIElement.localScale = Vector3.one;

        return newUIElement;
    }

    public void AddToGenom(string bodyPart, Vector2 position)
    {
        genomCode += "#" + bodyPart + position.ToString();
    }
    private void AddClickDestroy(RectTransform element)
    {
        EventTrigger trigger = element.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry clickEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };

        clickEntry.callback.AddListener((data) => { TryDestroy(element); });

        trigger.triggers.Add(clickEntry);
    }
    public void TryDestroy(RectTransform element)
    {
        if (selector.destroy)
        {
            Destroy(element.gameObject);
        }
    }

    public void SaveOrganism()
    {
        int count = FindObjectOfType<OrganismCounter>().count;
        string speciesName = FindObjectOfType<NameGenerator>().speciesName;

        if (moving) AddToGenom("l", Vector2.zero);
        if (defender) AddToGenom("d", Vector2.zero);

        Debug.Log(genomCode);
        speciesManager.AddSpecies(speciesName, genomCode, count, moving, defender);
        ResetOrganism();
      
        
    }
    public void SetMover()
    {
        moving = !moving;
    }
    public void SetArmor()
    {
        defender = !defender;
    }    
    public void ResetOrganism()
    {
        //usu� dodane obiekty
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            // je�li obiekt zawiera "Clone" w nazwie, zniszcz go
            if (obj.name.Contains("Clone"))
            {
                Destroy(obj);
            }
        }

        //wyczy�� genom
        genomCode = null;

        //resetuj nazw� i liczb� i liczbe joint�w
        FindObjectOfType<OrganismCounter>().ResetCount();
        FindObjectOfType<NameGenerator>().ResetName();
        joints = 0;

        //odznacz mover i defender
        Toggle[] toggleObjects = GameObject.FindObjectsOfType<Toggle>();
        foreach (Toggle obj in toggleObjects)
        {
            obj.isOn = false;
        }
        moving = false;
        defender = false;
    }


}
