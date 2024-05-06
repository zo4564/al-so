using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameGenerator : MonoBehaviour
{
    public TMP_InputField nameInput;
    public string speciesName;
    // Start is called before the first frame update
    void Start()
    {
        ResetName();   
        nameInput.onEndEdit.AddListener(UpdateName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void UpdateName(string input)
    {
        speciesName = nameInput.text;
    }
    public void ResetName()
    {
        nameInput.text = null;

    }
    private void UpdateInputField()
    {
        nameInput.text = speciesName;
    }
}
