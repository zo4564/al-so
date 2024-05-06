using UnityEngine;
using System.Collections.Generic;
using System.Globalization;
using static System.Collections.Specialized.BitVector32;
using System;

//po��czenie nazwy cz�ci i sprite do niej
[System.Serializable]
public class BodyPartSpritePair
{
    public char bodyPartType;
    public Sprite sprite;
}

//generator cz�sci cia�a konkretnych organizm�w, buduje je na podstawie genomu
public class OrganismSpriteGenerator : MonoBehaviour
{
    public List<BodyPartSpritePair> bodyPartSprites; 

    //zrobi cz�ci cia�a jako nowe obiekty
    public void GenerateBodyObjects(Genom genom, GameObject body)
    {
        //ustala warstwy
        SpriteRenderer parentRenderer = body.GetComponent<SpriteRenderer>();
        int parentSortingLayer = parentRenderer.sortingLayerID;
        int parentOrderInLayer = parentRenderer.sortingOrder;

        for (int i = 0; i < genom.bodyParts.Count; i++)
        {
            //szuka cz�sci i pozycji i sprita
            char bodyPart = genom.bodyParts[i];
            Vector2 position = genom.positions[i];
            Sprite sprite = FindSpriteForBodyPart(bodyPart);
            position /= 2f;
            
            //tworzy obiekt
            GameObject cell = new GameObject(bodyPart.ToString());
            cell.name = bodyPart.ToString();
            cell.tag = bodyPart.ToString();

            //oblicza now� pozycje
            Vector3 newPosition = new Vector3(position.x, position.y, 0);
            newPosition += body.transform.position;
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

            //w��cza movera
            if (bodyPart.ToString().Equals("l"))
            {
                body.GetComponent<Mover>().enabled = true;
            }
            


        }
    }
    //generowanie sprita NIE DZIA�A
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
            char bodyPart = genom.bodyParts[i];
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

   
    private Sprite FindSpriteForBodyPart(char bodyPartType)
    {
        foreach (var pair in bodyPartSprites)
        {
            if (pair.bodyPartType == bodyPartType)
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