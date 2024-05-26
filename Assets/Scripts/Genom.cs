using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Genom : MonoBehaviour
{ 
    public string code;
    public int codeLength;
    public List<string> bodyParts;
    public List<Vector2> positions;
    public int mutationFactor;

    public void Start()
    {
    }

    public void GenerateGenom(string genomCode)
    {
        bodyParts = new List<string>();
        positions = new List<Vector2>();
        code = genomCode;

        ParseGenomCode(genomCode);
        Mutate();
    }



    public void Mutate()
    {
        // TODO: poprawiæ generowanie pozycji i zrobiæ ¿eby mutacje zale¿a³y od mutationfactor
        string bodyPart = GenerateRandomBodyPart();
        Vector2 position = GenerateRandomVector(5f);
        bodyParts.Add(bodyPart);
        positions.Add(position);
        code = UpdateGenomCode(bodyPart, position);
          
        
        codeLength = bodyParts.Count;

    }
    public override string ToString()
    {
        string genomInString  = $"codeLength: {codeLength}, code: {code}, components: \n";
        for (int i = 0; i < positions.Count; i++)
        {
            genomInString += $"{bodyParts[i].ToString()}, x: {positions[i].x}, y: {positions[i].y}\n";
        }
        return genomInString;
    }

    public void SetMutationFactor(int factor)
    {
        mutationFactor = factor;
        mutationFactor += UnityEngine.Random.Range(-2, 2);

    }
    public string GenerateOrganismCode()
    {
        string generatedCode = "";

        for (int i = 0; i < codeLength; i++)
        {
            string bodyPart = bodyParts[i];
            // TODO: rzuca b³êdem jak jest reprodukcja
            Vector2 position = positions[i];
            if (bodyPart.Equals('j'))
                position *= 1.5f;

            generatedCode += $"#{bodyPart}({position.x:F2}, {position.y:F2})";
        }
        return generatedCode;
    }

    private string GenerateRandomBodyPart()
    {
        string[] bodyParts = { "e", "m", "j", "p", "g" };
        string bodyPart = bodyParts[UnityEngine.Random.Range(0, bodyParts.Length)];
        return bodyPart += "0";
    }

    private Vector2 GenerateRandomVector(float range)
    {
        float angle = UnityEngine.Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float x = range * Mathf.Cos(angle);
        float y = range * Mathf.Sin(angle);
        return new Vector2(x, y);
    }
    public int CalculateRequiredFood()
    {
        int food = 0;
        for (int i = 0; i < codeLength; i++)
        {
            food++;
            if (bodyParts[i].Equals("s"))
            {
                food += Convert.ToInt32(positions[i].x);
            }
            if (bodyParts[i].Equals("g"))
            {
                food += 3;
            }
            if (bodyParts[i].Equals("a"))
            {
               food += 2;
            }
            if (bodyParts[i].Equals("p"))
            {
                food = 1;
            }
        }
        Debug.Log("required food: " + food);
        return food;
    }

    private string UpdateGenomCode(string bodyPart, Vector2 position)
    {
        string newCode = code;
        newCode += $"#{bodyPart}({position.x:F2}, {position.y:F2})";
        return newCode;

    }
    private void ParseGenomCode(string genomCode)
    {
        
        string[] parts = genomCode.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
        //Debug.Log(parts.Length);

        foreach (string part in parts)
        {

            string bodyPart = part[0].ToString();
            bodyPart += part[1].ToString();
            //Debug.Log(bodyPart);
            bodyParts.Add(bodyPart);

            int startIndex = part.IndexOf('(');
            int endIndex = part.IndexOf(')');

            if (startIndex >= 0 && endIndex > startIndex)
            {
                string coordinateString = part.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();
                

                string[] coordinates = coordinateString.Split(',');
                
                string coordX = coordinates[0].Trim(); 
                string coordY = coordinates[1].Trim();
                   
                float x = float.Parse(coordX, CultureInfo.InvariantCulture);
                float y = float.Parse(coordY, CultureInfo.InvariantCulture);
                positions.Add(new Vector2(x, y));
                
            }
        }
    }
}


