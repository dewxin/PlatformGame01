using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectBoundry : Boundry
{
    [SerializeField] 
    private float left = 0f;
    [SerializeField] 
    private float right = 2f;
    [SerializeField] 
    private float top = 20f;
    [SerializeField] 
    private float down = 0f;

    public bool IsMapBoundry = false;
    public float DefaultPadding = 1f;


    private void Awake()
    {
        if (IsMapBoundry)
            Boundry.MapBoundry = this;

    }

    void Start()
    {

    }


    public Rect GetBoundry()
    {
        Rect rect = new Rect
        {
            xMin = transform.position.x + left,
            xMax = transform.position.x + right,
            yMin = transform.position.y + down,
            yMax = transform.position.y + top
        };

        return rect;
    }

    public Rect GetBoundryPadding(Vector2 padding)
    {
        var boundry = GetBoundry();
        Rect rect = new Rect
        {
            xMin = boundry.xMin + padding.x,
            xMax = boundry.xMax - padding.x,
            yMin = boundry.yMin + padding.y,
            yMax = boundry.yMax - padding.y,
        };

        return rect;
    }




    private void OnDrawGizmos()
    {
        if (!DrawGizmos)
            return;

        var boundry = GetBoundry();
        Gizmos.DrawWireCube(boundry.center, boundry.size);

    }

}
