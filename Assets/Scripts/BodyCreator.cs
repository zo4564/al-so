using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
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
    public Toggle moverToggle;
    public Slider moverSlider;

    public Button producerButton;

    public Vector2 bodyCenter;
    public float radius = 50f;

    public Genom speciesGenom;

    public int joints;
    public bool moving;
    public bool defender;
    public string genomCode;
    public float speed;
    public void Start()
    {
        speciesManager = FindObjectOfType<SpeciesManager>();
        selector = FindObjectOfType<BodyElementSelector>();
        bodyCenter = bodyCreatorRectTransform.position;

        speciesGenom = FindObjectOfType<Genom>();


        defender = false;
        moving = false;
        joints = 1;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(selector.selectedElementPrefab)
        {
        if (selector.selectedElementPrefab.name == "jointBase")
            {
                PlaceBodyPart(eventData, bodyCenter, joints);
            }
        else PlaceBodyPart(eventData, bodyCenter, 0);
        }
        
    }
    public void PlaceBodyPart(PointerEventData eventData, Vector2 parentCenter, int jointIndex)
    {
        uiElementPrefab = selector.selectedElementPrefab.GetComponent<RectTransform>();
            
        if (uiElementPrefab != null && parentCanvas != null && !selector.destroy)
        {
            //oblicz pozycje
            Vector2 clickPosition = eventData.position;
            Vector2 direction = clickPosition - parentCenter;

            //ustaw nazwy i obs�u� jointy
            string bodyPart = PrepareBodyPart(selector.selectedElementPrefab.name);


            //stw�rz element i go ustaw
            Vector2 constrainedPosition = parentCenter + (direction.normalized * radius);
            RectTransform newUIElement = CreateBodyPart(bodyPart, constrainedPosition, direction);


            //zaktualizuj genom
            Vector2 realPosition = (direction.normalized * radius) / (10 * parentCanvas.localScale.x);

            speciesGenom.bodyParts.Add(bodyPart + jointIndex);
            speciesGenom.positions.Add(realPosition);

            //AddToGenom(bodyPart, realPosition, jointIndex);
        }
    }
    
    public string PrepareBodyPart(string bodyPart)
    {
        if (selector.selectedElementPrefab.name == "jointBase")
        {
            radius = parentCanvas.localScale.x * 120f;
            joints++;
            bodyPart = "j";
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
        AddClickDestroy(newUIElement);
        newUIElement.name = bodyPart + "Clone";
        if (selector.selectedElementPrefab.name == "jointBase")
        {
            newUIElement.GetComponentInChildren<Image>().enabled = true;
            newUIElement.GetComponentInChildren<Button>().enabled = false;
            newUIElement.GetComponentInChildren<JointCreator>().jointCenter = constrainedPosition;
            newUIElement.GetComponentInChildren<JointCreator>().jointIndex = joints;
            newUIElement.GetComponentInChildren<JointCreator>().bodyCreator = this;
        }
        if (selector.selectedElementPrefab.name == "p")
        {
            moverToggle.interactable = false;
            moverToggle.isOn = false;
            moving = false;
            moverSlider.interactable = false;
            speed = 0f;
            moverSlider.value = speed;

        }

        //ustaw pozycje, skale i rotacje
        newUIElement.position = constrainedPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        newUIElement.rotation = Quaternion.Euler(0, 0, angle - 90);
        newUIElement.localScale = Vector3.one;

        return newUIElement;
    }

    public void AddToGenom(string bodyPart, Vector2 position, int jointIndex)
    {
        genomCode += "#" + bodyPart + jointIndex + position.ToString();
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

        if (moving)
        {
            //AddToGenom("l", Vector2.zero, 0);
            //AddToGenom("s", new Vector2(speed, 300f), 0);
            speciesGenom.bodyParts.Add("l0");
            speciesGenom.positions.Add(new Vector2(speed, 300f));
        }
        if (defender) //AddToGenom("d", Vector2.zero, 0);
        {
            speciesGenom.bodyParts.Add("a0");
            speciesGenom.positions.Add(Vector2.zero);
        }


        genomCode = speciesGenom.GenerateOrganismCode();
        Debug.Log(genomCode);
        speciesManager.AddSpecies(speciesName, genomCode, count, moving, defender, speed);
        ResetOrganism();
      
        
    }
    public void SetMover()
    {
        moving = !moving;
        producerButton.interactable = !moving;
        moverSlider.interactable = moving;
        if (moving && selector.selectedElementPrefab.name == "p")
            selector.selectedElementPrefab = null;
    }
    public void SetDefender()
    {
        defender = !defender;
    }   
    public void SetSpeed()
    {
        speed = moverSlider.value;
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
        speciesGenom.ResetGenom();

        //resetuj nazw� i liczb� i liczbe joint�w
        FindObjectOfType<OrganismCounter>().ResetCount();
        FindObjectOfType<NameGenerator>().ResetName();
        joints = 0;

        //odznacz mover i defender
        Toggle[] toggleObjects = GameObject.FindObjectsOfType<Toggle>();
        foreach (Toggle obj in toggleObjects)
        {
            obj.isOn = false;
            obj.interactable = true;
        }
        moving = false;
        defender = false;

        producerButton.interactable = true;
        moverSlider.interactable = true;
        speed = 2f;
        moverSlider.value = speed;
    }


}
