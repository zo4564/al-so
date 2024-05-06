using System.Collections;
using System.Globalization;
using UnityEngine;

//organizm posiada genom i sw�j generator, kt�ry go buduje
public class Organism : MonoBehaviour
{
    public Genom genom;
    public string organismCode; 
    public int numberOfCells;
    public OrganismSpriteGenerator spriteGenerator;

    void Start()
    {

    }
    
    public void SetCode(string genomCode)
    {
        organismCode = genomCode;
    }
    public void InitializeGenom()
    {
        genom = new Genom(organismCode);
        //Debug.Log(genom);
    }

    public void GenerateOrganism()
    {
        spriteGenerator = FindAnyObjectByType<OrganismSpriteGenerator>();
        spriteGenerator.GenerateBodyObjects(genom, this.gameObject);
        
    }
    public void SpecifyOrganism(string genomCode)
    {
        SetCode(genomCode);
        InitializeGenom();
        Debug.Log("genom organizmu: ");
        Debug.Log(genom);
        GenerateOrganism();
        
    }
    
}
