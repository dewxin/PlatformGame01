using Assets.Scripts.ScriptableObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ShortCutSlotItem
{
    public KeyCode KeyCode;

    public Skill_ScriptableObj Skill;

    public PlayerSingleton player => PlayerSingleton.Instance;

    public bool TryUseAndRetCoolDown(out float coolDown)
    {
        if(Skill !=null && player.State.CanUseSkill() && Skill.CanUseSkill(player.PlayerMain.SceneUnitInfo))
        {
            player.PlayerMain.SceneUnitSkill.UseSkill(Skill);

            coolDown= Skill.CoolDown;
            return true;
        }

        coolDown= 0;
        return false;
    }


}
