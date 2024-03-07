using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Windows;

namespace Assets.Scripts.ScriptableObj
{
    [CreateAssetMenu(fileName = "MoveEffect", menuName = "PlatformGameObj/Skill/Effect/MoveEffect")]
    public class MoveEffect :BaseEffect
    {
        public Vector2 Move = Vector2.right * 4;

        public override void Start()
        {
            //var end = CasterGameObj.transform.position.x + Move.x * CasterGameObj.transform.localScale.x;

            var end = TargetSceneUnit.SelfGameObj.transform.position.x;


            if (end >= CasterSceneUnit.SelfGameObj.transform.position.x)
            {
                end -= Move.x;
                CasterSceneUnit.SelfGameObj.transform.localScale = new Vector3(1, 1, 1);
            }
            else 
            {
                end += Move.x;
                CasterSceneUnit.SelfGameObj.transform.localScale = new Vector3(-1, 1, 1);
            }

            CasterSceneUnit.SelfGameObj.transform.DOMoveX(end, Duration);

        }

    }
}
