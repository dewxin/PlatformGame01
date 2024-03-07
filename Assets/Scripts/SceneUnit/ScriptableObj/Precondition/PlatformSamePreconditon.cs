using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ScriptableObj
{
    [CreateAssetMenu(fileName = "PlatformSamePreconditon", menuName = "PlatformGameObj/Skill/Preconditon/PlatformSamePreconditon")]
    public class PlatformSamePreconditon : BasePrecondition
    {
        public override bool CheckPass(SceneUnitInfo sceneUnitInfo)
        {
            return sceneUnitInfo.Platform == sceneUnitInfo.Target.Platform;
        }
    }
}
