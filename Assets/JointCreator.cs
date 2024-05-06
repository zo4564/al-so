using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JointCreator : MonoBehaviour
{
    public BodyCreator bodyCreator;
    public Vector2 jointPosition;
    public void Start()
    {
        bodyCreator = GetComponent<BodyCreator>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        // zrób tak zbey po klikniêciu w jointa u¿ywa³a siê metoda body creatora:
        //oblicza³a siê pozycja od œtordka jointa
        //ustawia³ siê genom z odpowiedni¹ nazw¹

    }
}
