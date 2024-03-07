using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ScriptableObj
{
    [CreateAssetMenu(fileName = "NeedTargetPreconditon", menuName = "PlatformGameObj/Skill/Preconditon/NeedTargetPreconditon")]
    public class NeedTargetPreconditon : BasePrecondition
    {
        public override bool CheckPass(SceneUnitInfo sceneUnitInfo)
        {
            return sceneUnitInfo.Target != null;
        }
    }
}
