using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Genom
{
    public string code;
    public int codeLength;
    public List<string> bodyParts;
    public List<Vector2> positions;

    public Genom()
    {
        this.codeLength = 5;
        this.bodyParts = new List<string>();
        this.positions = new List<Vector2>();
        this.code = GenerateOrganismCode();
    }

    public Genom(string genomCode)
    {
        this.bodyParts = new List<string>();
        this.positions = new List<Vector2>();
        this.code = genomCode;

        ParseGenomCode(genomCode);
    }

    public Genom(int codeLength, string code, List<string> bodyParts, List<Vector2> positions)
    {
        this.codeLength = codeLength;
        this.code = code;
        this.bodyParts = bodyParts;
        this.positions = positions;
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

    public string GenerateOrganismCode()
    {
        string generatedCode = "";

        for (int i = 0; i < codeLength; i++)
        {
            string bodyPart = GenerateRandomBodyPart();
            Vector2 position = GenerateRandomVector(2f);
            if (bodyPart.Equals('j'))
                position *= 1.5f;

            generatedCode += $"#{bodyPart}({position.x:F2}, {position.y:F2})";
            bodyParts.Add(bodyPart);
            positions.Add(position);
        }
        return generatedCode;
    }

    private string GenerateRandomBodyPart()
    {
        string[] bodyParts = { "e", "l", "m", "j", "p", "g" };
        return bodyParts[UnityEngine.Random.Range(0, bodyParts.Length)];
    }

    private Vector2 GenerateRandomVector(float range)
    {
        float angle = UnityEngine.Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float x = range * Mathf.Cos(angle);
        float y = range * Mathf.Sin(angle);
        return new Vector2(x, y);
    }

    private void ParseGenomCode(string genomCode)
    {
        
        string[] parts = genomCode.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string part in parts)
        {

            string bodyPart = part[0].ToString();
            bodyPart += part[1].ToString();
            Debug.Log(bodyPart);
            bodyParts.Add(bodyPart);

            int startIndex = part.IndexOf('(');
            int endIndex = part.IndexOf(')');

            if (startIndex >= 0 && endIndex > startIndex)
            {
                string coordinateString = part.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();
                

                string[] coordinates = coordinateString.Split(',');

                if (coordinates.Length == 2)
                {
                    string coordX = coordinates[0].Trim(); 
                    string coordY = coordinates[1].Trim();
                    
                    float x = float.Parse(coordX, CultureInfo.InvariantCulture);
                    float y = float.Parse(coordY, CultureInfo.InvariantCulture);
                    positions.Add(new Vector2(x, y));
                }
            }
        }
    }
}


