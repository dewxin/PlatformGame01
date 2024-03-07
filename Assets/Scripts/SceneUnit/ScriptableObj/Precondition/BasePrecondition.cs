using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ScriptableObj
{
    public abstract class BasePrecondition:ScriptableObject
    {
        public abstract bool CheckPass(SceneUnitInfo sceneUnitInfo);
    }
}
