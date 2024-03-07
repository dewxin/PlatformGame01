using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Assets.Scripts.ScriptableObj
{

    public abstract class BaseEffect :ScriptableObject
    {
        public float Duration = 1f; //second
        public float ExpireTime { get; set; } = -1;

        public SceneUnitInfo CasterSceneUnit { get; set; }
        public SceneUnitInfo TargetSceneUnit { get; set; }

        public virtual bool IsUnique => false;

        public virtual void OnAddDuplicateUnique(BaseEffect newEffect)
        {

        }

        public virtual void Start()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void OnDestroy()
        {

        }
    }
}
