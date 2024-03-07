using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SpriteMerge : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;// assumes you've dragged a reference into this
    public Action<Sprite> OnMainCharacterSpriteUpdate = delegate{};

    //TODO need a cache system
    private Sprite cache;
    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(spriteRenderer == null)
            spriteRenderer = this.gameObject.AddComponent<SpriteRenderer>();
    }

    public void Update()
    {
        spriteRenderer.sprite = Create(this.transform);
        OnMainCharacterSpriteUpdate(spriteRenderer.sprite);
    }

    /* Takes a transform holding many sprites as input and creates one flattened sprite out of them */
    public Sprite Create(Transform input)
    {
        var spriteRendererList = input.GetComponentsInChildren<SpriteRenderer>().ToList();
        if (spriteRendererList.Count == 0)
        {
            Debug.Log("No SpriteRenderers found in " + input.name + " for SpriteMerge");
            return null;
        }
        spriteRendererList.Sort((sr1,sr2)=>sr1.sortingOrder.CompareTo(sr2.sortingOrder));
        var spriteList = new List<Sprite>();
        foreach(var spriteRender in spriteRendererList)
        {
            var sprite = spriteRender.sprite;
            if (sprite == null)
                continue;
            if (spriteRender.gameObject == this.gameObject)
                continue;

            spriteList.Add(sprite);
        }

        if (cache != null && cache.name == CalculateName(spriteList))
            return cache;

        var size = CalculateSize(spriteList,out Vector2Int pivot);
        if(size == Vector2.zero)
        {
            Debug.LogWarning("SpriteMerge.cs final size should not be 0");
            return null;
        }
        cache = CreateSprite(spriteList, size, pivot);
        return cache;
   }

    private Vector2Int CalculateSize(List<Sprite> spriteList, out Vector2Int pivot)
    {
        UnityEngine.Profiling.Profiler.BeginSample("SpriteMerge CalculateSize");

        if(spriteList.Count == 0)
        {
            pivot = Vector2Int.zero;
            return Vector2Int.zero;
        }

        int minX = int.MaxValue;
        int maxX = int.MinValue;
        int minY = int.MaxValue;
        int maxY = int.MinValue;

        int pivotX = 0;
        int pivotY=0;
        foreach(var sprite in spriteList)
        {
            var minVec =  -sprite.pivot;
            if(minVec.x < minX)
            {
                pivotX = (int)sprite.pivot.x;
                minX = (int)minVec.x;
            }
            if(minVec.y < minY)
            {
                pivotY = (int)sprite.pivot.y;
                minY = (int)minVec.y;
            }

            var maxVec = sprite.rect.size - sprite.pivot;
            maxX = (int)Mathf.Max(maxX, maxVec.x);
            maxY = (int)Mathf.Max(maxY, maxVec.y);
        }

        var result = new Vector2Int(maxX-minX,maxY-minY);
        pivot = new Vector2Int(pivotX, pivotY);

        UnityEngine.Profiling.Profiler.EndSample();
        return result;
    }

    private Sprite CreateSprite(List<Sprite> spriteList, Vector2Int size, Vector2Int pivotPixel)
    {
        UnityEngine.Profiling.Profiler.BeginSample($"SpriteMerge CreateSprite");
        var pivotFloat = ((Vector2)pivotPixel) / size;
        var targetTexture = new Texture2D(size.x, size.y, TextureFormat.RGBA32, false, false);
        targetTexture.filterMode = FilterMode.Point;

        var targetPixels = new Color[size.x * size.y];

        var fillColor = new Color(0, 0, 0, 0);
        for(int i = 0; i < targetPixels.Count();++i)
        {
            targetPixels[i] = fillColor;
        }

        foreach(var sprite in spriteList)
        {
            UnityEngine.Profiling.Profiler.BeginSample("SpriteMerge CreateSprite 1 sprite");
            var offsetPixel = pivotPixel - new Vector2Int((int)sprite.pivot.x, (int)sprite.pivot.y);

            var spriteSize = sprite.rect.size;

            //优化1
            int spriteSizeX = (int)spriteSize.x;
            int spriteSizeY = (int)spriteSize.y;

            int spriteRectX = (int)sprite.rect.x;
            int spriteRectY = (int)sprite.rect.y;

            //优化2
            var pixels = sprite.texture.GetPixels(spriteRectX, spriteRectY, spriteSizeX, spriteSizeY);
            for(int i = 0;i< spriteSizeX; i++)
            {
                for(int j = 0; j < spriteSizeY; j++)
                {
                    var index = i + j * spriteSizeX;
                    var color = pixels[index];

                    //避免透明的像素覆盖之前的颜色
                    if (color.a == 0)
                        continue;

                    //targetTexture.SetPixel(i + offsetPixel.x, j + offsetPixel.y, color);

                    var dstIndex = (i + offsetPixel.x) + (j + offsetPixel.y) * size.x;
                    targetPixels[dstIndex] = color;
                }
            }
            UnityEngine.Profiling.Profiler.EndSample();
        }


        targetTexture.SetPixels(targetPixels);
        targetTexture.Apply(false, true);// read/write is disabled in 2nd param to free up memory
        var result =  Sprite.Create(targetTexture, new Rect(new Vector2(), size), pivotFloat, 100, 0, SpriteMeshType.FullRect);

        result.name = CalculateName(spriteList);

        UnityEngine.Profiling.Profiler.EndSample();
        return result;
    }


    private string CalculateName(List<Sprite> spriteList)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (var sprite in spriteList)
            stringBuilder.Append(sprite.name);

        return stringBuilder.ToString();
    }
}