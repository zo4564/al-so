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
        // zr�b tak zbey po klikni�ciu w jointa u�ywa�a si� metoda body creatora:
        //oblicza�a si� pozycja od �tordka jointa
        //ustawia� si� genom z odpowiedni� nazw�

    }
}
