using UnityEngine;
using System.Collections.Generic;
using System.Globalization;
using static System.Collections.Specialized.BitVector32;
using System;
using UnityEngine.UIElements;

//po³¹czenie nazwy czêœci i sprite do niej
[System.Serializable]
public class BodyPartSpritePair
{
    public string bodyPartType;
    public Sprite sprite;
}

//generator czêsci cia³a konkretnych organizmów, buduje je na podstawie genomu
public class OrganismSpriteGenerator : MonoBehaviour
{
    public OrganismObjectPool organismPool;
    public List<BodyPartSpritePair> bodyPartSprites;

    public void Start()
    {
        organismPool = FindAnyObjectByType<OrganismObjectPool>();
    }
    //zrobi czêœci cia³a jako nowe obiekty
    public void GenerateBodyObjects(Genom genom, GameObject body)
    {
        //ustala warstwy
        SpriteRenderer parentRenderer = body.GetComponent<SpriteRenderer>();
        int parentSortingLayer = parentRenderer.sortingLayerID;
        int parentOrderInLayer = parentRenderer.sortingOrder;

        List<Vector2> jointPositions = new List<Vector2>();
        jointPositions.Add(body.transform.position);

        for (int i = 0; i < genom.bodyParts.Count; i++)
        {
            //szuka czêsci i pozycji i sprita
            string bodyPart = genom.bodyParts[i][0].ToString();
            int jointIndex = Convert.ToInt32(genom.bodyParts[i][1].ToString());

            Vector2 position = genom.positions[i];
            Sprite sprite = FindSpriteForBodyPart(bodyPart);
            position /= 2f;

            //tworzy obiekt
            GameObject cell = organismPool.GetBodyPart();
            cell.name = bodyPart;
            cell.tag = bodyPart[0].ToString();

            Vector3 newPosition = new Vector3();
            //oblicza now¹ pozycje
            if(bodyPart.Equals("j"))
            {
                jointPositions.Add(CalculateBodyPartPosition(position, body.transform.position));
                newPosition = CalculateBodyPartPosition(position, body.transform.position);
            }
            else    
                newPosition = CalculateBodyPartPosition(position, jointPositions[jointIndex]);

            //ustawia pozycje i rotacje
            cell.transform.SetPositionAndRotation(newPosition, Quaternion.identity);
            cell.transform.SetParent(body.transform);
            float angle = Mathf.Atan2(position.y, position.x) * Mathf.Rad2Deg;
            angle -= 90f;
            cell.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            //ustawienie sprita i odpowiednie warstwy
            SpriteRenderer renderer = cell.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingLayerID = parentSortingLayer;
            renderer.sortingOrder = parentOrderInLayer + 1;

            //w³¹cza movera
            if (bodyPart.Equals("l"))
            {
                body.GetComponent<Mover>().enabled = true;
                body.GetComponent<MovementController>().enabled = true;
            }
            if (bodyPart.Equals("s"))
            {
                body.GetComponent<Mover>().moveSpeed = position.x;
                body.GetComponent<Mover>().rotationSpeed = position.y;
            }
            //w³¹cza oko
            if (bodyPart.Equals("e"))
            {
                VisionSensor eye = cell.AddComponent<VisionSensor>();
                eye.enabled = true;
                eye.detectionLayer = LayerMask.GetMask("FOOD");
            }
            //w³¹cza chodzenie
            if (bodyPart.Equals("m"))
            {
                Eater eater = cell.AddComponent<Eater>();
                eater.enabled = true;
                eater.detectionLayer = LayerMask.GetMask("FOOD");

            }
            //w³¹cza ataki
            if (bodyPart.Equals("g"))
            {
                AttackSystem gun = cell.AddComponent<AttackSystem>();
                gun.enabled = true;
                gun.detectionLayer = LayerMask.GetMask("ORGANISM");

            }
            //dodaje odpornoœæ - przenosi na bezpieczn¹ warstwê
            if (bodyPart.Equals("a"))
            {
                body.layer = 7;

            }
            if (bodyPart.Equals("p"))
            {
                Producer producer = cell.AddComponent<Producer>();
                producer.enabled = true;
                body.GetComponent<Collider2D>().enabled = false;


            }



        }
    }
    public Vector3 CalculateBodyPartPosition(Vector2 positionFromGenom, Vector3 parentPartPosition)
    {
        Vector3 newPosition = new Vector3(positionFromGenom.x, positionFromGenom.y, 0);
        newPosition += parentPartPosition;
        return newPosition;
    }
    //generowanie sprita NIE DZIA£A
    public Sprite GenerateOrganismSprite(Genom genom)
    {
        Texture2D texture = new Texture2D(1024, 1024); 
        Color[] pixels = new Color[texture.width * texture.height];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.clear; 
        }
        texture.SetPixels(pixels);
        texture.Apply();

        for(int i = 1; i < genom.bodyParts.Count; i++)
        {
            string bodyPart = genom.bodyParts[i];
            Vector2 position = genom.positions[i];
            Sprite sprite = FindSpriteForBodyPart(bodyPart);

            if (sprite != null)
            {
                int x = Mathf.RoundToInt(position.x * texture.width / 10); 
                int y = Mathf.RoundToInt(position.y * texture.height / 10);

                DrawSprite(texture, sprite, x, y);
            }
            else
            {
                Debug.LogError("No sprite found for body part type: " + bodyPart);
            }
            
        }

        Sprite finalSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
        return finalSprite;
    }

   
    private Sprite FindSpriteForBodyPart(string bodyPartType)
    {
        foreach (var pair in bodyPartSprites)
        {
            if (pair.bodyPartType.Equals(bodyPartType))
            {
                return pair.sprite;
            }
        }
        return null; 
    }

    private void DrawSprite(Texture2D texture, Sprite sprite, int x, int y)
    {
        if (sprite == null)
        {
            Debug.LogWarning("Attempting to draw a null sprite.");
            return;
        }

        Rect spriteRect = sprite.rect;
        Color[] spritePixels = sprite.texture.GetPixels(
            (int)spriteRect.x,
            (int)spriteRect.y,
            (int)spriteRect.width,
            (int)spriteRect.height);

        for (int dy = 0; dy < spriteRect.height; dy++)
        {
            for (int dx = 0; dx < spriteRect.width; dx++)
            {
                Color pixel = spritePixels[dy * (int)spriteRect.width + dx];
                int tx = x + dx;
                int ty = y + dy;

                if (tx >= 0 && tx < texture.width && ty >= 0 && ty < texture.height)
                {
                    Color existingColor = texture.GetPixel(tx, ty);
                    Color finalColor = Color.Lerp(existingColor, pixel, pixel.a);
                    texture.SetPixel(tx, ty, finalColor);
                }
            }
        }
    }
    
}