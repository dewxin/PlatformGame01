using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public interface IPlatformUser
{
    Vector3 Position { get; set; }
}

//实际上subPlatform
public class Platform : MonoBehaviour
{
    public enum Direction
    {
        None = 0,
        Horizontal = 1,
        Vertical = 2,
    }

    private bool inited;

    //TODO 这里不是tranform， 当地行移动的时候会有问题，
    //可能需要改成相对位置
    public Vector3 StartPoint;
    public Vector3 EndPoint;

    public Vector3 LineDir { get; private set; }

    [SerializeField]
    private Direction direction = Direction.Horizontal;
    public Direction Direct {get => direction; set { direction = value; } }

    public Platform PrevPlatform;
    public Platform NextPlatform;

    // Start is called before the first frame update
    void Awake()
    {
        Init();

    }

    public void Init()
    {
        if (inited)
            return;
        LineDir = (EndPoint - StartPoint).normalized;

    }


    public Vector3 NearestPointOnLine(Vector3 pnt)
    {
        Init();

        var v = pnt - StartPoint;
        var d = Vector3.Dot(v, LineDir);
        return StartPoint + LineDir * d;
    }

    public Vector2 GetCorrectedSpeed(Vector2 velocity)
    {
        var magnitude = velocity.magnitude;
        var subVec = Vector2.Dot(velocity, LineDir);
        var result = (subVec * LineDir).normalized * magnitude;
        return result;
    }

    public bool MoveRetIfBoundry(IPlatformUser platformUser, ref float movement, out Platform ajacentPlatform)
    {
        var position = platformUser.Position;

        var destination = StartPoint;
        var newPlatform = PrevPlatform;
        if (movement > 0)
        {
            destination = EndPoint;
            newPlatform = NextPlatform;
        }

        var vector2Dest = (destination - position);
        var distanceLeft = Mathf.Abs(movement) - vector2Dest.magnitude;
        if (distanceLeft < 0)
        {
            position += LineDir * movement;
            platformUser.Position = position;
            ajacentPlatform = null;
            movement = 0;
            return false;
        }

        platformUser.Position = destination;
        ajacentPlatform = newPlatform;
        movement = distanceLeft * Mathf.Sign(movement);
        return true;

    }



}
