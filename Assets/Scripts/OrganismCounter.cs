using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class OrganismCounter : MonoBehaviour
{
    public TMP_InputField countInput;
    public int count;

    void Start()
    {
        count = 1;
        countInput.contentType = TMP_InputField.ContentType.IntegerNumber;
        countInput.onEndEdit.AddListener(OnEndEdit);
        UpdateInputField();
    }

    public void IncreaseCount()
    {
        if (count < 100) 
        {
            count++;
            UpdateInputField();
        }
    }

    public void DecreaseCount()
    {
        if (count > 1)
        {
            count--;
            UpdateInputField();
        }
    }

    void UpdateInputField()
    {
        countInput.text = count.ToString(); 
    }

    private void OnEndEdit(string input)
    {
       
        int newCount;

        bool isValid = Int32.TryParse(input, out newCount);

        if (isValid && newCount >= 1 && newCount <= 100) 
        {
            count = newCount;
        }
        else
        {
         
            UpdateInputField();
        }
    }
    public void ResetCount()
    {
        count = 1;
        UpdateInputField();

    }
}
