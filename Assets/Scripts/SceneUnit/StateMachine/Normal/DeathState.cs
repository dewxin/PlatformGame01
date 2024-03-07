using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DeathState : DeathStateBase
{
    public float timer { get; private set; }

    private SimpleStateManager stateManager => Manager as SimpleStateManager; 

    public DeathState()
    {
    }


    public override void OnEnter()
    {

        Action playDead = () => { stateManager.Play(AvatarAnimEnum.Dead09); };
        stateManager.Play(AvatarAnimEnum.Die08, once:true, callbackOnFinishing:playDead);
        SoundManager.Instance.PlayEffect(stateManager.SceneUnitInfo.DeathSoundID);
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
