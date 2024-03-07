using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ScriptableObj
{
    [CreateAssetMenu(fileName = "SubSkill", menuName = "PlatformGameObj/Skill/SubSkill", order = 1)]
    public class SubSkill_ScriptableObj:ScriptableObject
    {
        public Skill_ScriptableObj parent;

        public float Duration = 1; //second

        //比如 0-1 0-2 0-3 0-4
        //不同部件都会使用这个 id
        public ActionAnimation ActionAnimation;
        public string SoundId;

        public int Damage;

        public List<BaseEffect> SelfEffectList = new List<BaseEffect>();

        public List<AnimEventEffect> EffectOnEventList = new List<AnimEventEffect>();

    }
}
