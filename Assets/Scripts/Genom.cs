using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Unity.Jobs;
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
        mutationFactor = 10;

        ParseGenomCode(genomCode);

        int rand = UnityEngine.Random.Range(0, 100);
        Debug.Log("rand: " + rand + "mf: " + mutationFactor);
        if (rand < mutationFactor)
        {
            Mutate();
        }
        codeLength = bodyParts.Count;
    }



    public void Mutate()
    {
        // TODO: poprawiæ generowanie pozycji

        Debug.Log("mutatuin");
        string bodyPart = GenerateRandomBodyPart();
        Vector2 position = GenerateRandomVector(5f);
        bodyParts.Add(bodyPart);
        positions.Add(position);
        code = UpdateGenomCode(bodyPart, position);
          
        

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
            Vector2 position = positions[i];
            if (bodyPart.Equals('j'))
                position *= 1.5f;

            generatedCode += $"#{bodyPart}({position.x:F2}, {position.y:F2})";
        }
        return generatedCode;
    }

    private string GenerateRandomBodyPart()
    {
        List<string> bodyPartsToGenerate = new List<string>{ "e", "m", "j", "g", "p", "l" };

        int joints = 0;
        foreach (string bodyPart in bodyParts) 
        {
            if (bodyPart[0].Equals('j'))
            {
                joints++;
            }
            if (bodyPart[0].Equals('l'))
            {
                bodyPartsToGenerate.Remove("p");
            }
            if (bodyPart[0].Equals('p'))
            {
                bodyPartsToGenerate.Remove("l");
            }
        }
        int jointIndex = UnityEngine.Random.Range(0, joints + 1);
        string newBodyPart = bodyPartsToGenerate[UnityEngine.Random.Range(0, bodyPartsToGenerate.Count)];

        if (newBodyPart.Equals("j"))
            jointIndex = joints + 1;

        return newBodyPart += jointIndex;
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


