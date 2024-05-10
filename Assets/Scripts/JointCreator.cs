using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JointCreator : MonoBehaviour, IPointerClickHandler
{
    public BodyCreator bodyCreator;
    public Vector2 jointCenter;
    public int jointIndex;

    //public RectTransform jointCreatorRectTransform;
    public void Start()
    {
        //jointCreatorRectTransform = GetComponentInParent<RectTransform>();
        //jointCenter = jointCreatorRectTransform.position;
        Debug.Log("dzia�am");
        Debug.Log(jointCenter);
        Debug.Log(jointIndex);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        // zr�b tak zbey po klikni�ciu w jointa u�ywa�a si� metoda body creatora:
        //oblicza�a si� pozycja od �tordka jointa
        //ustawia� si� genom z odpowiedni� nazw�
        bodyCreator.PlaceBodyPart(eventData, jointCenter, jointIndex - 1);
        

    }
}
