using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WildBoar.GUIModule;

namespace Assets.Scripts.ScriptableObj
{

    [CreateAssetMenu(fileName = "Skill", menuName = "PlatformGameObj/Skill/Skill", order = 1)]
    public class Skill_ScriptableObj:ScriptableObject
    {
        public string Name;

        public SpriteAtlas SpriteAtlas;
        public string SpriteName;
        public float CoolDown; //cd second

        public string Description;
        public List<BasePrecondition> PreconditionList = new List<BasePrecondition>();
        public List<SubSkill_ScriptableObj> SubSkillList = new List<SubSkill_ScriptableObj>();

        public int CurrentSubSkillIndex { get; set; } = 0;

        public SubSkill_ScriptableObj GetFirstSubSkill()
        {
            if(CurrentSubSkillIndex >= SubSkillList.Count)
                CurrentSubSkillIndex = 0;

            return SubSkillList[CurrentSubSkillIndex];
        }

        public bool CanUseSkill(SceneUnitInfo sceneUnitInfo)
        {
            foreach(var precondition in PreconditionList)
            {
                if(!precondition.CheckPass(sceneUnitInfo)) 
                    return false;
            }

            return true;
        }
    }
}
