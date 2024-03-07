using I18N.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class PatrolState : PatrolStateBase, IPlatformUser
{

    public new SimpleStateManager StateManager => base.Manager as SimpleStateManager;

    private Parameter Parameter => StateManager.Parameter;

    private Platform platform => StateManager.Platform;

    public Vector3 Position { get => StateManager.transform.position; set => StateManager.transform.position=value; }

    private int _Direction = 1;
    public int Direction 
    {
        get => Math.Sign( _Direction);
        set
        {
            _Direction= value;

            var x = Math.Sign(platform.LineDir.x * _Direction);
            StateManager.AvatarHolder.transform.localScale = new Vector3(x,1,1);

        }
    }
    private float distance = 0;

    public override void OnEnter()
    {
        StateManager.Play(AvatarAnimEnum.Walk02);

        SetDirection();
    }



    public override void OnUpdate()
    {

        var movement = Parameter.moveSpeed * Time.deltaTime* Direction;
        distance += Math.Abs(movement);
        if(platform.MoveRetIfBoundry(this, ref movement, out var _))
        {
            ReachPlatformEnd();
        }


    }

    public override void OnExit()
    {
        distance = 0;
    }

    private void SetDirection()
    {
        Direction = -1;
        if (Random.Range(-1f, 1f) > 0)
            Direction = 1;
    }


    private void ReachPlatformEnd()
    {
        Direction = -Direction;
    }

    public bool CanExit()
    {
        if(distance > Parameter.restAfterMoveThisDistance)
            return true;

        return false;
    }


}
