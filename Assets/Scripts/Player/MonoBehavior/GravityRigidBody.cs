using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[RequireComponent(typeof(PlatformRigidBody))]
public class GravityRigidBody : MonoBehaviour
{
    [SerializeField]
    private float groundCheckRadius = 0.1f;
    [SerializeField]
    private float climbCheckRadius = 0.25f;

    private PlatformRigidBody platformRigidBody;
    private bool onPlatform => platformRigidBody.onPlatform;

    //todo 和PlayerInput的Velocity冗余了 感觉应该改改
    public Vector2 velocity;

    private const float gravityAcceleration = 9.8f;
    [SerializeField]
    private float gravityScale = 1.2f;

    void Awake()
    {
        platformRigidBody = GetComponent<PlatformRigidBody>();
    }

    private void OnEnable()
    {
        platformRigidBody.OnNextPlatformNull += OnNextPlatformNull;
    }

    private void OnDisable()
    {
        platformRigidBody.OnNextPlatformNull -= OnNextPlatformNull;
    }



    private void FixedUpdate()
    {
        CheckPlatformWhenJump();

        if(!onPlatform)
        {
            UpdateSpeed();
            UpdatePostion();
        }
    }


    private void CheckPlatformWhenJump()
    {
        if(onPlatform)
            return;

        var collider = CheckHorizontalPlatform();
        if (collider == null)
            return;

        var nomalVec = collider.gameObject.transform.up;
        if (Vector3.Dot(velocity, nomalVec) > 0)
            return;

        var platform = collider.GetComponent<Platform>();
        var position = platform.NearestPointOnLine(transform.position);
        this.transform.position = position;
        platformRigidBody.LandPlatformFromJump(platform);
    }

    public Collider2D CheckHorizontalPlatform()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, groundCheckRadius, LayerMask.GetMask("Platform")).collider;

    }

    public Collider2D CheckVerticalPlatform(Vector2 direction)
    {
        return Physics2D.Raycast(transform.position, direction, climbCheckRadius, LayerMask.GetMask("ClimbPlatform")).collider;
    }


    public void OnNextPlatformNull(Vector3 Position, Vector2 vector)
    {
        //Debug.Log($"Leave platform {vector}");
        this.velocity = vector; 
    }


    private void UpdateSpeed()
    {
        velocity += Vector2.down * gravityAcceleration * gravityScale * Time.fixedDeltaTime;
    }

    private void UpdatePostion()
    {
        Vector3 vel = velocity * Time.fixedDeltaTime;
        transform.position += vel;
    }

    private void OnDrawGizmos()
    {
        Vector3 offset = Vector3.left * .01f;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position - offset + Vector3.up * groundCheckRadius, transform.position -offset);

        Gizmos.color = Color.blue;

        Gizmos.DrawLine(transform.position+ offset, transform.position + offset + Vector3.up * climbCheckRadius);
    }

}
