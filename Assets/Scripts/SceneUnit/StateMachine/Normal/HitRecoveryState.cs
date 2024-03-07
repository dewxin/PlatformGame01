using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class HitRecoveryState : HitRecoveryBase
{
    public float timer { get; private set; }

    private SimpleStateManager stateManager => Manager as SimpleStateManager; 

    public HitRecoveryState()
    {
    }


    public override void OnEnter()
    {
        stateManager.Play(AvatarAnimEnum.Hit07);
    }

    public override void OnUpdate()
    {
        timer += Time.deltaTime;
    }

    public override void OnExit()
    {
        timer = 0;
    }

}
