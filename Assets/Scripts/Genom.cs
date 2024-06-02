using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Unity.Jobs;
using Unity.VisualScripting;
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
    public void ResetGenom()
    {
        code = null;
        codeLength = 0;
        bodyParts.Clear();
        positions.Clear();
    }

    public void GenerateGenom(string genomCode)
    {
        bodyParts = new List<string>();
        positions = new List<Vector2>();
        code = genomCode;
        mutationFactor = 10;

        
        ParseGenomCode(genomCode);

        int rand = UnityEngine.Random.Range(0, 100);
        if (rand < mutationFactor / 2)
        {
            MutateRemove();
            Debug.Log("mutation add");
        }
        else if(rand < mutationFactor) 
        {
            MutateAdd();
            Debug.Log("mutation remove");
        }
        codeLength = bodyParts.Count;
    }



    public void MutateAdd()
    {

        string bodyPart = GenerateRandomBodyPart();
        Vector2 position = GenerateRandomVector() * 5;

        if (bodyPart[0].Equals('l'))
            position = GenerateMoverVector();
        if (bodyPart[0].Equals('a'))
            position = Vector2.zero;

        bodyParts.Add(bodyPart);
        positions.Add(position);
        code = UpdateGenomCode(bodyPart, position);
          
        

    }
    public void MutateRemove()
    {

        int randomIndex = UnityEngine.Random.Range(0, bodyParts.Count);

        bodyParts.RemoveAt(randomIndex);
        positions.RemoveAt(randomIndex);
        code = GenerateOrganismCode();



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

        for (int i = 0; i < bodyParts.Count; i++)
        {
            string bodyPart = bodyParts[i];
            Vector2 position = positions[i];
            if (bodyPart.Equals('j'))
                position *= 1.5f;

            generatedCode += $"#{bodyPart}{position}";
        }
        return generatedCode;
    }

    private string GenerateRandomBodyPart()
    {
        List<string> bodyPartsToGenerate = new List<string>{ "e", "m", "j", "g", "p", "l", "a" };

        int joints = 0;
        bool canMove = false;
        
            foreach (string bodyPart in bodyParts) 
            {
                if (bodyPart[0].Equals('j'))
                {
                    joints++;
                }
            if (bodyPart[0].Equals('a'))
            {
                bodyPartsToGenerate.Remove("a");
            }
            if (bodyPart[0].Equals('l'))
                {
                    canMove = true;
                    bodyPartsToGenerate.Remove("p");
                    bodyPartsToGenerate.Remove("l");
                }
                if (bodyPart[0].Equals('p'))
                {
                    bodyPartsToGenerate.Remove("l");
                }
            }

            //TODO: testuj czy to dzia³a
        if (!canMove && !bodyPartsToGenerate.Contains("l"))
        {
            bodyPartsToGenerate.Add("l");
        }
        if (!canMove && !bodyPartsToGenerate.Contains("p"))
        {
            bodyPartsToGenerate.Add("p");
        }

        int jointIndex = UnityEngine.Random.Range(0, joints + 1);
        string newBodyPart = bodyPartsToGenerate[UnityEngine.Random.Range(0, bodyPartsToGenerate.Count)];

        if (newBodyPart.Equals("j"))
            jointIndex = joints + 1;

        return newBodyPart += jointIndex;
    }

    private Vector2 GenerateRandomVector()
    {
        float angle = UnityEngine.Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float x = Mathf.Cos(angle);
        float y = Mathf.Sin(angle);
        return new Vector2(x, y).normalized;
    }
    private Vector2 GenerateMoverVector()
    {
        float x = UnityEngine.Random.Range(0f, 10f);
        float y = 300f;
        return new Vector2(x, y);
    }
    public int CalculateRequiredFood()
    {
        int food = 0;
        for (int i = 0; i < codeLength; i++)
        {
            food++;
            if (bodyParts[i].Equals("l"))
            {
                food += Convert.ToInt32(positions[i].x / 2);
            }
            if (bodyParts[i].Equals("g"))
            {
                food += 2;
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

        if (food < 3)
            food = 3;
        return food;
    }
    public float CalculateEnergyCost()
    {
        float energyCost = bodyParts.Count;
        for (int i = 0; i < bodyParts.Count; i++)
        {
            
            if (bodyParts[i].Equals("l"))
            {
                energyCost += Convert.ToInt32(positions[i].x);
            }
            if (bodyParts[i].Equals("g"))
            {
                energyCost += 1;
            }
            if (bodyParts[i].Equals("a"))
            {
                energyCost += 1;
            }
            if (bodyParts[i].Equals("p"))
            {
                energyCost /= 2f;
            }
        }
        return energyCost;
    }

    private string UpdateGenomCode(string bodyPart, Vector2 position)
    {
        string newCode = code;
        newCode += $"#{bodyPart}{position}";
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


