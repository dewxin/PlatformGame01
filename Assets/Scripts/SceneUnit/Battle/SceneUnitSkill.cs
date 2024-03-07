using Assets.Scripts.ScriptableObj;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SceneUnitSkill:MonoBehaviour
{
    public SceneUnitInfo CurrentSkillTarget { get; private set; }
    public Skill_ScriptableObj CurrentSkill { get; private set; }
    public SubSkill_ScriptableObj CurrentSubSkill { get; private set; }


    public SceneUnitInfo SceneUnit { get; private set; }
    public SceneUnitEffectManager EffectManager { get; private set; }
    public SceneUnitAvatarHolder AvatarHolder { get; private set; }

    public void Start()
    {
        var hasSceneUnitInfo = GetComponent<IHasSceneUnitInfo>();
        SceneUnit = hasSceneUnitInfo.GetSceneUnit();
        EffectManager = GetComponent<SceneUnitEffectManager>();
        AvatarHolder = GetComponent<SceneUnitAvatarHolder>();
        AvatarHolder.OnTriggerAnimeEvent += HandleEvent;

    }

    public void UseSkill(Skill_ScriptableObj skill)
    {
        //TODO 感觉Target放skill里面传进来更好一点
        CurrentSkill= skill;
        CurrentSkillTarget = SceneUnit.Target;
        StartCoroutine(UseSkillInner());
    }

    private IEnumerator UseSkillInner()
    {
        foreach(var subSkill in CurrentSkill.SubSkillList)
        {
            UseSubSkill(subSkill);
            yield return new WaitForSeconds(subSkill.Duration);
        }

        CurrentSkill = null;
    }

    private void UseSubSkill(SubSkill_ScriptableObj subSkill)
    {
        CurrentSubSkill = subSkill;
        foreach (var selfEffect in subSkill.SelfEffectList)
        {
            AddEffectToManager(selfEffect);
        }

        if (!string.IsNullOrEmpty(subSkill.SoundId))
            SoundManager.Instance.PlayEffect(subSkill.SoundId);

        if(subSkill.Damage > 0)
        {
            SoundManager.Instance.PlayEffect(CurrentSkillTarget.HitSoundID);

            CurrentSkillTarget.DecreaseHP(subSkill.Damage);
        }

        AvatarHolder.Play(subSkill.ActionAnimation, true);
    }

    public void HandleEvent(AnimEventEnum animEvent)
    {
        foreach(var eventEffect in CurrentSubSkill.EffectOnEventList)
        {
            if(eventEffect.Event == animEvent)
            {
                AddEffectToManager(eventEffect.Effect);
            }

        }

    }


    private void AddEffectToManager(BaseEffect baseEffect)
    {
        var instanceEffect = Instantiate(baseEffect);
        instanceEffect.CasterSceneUnit = SceneUnit;
        instanceEffect.TargetSceneUnit = CurrentSkillTarget;
        EffectManager.AddEffect(instanceEffect);
    }
}
