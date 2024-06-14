using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JointCreator : MonoBehaviour, IPointerClickHandler
{
    public BodyCreator bodyCreator;
    public Vector2 jointCenter;
    public int jointIndex;

    public BodyElementSelector selector;

    //public RectTransform jointCreatorRectTransform;
    public void Start()
    {
        selector = FindObjectOfType<BodyElementSelector>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (selector.selectedElementPrefab && selector.selectedElementPrefab.name != "jointBase")
        {
            bodyCreator.PlaceBodyPart(eventData, jointCenter, jointIndex - 2);
        }
                
        

    }
}
