using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ScriptableObj
{
    [CreateAssetMenu(fileName = "VisualFlashEffect", menuName = "PlatformGameObj/Skill/VisualEffect/VisualFlashEffect")]
    public class VisualFlashEffect : VisualEffect
    {
        private Material rendererOriginMaterial { get; set; }

        public Material material;
        public Material InstanceMaterial { get; set; }
        public Color FlashColor;

        public AnimationCurve FlashAmountCurve;

        private float totalTime = 0;

        public override void Start()
        {
            base.Start();

            //TODO 这里有时候会拿到 边缘描边的 material
            // 然后描边的material 有时候替换掉 受击闪光 的material
            rendererOriginMaterial = TargetSpriteRenderer.material;
            TargetSpriteRenderer.material = new Material(material);
            InstanceMaterial = TargetSpriteRenderer.material;


            InstanceMaterial.SetColor("_FlashColor", FlashColor);
        }

        public override void Update()
        {
            totalTime += Time.deltaTime;

            var flashAmount = FlashAmountCurve.Evaluate(totalTime);

            InstanceMaterial.SetFloat("_FlashAmount", flashAmount);

        }

        public override void OnDestroy()
        {
            if(TargetSpriteRenderer!= null)
                TargetSpriteRenderer.material = rendererOriginMaterial;
        }
    }
}
