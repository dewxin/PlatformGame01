using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ScriptableObj
{
    public abstract class VisualEffect:BaseEffect
    {
        public SpriteRenderer CasterSpriteRenderer { get; set; }
        public SpriteRenderer TargetSpriteRenderer { get; set; }

        public override void Start()
        {
            base.Start();

            CasterSpriteRenderer = CasterSceneUnit.SceneUnitInfoOwner.GetSpriteRenderer();
            TargetSpriteRenderer = TargetSceneUnit.SceneUnitInfoOwner.GetSpriteRenderer();
        }

    }
}
