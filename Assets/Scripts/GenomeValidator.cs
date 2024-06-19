using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class GenomeValidator : MonoBehaviour
{

    string pattern = @"#([a-z]\d)\(([-+]?[0-9]*\.?[0-9]+), ([-+]?[0-9]*\.?[0-9]+)\)";

    void Start()
    {

    }

    public bool ValidateInput(string userInput)
    {
        if (Regex.IsMatch(userInput, pattern))
        {
            return true;
        }
        return false;
    }

    void OnDestroy()
    {
    }
}