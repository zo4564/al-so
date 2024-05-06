using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Genom
{
    public string code;
    public int codeLength;
    public List<char> bodyParts;
    public List<Vector2> positions;

    public Genom()
    {
        this.codeLength = 5;
        this.bodyParts = new List<char>();
        this.positions = new List<Vector2>();
        this.code = GenerateOrganismCode();
    }

    public Genom(string genomCode)
    {
        this.bodyParts = new List<char>();
        this.positions = new List<Vector2>();
        this.code = genomCode;

        ParseGenomCode(genomCode); // Parsing the code to extract data
    }

    public Genom(int codeLength, string code, List<char> bodyParts, List<Vector2> positions)
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
            char bodyPart = GenerateRandomBodyPart();
            Vector2 position = GenerateRandomVector(2f);
            if (bodyPart == 'j')
                position *= 1.5f;

            generatedCode += $"#{bodyPart}({position.x:F2}, {position.y:F2})";
            bodyParts.Add(bodyPart);
            positions.Add(position);
        }
        return generatedCode;
    }

    private char GenerateRandomBodyPart()
    {
        char[] bodyParts = { 'e', 'l', 'm', 'j', 'p', 'g' };
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
        
        string[] parts = genomCode.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string part in parts)
        {
            //Debug.Log("analizuje: " + part);

            char bodyPart = part[0];
            //Debug.Log("komórka: " + bodyPart);
            bodyParts.Add(bodyPart);

            int startIndex = part.IndexOf('(');
            int endIndex = part.IndexOf(')');

            if (startIndex >= 0 && endIndex > startIndex)
            {
                string coordinateString = part.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();
                //Debug.Log("pozycja string: " + coordinateString);

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


