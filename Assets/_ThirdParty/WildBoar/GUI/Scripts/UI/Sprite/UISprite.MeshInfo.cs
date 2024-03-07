//-------------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2023 Tasharen Entertainment Inc
//-------------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using System.IO;
using UnityEngine.UIElements;
using System;

namespace WildBoar.GUIModule
{

	public partial class UISprite
	{

		[SerializeField]
		public UISpriteFillMethod fillMethod = UISpriteFillMethod.Radial360;

		[SerializeField]
		private float fillAmount = 1;

        public float FillAmount => fillAmount;


        public void SetFillAmount(float fillAmount)
        {
            this.fillAmount = Mathf.Clamp01(fillAmount);
            RefreshDrawCall();
        }



        #region Simple

        private UIMeshInfo GetMeshInfoSimple()
        {

            var uvRect = NGUIMath.ConvertToTexCoords(SpriteData.Rect, atlasTexture.width, atlasTexture.height);

            return GetMeshInfoWithUvRect(uvRect, RenderTargetWorldCorners);
        }


        protected UIMeshInfo GetMeshInfoWithUvRect(Rect uvRect, RectCorners rectCorners)
        {
            var meshInfo = new UIMeshInfo();

            meshInfo.vertexList = rectCorners.GetVertexList();

            meshInfo.indexList = new List<int>()
            {
                0, 1, 2, 0, 2, 3
            };

            var uvs = meshInfo.uvList;
            uvs.Add(new Vector2(uvRect.xMin, uvRect.yMin)); //Left Bottom
            uvs.Add(new Vector2(uvRect.xMin, uvRect.yMax)); //Left Top
            uvs.Add(new Vector2(uvRect.xMax, uvRect.yMax));
            uvs.Add(new Vector2(uvRect.xMax, uvRect.yMin));


            var colorList = meshInfo.colorList;

            colorList.Add(color);
            colorList.Add(color);
            colorList.Add(color);
            colorList.Add(color);

            return meshInfo;
        }


        #endregion



        #region Filled

        private UIMeshInfo GetMeshInfoFilled()
        {
            if (fillMethod == UISpriteFillMethod.Radial360)
                return GetMeshInfoFilled_Radial360();
            else if (fillMethod == UISpriteFillMethod.Horizontal)
                return GetMeshInfoFilled_Horizontal();

            return null;
        }

        private UIMeshInfo GetMeshInfoFilled_Horizontal()
        {
            var croppedRect = SpriteData.Rect;
            croppedRect.width *= fillAmount;

            var uvRect = NGUIMath.ConvertToTexCoords(croppedRect, atlasTexture.width, atlasTexture.height);
            var rectCorners = RenderTargetWorldCorners;

            rectCorners.TopRight = rectCorners.TopLeft + (rectCorners.TopRight - rectCorners.TopLeft) * fillAmount;
            rectCorners.BottomRight = rectCorners.BottomLeft + (rectCorners.BottomRight - rectCorners.BottomLeft) * fillAmount;

            return GetMeshInfoWithUvRect(uvRect, rectCorners);
        }


        private UIMeshInfo GetMeshInfoFilled_Radial360()
        {
            var uvRect = NGUIMath.ConvertToTexCoords(SpriteData.Rect, atlasTexture.width, atlasTexture.height);

            var rectCorners = RenderTargetLocalCorners;
            var meshInfo = new UIMeshInfo();

            var vertexBeforeCutList = new List<Vector3>
            {
                (rectCorners.TopLeft + rectCorners.TopRight) / 2, //↑
                rectCorners.TopRight, // ↗
                (rectCorners.TopRight + rectCorners.BottomRight) / 2, //→
                rectCorners.BottomRight, //↘
                (rectCorners.BottomLeft + rectCorners.BottomRight) / 2, //↓
                rectCorners.BottomLeft, //↙
                (rectCorners.BottomLeft + rectCorners.TopLeft) / 2, //←
                rectCorners.TopLeft, //↖
            };

            var uvBeforeCutList = new List<Vector2>
            {
                new Vector2(uvRect.center.x, uvRect.yMax),  //↑
                new Vector2(uvRect.xMax, uvRect.yMax), // ↗
                new Vector2(uvRect.xMax, uvRect.center.y), //→
                new Vector2(uvRect.xMax, uvRect.yMin), //↘
                new Vector2(uvRect.center.x, uvRect.yMin), //↓
                new Vector2(uvRect.xMin, uvRect.yMin), //↙
                new Vector2(uvRect.xMin, uvRect.center.y), //←
                new Vector2(uvRect.xMin, uvRect.yMax), //↖

            };


            fillAmount = Mathf.Clamp01(fillAmount);
            var angle = fillAmount * 360;


            var vertexAfterCutList = new List<Vector3>();
            var uvAfterCutList = new List<Vector2>();
            var colorList = new List<Color>();
            vertexAfterCutList.Add(rectCorners.Center);
            uvAfterCutList.Add(uvRect.center);
            colorList.Add(color);

            var indexList = new List<int>();
            int currentIndex = 0;


            int startPointIndex;
            Vector3 vertex = GetPointOnRect(rectCorners.Center, angle, rectCorners.Size / 2,out startPointIndex);

            //第一个点的uv对齐
            int firstIndex = startPointIndex % vertexBeforeCutList.Count;
            int nextIndex = startPointIndex - 1;
            float f = (vertex - vertexBeforeCutList[firstIndex]).magnitude / (vertexBeforeCutList[nextIndex] - vertexBeforeCutList[firstIndex]).magnitude;
            var uv = uvBeforeCutList[nextIndex] * f + (1 - f) * uvBeforeCutList[firstIndex];

            vertexAfterCutList.Add(vertex);
            uvAfterCutList.Add(uv);
            colorList.Add(color);
            currentIndex++;
            indexList.Add(0);
            indexList.Add(currentIndex);
            indexList.Add(currentIndex + 1);

            //处理第二个点直到最后一个点
            for (int i = startPointIndex; i < vertexBeforeCutList.Count; i++)
            {

                vertex = vertexBeforeCutList[i];
                uv = uvBeforeCutList[i];

                vertexAfterCutList.Add(vertex);
                uvAfterCutList.Add(uv);
                colorList.Add(color);
                currentIndex++;
                indexList.Add(0);
                indexList.Add(currentIndex);
                indexList.Add(currentIndex+1);
            }

            vertexAfterCutList.Add(vertexBeforeCutList[0]);
            uvAfterCutList.Add(uvBeforeCutList[0]);
            colorList.Add(color);

            meshInfo.uvList = uvAfterCutList;
            meshInfo.vertexList = vertexAfterCutList;

            for(int i = 0; i < vertexAfterCutList.Count; i++)
            {
                vertexAfterCutList[i] = transform.TransformPoint(vertexAfterCutList[i]);
            }
            meshInfo.colorList = colorList;

            meshInfo.indexList = indexList;


            return meshInfo;
        }

        private Vector2 GetPointOnRect(Vector2 center, float angle, Vector2 halfSize, out int nextPointIndex)
        {
            var radian = angle * Mathf.Deg2Rad;

            float leftTopAngle = Mathf.Atan(halfSize.x / halfSize.y) * Mathf.Rad2Deg;
            float rightTopAngle = 360 - leftTopAngle;
            float leftDownAngle = 180 - leftTopAngle;
            float rightDownAngle = 180 + leftTopAngle;

            //https://python.tutorialink.com/finding-points-on-a-rectangle-at-a-given-angle/
            if(angle<=leftTopAngle)
            {
                nextPointIndex = 8;
                return new Vector2(center.x - halfSize.y * Mathf.Tan(radian), center.y + halfSize.y);
            }
            else if (leftTopAngle <= angle && angle <= 90)
            {
                nextPointIndex = 7;
                return new Vector2(center.x - halfSize.x, center.y + halfSize.x / Mathf.Tan(radian));
            }
            else if (90 <= angle && angle <= leftDownAngle)
            {
                nextPointIndex = 6;
                return new Vector2(center.x - halfSize.x, center.y + halfSize.x / Mathf.Tan(radian));
            }
            else if(leftDownAngle < angle && angle <= 180)
            {
                nextPointIndex = 5;
                return new Vector2(center.x + halfSize.y * Mathf.Tan(radian), center.y - halfSize.y);
            }
            else if(180<=angle && angle<= rightDownAngle)
            {
                nextPointIndex = 4;
                return new Vector2(center.x + halfSize.y * Mathf.Tan(radian), center.y - halfSize.y);
            }
            else if(rightDownAngle <= angle && angle <= 270)
            {
                nextPointIndex= 3;
                return new Vector2(center.x + halfSize.x, center.y - halfSize.x / Mathf.Tan(radian));
            }
            else if(270 <= angle && angle <= rightTopAngle)
            {
                nextPointIndex= 2;
                return new Vector2(center.x + halfSize.x, center.y - halfSize.x / Mathf.Tan(radian));
            }
            else if (rightTopAngle <= angle)
            {
                nextPointIndex = 1;
                return new Vector2(center.x - halfSize.y * Mathf.Tan(radian), center.y + halfSize.y);
            }

            throw new Exception("shouldnt reach here");
        }

        #endregion

    }
}
