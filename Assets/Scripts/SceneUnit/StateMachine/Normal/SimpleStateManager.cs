using Assets.Scripts.ScriptableObj;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class Parameter
{
    public float restAfterMoveThisDistance = 3f;
    public float idleTime = 4f;

    //todo 这么配置是不是有问题
    public float hitRecoveryTime = 1f;

    public float deathTime = 5f;

    public float moveSpeed = 0.5f;

}

public class SimpleStateManager: StateManagerBase
{
    public Parameter Parameter = new Parameter();

    private Platform _Platform;
    public Platform Platform
    {
        get
        {
            if (_Platform == null)
            {
                FindPaltform();
            }
            return _Platform;

        }
        private set => _Platform = value;
    }

    private SceneUnitAvatarHolder _AvatarHolder;
    public SceneUnitAvatarHolder AvatarHolder 
    {
        get
        {
            if(_AvatarHolder == null)
            {
                FindAvatarHolder();
            }
            return _AvatarHolder;
        }
        private set => _AvatarHolder = value;
    }

    public void Awake()
    {
    }

    public void Start()
    {
        FindPaltform();
        FindAvatarHolder();

        AddState(new IdleState());
        AddState(new PatrolState());
        AddState(new HitRecoveryState());
        AddState(new DeathState());

        EnterNewState(StateType.Idle);
    }

    private void FindAvatarHolder()
    {
        AvatarHolder = GetComponentInChildren<SceneUnitAvatarHolder>();
    }

    private void FindPaltform()
    {
         var hit = Physics2D.Raycast(transform.position , Vector2.down, 100f, LayerMask.GetMask("Platform"));
        if (hit.collider == null)
            Debug.LogError($"{gameObject.name} cannot find platform");

        Platform = hit.transform.GetComponent<Platform>();

        this.transform.position = Platform.NearestPointOnLine(transform.position);

    }

    protected override bool TryGetNewState(out StateType newState)
    {
        if (currentState is PatrolState)
        {
            var patrolState = (PatrolState)currentState;
            if (patrolState.CanExit())
            {
                newState = StateType.Idle;
                return true;
            }
        }

        else if (currentState is IdleState)
        {
            var idleState = (IdleState)currentState;
            if (idleState.timer >= Parameter.idleTime)
            {
                newState = StateType.Patrol;
                return true;
            }
        }

        else if(currentState is HitRecoveryState)
        {
            var hitRecoveryState = (HitRecoveryState)currentState;
            if (hitRecoveryState.timer >= Parameter.hitRecoveryTime)
            {
                newState = StateType.Idle;
                return true;
            }
        }

        else if (currentState is DeathState)
        {
            var deathState = (DeathState)currentState;
            if(deathState.timer >= Parameter.deathTime)
            {
                OnDeathStateOver();
            }

        }


        newState = StateType.None;
        return false;
    }


    public void Play(AvatarAnimEnum avatarAnimEnum, bool once = false, Action callbackOnFinishing = null)
    {
        var animation = CreateAnimtion(avatarAnimEnum);
        AvatarHolder.Play(animation, once, callbackOnFinishing);
    }

    public ActionAnimation CreateAnimtion(AvatarAnimEnum avatarAnimEnum, float framePerSecond = 10f)
    {
        var spriteHolder = AvatarHolder.GetSpriteHolderList().Single();
        return ActionAnimation.CreateMonsterAnimation(spriteHolder.avatarId, avatarAnimEnum, framePerSecond);
    }
}


