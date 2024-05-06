using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JointCreator : MonoBehaviour
{
    public BodyCreator bodyCreator;
    public void Start()
    {
        bodyCreator = GetComponent<BodyCreator>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
       
    }
}
