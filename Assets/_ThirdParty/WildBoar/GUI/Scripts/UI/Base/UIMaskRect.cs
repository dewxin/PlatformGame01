using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WildBoar.GUIModule
{
    public class UIMaskRect:UIDrawCallMaker
    {
        private const int MaskID = 1;

        public override UIMeshInfo GetMeshInfo()
        {
            var rectCorners = RenderTargetWorldCorners;

            var geometry = new UIMeshInfo();

            geometry.vertexList = rectCorners.GetVertexList();
            return geometry;
        }

        public override void SetStencilMask(int maskID)
        {
            //ignore
        }

        public override UIDrawCall TryGenerateDrawCall()
        {
            var drawCall = UIDrawCall.Create(this);
            var shader = Shader.Find("Unlit/StencilMask");
            var material = new Material(shader);
            drawCall.mRenderer.material = material;
            material.SetInt("_StencilRef", MaskID);
            return drawCall;
        }

        private void OnEnable()
        {
            var myRect = GetComponent<UIRect>();
            var rectList = GetComponentsInChildren<UIRect>();
            foreach (var rect in rectList)
            {
                if (rect != myRect)
                    rect.MaskRect = myRect;
            }

            var drawCallMakerList = GetComponentsInChildren<UIDrawCallMaker>();
            foreach (var drawCallMaker in drawCallMakerList)
            {
                drawCallMaker.SetStencilMask(MaskID);
            }
        }

        private void OnDisable()
        {
            var myRect = GetComponent<UIRect>();
            var rectList = GetComponentsInChildren<UIRect>();
            foreach (var rect in rectList)
            {
                if (rect != myRect)
                    rect.MaskRect = null;
            }
        }

        private void Awake()
        {


        }
    }
}
