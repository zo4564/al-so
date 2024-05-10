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
        Debug.Log("dzia³am");
        Debug.Log(jointCenter);
        Debug.Log(jointIndex);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        // zrób tak zbey po klikniêciu w jointa u¿ywa³a siê metoda body creatora:
        //oblicza³a siê pozycja od œtordka jointa
        //ustawia³ siê genom z odpowiedni¹ nazw¹
        bodyCreator.PlaceBodyPart(eventData, jointCenter, jointIndex - 1);
        

    }
}
