using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;


public class PlayerInputMove : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3f;
    [SerializeField]
    private float climbSpeed = 1f;

    [SerializeField]
    private float jumpSpeed = 5f;

    public bool onPlatform => platformRigidbody.onPlatform;

    private PlatformRigidBody platformRigidbody;
    private GravityRigidBody gravityRigidbody;
    private SceneUnitAvatarHolder avatarHolder;


    public float inputX { get; private set; }
    public float inputY { get; private set; }

    public Vector2 InputSpeed => Vector2.right * moveSpeed * inputX + Vector2.up * inputY * climbSpeed;

    private Platform climbPlatform;
    private Vector3 needClimbPos;

    void Start()
    {
        platformRigidbody= GetComponent<PlatformRigidBody>();
        gravityRigidbody = GetComponent<GravityRigidBody>();
        avatarHolder = GetComponent<SceneUnitAvatarHolder>();

        DontDestroyOnLoad(this.gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        inputX = 0;
        inputY = 0;

        if (InputManager.Instance.GetInput(InputEnum.MoveLeft))
            inputX = -1;
        if (InputManager.Instance.GetInput(InputEnum.MoveRight))
            inputX = 1;
        if (InputManager.Instance.GetInput(InputEnum.MoveUp))
            inputY = 1;
        if (InputManager.Instance.GetInput(InputEnum.MoveDown))
            inputY = -1;

        if(PlayerSingleton.Instance.State.CanInput())
        {
            HandleRun();
            HandleJump();
            HandleClimb();
        }


        ResetInputWhenBlock();
    }

    private void HandleRun()
    {
        FlipSprite();
        ChangeAnimation();
    }

    private void ResetInputWhenBlock()
    {
        foreach(var blockCollider in BlockCollider.List)
        {
            if (blockCollider.BoxCollider == null)
                continue;

            if(blockCollider.BoxCollider.bounds.Contains(transform.position))
            {
                if(gravityRigidbody.velocity.x * blockCollider.ForbiddenVector().x > 0)
                    gravityRigidbody.velocity.x = 0;

                if (inputX * blockCollider.ForbiddenVector().x > 0)
                    inputX = 0;

            }
        }

    }


    private void FlipSprite()
    {
        if (!onPlatform)
            return;
        if(inputX > 0.01f)
        {
            this.transform.localScale = new Vector3(1, 1,1);
        }
        else if(inputX < -0.01f)
        {
            this.transform.localScale = new Vector3(-1, 1,1);
        }
    }
    private void ChangeAnimation()
    {
        if(onPlatform)
        {
            if(platformRigidbody.PlatformDirect == Platform.Direction.Horizontal)
            {
                if (inputX == 0)
                {
                    if(PlayerSingleton.Instance.State.IsFighting)
                        avatarHolder.Play(AvatarAnimEnum.BattleIdle01);
                    else
                        avatarHolder.Play(AvatarAnimEnum.Idle00);
                }
                else if (Mathf.Abs(inputX) > 0.01f)
                {
                    avatarHolder.Play(AvatarAnimEnum.Run03);
                }
            }
            else if (platformRigidbody.PlatformDirect == Platform.Direction.Vertical)
            {
                avatarHolder.Play(AvatarAnimEnum.Climb06);
                if (inputY == 0)
                    avatarHolder.Pause();
                else
                    avatarHolder.Resume();

            }

        }
        else
        {
            avatarHolder.Play(AvatarAnimEnum.Jump05);
        }
    }

    private void HandleJump()
    {
        if(InputManager.Instance.GetInputDown(InputEnum.Jump))
        {
            platformRigidbody.Jump(jumpSpeed);
            avatarHolder.Play(AvatarAnimEnum.Jump05);
        }
    }


    private void HandleClimb()
    {
        bool needClimbWhenOnPlatform = (inputY != 0) && (onPlatform);
        bool needClimbWhenJump = (inputY > 0);

        bool needClimb = needClimbWhenJump || needClimbWhenOnPlatform;

        if (!needClimb)
        {
            climbPlatform = null;

            return;
        }

        //need climb


        if (climbPlatform == null)
        {
            //Try find climbPlaytform
            var collider = gravityRigidbody.CheckVerticalPlatform(Vector2.up* Mathf.Sign(inputY));
            if(collider != null)
            {
                climbPlatform = collider.GetComponent<Platform>();
                needClimbPos = this.transform.position;

            }
        }


        //检查是不是能够挂上去
        if (climbPlatform != null)
        {
            var platformX = climbPlatform.transform.position.x;
            var xDiffNeed = needClimbPos.x - platformX;
            var xDiff = transform.position.x - platformX;

            //到vertical platform位置了
            if (xDiff * xDiffNeed <=0)
            {
                this.transform.position = climbPlatform.NearestPointOnLine(this.transform.position);
                platformRigidbody.LandPlatformFromJump(climbPlatform);
            }

        }

    }



}
