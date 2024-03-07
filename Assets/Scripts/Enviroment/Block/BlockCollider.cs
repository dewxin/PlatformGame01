using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class BlockCollider : MonoBehaviour
{
    public static System.Collections.Generic.List<BlockCollider> List = new System.Collections.Generic.List<BlockCollider>();

    public enum BlockDirection
    {
        Left,
        Right,
    }
    public BlockDirection ForbiddenDirection;
    
    public BoxCollider BoxCollider { get; private set; }

    private void Start()
    {
        BoxCollider = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        List.Add(this);
    }

    private void OnDisable()
    {
        List.Remove(this);
        
    }

    public Vector3 ForbiddenVector()
    {
        if (ForbiddenDirection == BlockDirection.Left)
            return Vector3.left;
        else if(ForbiddenDirection == BlockDirection.Right)
            return Vector3.right;

        return Vector3.left;
    }


    private void OnDrawGizmosSelected()
    {

        GizmosDrawArrow();
        GizmosDrawX();
    }

    private void GizmosDrawArrow()
    {
        Gizmos.color = Color.white;
        var boxCollider = GetComponent<BoxCollider>();
        var halfSize = boxCollider.size/2;

        var origion = boxCollider.center + transform.position - ForbiddenVector() * halfSize.x;

        var point = boxCollider.center + transform.position + ForbiddenVector() * halfSize.x;

        var arrowUp = (origion + point) / 2 + Vector3.up * halfSize.y;
        var arrowDown = (origion + point) / 2 + Vector3.down * halfSize.y;

        var list = new System.Collections.Generic.List<Vector3>() { origion, point, arrowUp, point, arrowDown };
        Gizmos.DrawLineStrip(list.ToArray(), false);
    }

    private void GizmosDrawX()
    {
        Gizmos.color = Color.red;
        var boxCollider = GetComponent<BoxCollider>();
        var halfSize = boxCollider.size / 2;
        var middle = boxCollider.center + transform.position;

        var leftTop = middle + new Vector3(-halfSize.x, halfSize.y);
        var leftDown = middle + new Vector3(-halfSize.x, -halfSize.y);
        var rightTop = middle + new Vector3(halfSize.x, halfSize.y);
        var rightDown = middle + new Vector3(halfSize.x, -halfSize.y);

        Gizmos.DrawLine(leftTop, rightDown);
        Gizmos.DrawLine(leftDown, rightTop);
    }

}
