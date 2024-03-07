using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRigidBody : MonoBehaviour, IPlatformUser
{
    public bool onPlatform => platform != null;

    protected Platform platform;
    protected GravityRigidBody gravityRigidBody;

    public Platform.Direction PlatformDirect => platform.Direct;

    protected bool canJump => gravityRigidBody != null;

    public Vector3 Position { get => transform.position; set => transform.position = value; }

    private PlayerInputMove inputMove;

    public Action<Vector3, Vector2> OnNextPlatformNull = delegate{};
    public Action<Platform> OnLandPlatformFromJump = delegate { };

    //TODO 重构
    public Action<Platform> OnLeavePlatform = delegate { };
    public Action<Platform> OnEnterPlatform = delegate { };

    void Start()
    {
        gravityRigidBody = GetComponent<GravityRigidBody>();
        inputMove = GetComponent<PlayerInputMove>();
    }

    private void FixedUpdate()
    {
        if (onPlatform && PlayerSingleton.Instance.State.CanInput())
        {
            UpdatePostion();
            //CheckPlatformIfTurningNull();
        }
    }


    public void LandPlatformFromJump(Platform subPlatform)
    {
        this.platform = subPlatform;
        OnLandPlatformFromJump(platform);
        OnEnterPlatform(platform);
    }

    protected void UpdatePostion()
    {
        var direct = platform.Direct;
        if(direct == Platform.Direction.Horizontal)
        {
            var movement = inputMove.InputSpeed.x * Time.fixedDeltaTime;
            if(platform.MoveRetIfBoundry(this, ref movement, out var newPlatform))
            {
                var prevPlatformVelocity = GetPlatformMoveSpeed();

                OnLeavePlatform(this.platform);
                this.platform = newPlatform;
                if(newPlatform != null)
                    OnEnterPlatform(this.platform);
                //TODO 这里 一个大的平台里小平台的更新要不要调用OnLandPlatform?
                //TODO 感觉是要调用的，但是没有
                if (this.platform == null)
                    OnNextPlatformNull(transform.position, prevPlatformVelocity);
                if (platform != null)
                    platform.MoveRetIfBoundry(this, ref movement, out var _);
            }

        }
        else if(direct == Platform.Direction.Vertical)
        {
            var movement = inputMove.InputSpeed.y* Time.fixedDeltaTime;
            if(platform.MoveRetIfBoundry(this, ref movement, out var newPlatform))
            {
                this.transform.position = newPlatform.NearestPointOnLine(transform.position);
                OnLeavePlatform(this.platform);
                this.platform = newPlatform;
                if (newPlatform != null)
                    OnEnterPlatform(this.platform);
            }

        }

    }


    public Vector2 GetPlatformMoveSpeed()
    {
        var speed = Vector2.right * inputMove.InputSpeed.x;
        speed = platform.GetCorrectedSpeed(speed);
        return speed;
    }

    public void Jump(float jumpSpeed)
    {
        if (!onPlatform)
            return;

        if (!canJump)
            return;

        if(platform.Direct == Platform.Direction.Horizontal)
        {
            var jumpVec = new Vector2(inputMove.InputSpeed.x, jumpSpeed);
            OnNextPlatformNull(transform.position, jumpVec);
            OnLeavePlatform(this.platform);
            platform = null;
        } 
        else if(platform.Direct == Platform.Direction.Vertical)
        {
            var jumpVec = new Vector2(inputMove.InputSpeed.x, jumpSpeed);
            if (jumpVec.x == 0)
                jumpVec = Vector2.zero;

            OnNextPlatformNull(transform.position, jumpVec);
            OnLeavePlatform(this.platform);
            platform = null;

        }
        
    }

}
