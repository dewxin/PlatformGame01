//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace WildBoar.GUIModule
//{
//    internal class SpriteFillTool
//    {

//        #region Fill Functions

//        protected void CacheDataInGeometryInner(Rect uvOuter, Rect uvInner)
//        {
//            switch (type)
//            {
//                case Type.Simple:
//                    SimpleFill(uvOuter);
//                    break;

//                case Type.Sliced:
//                //	SlicedFill(verts, uvs, cols, ref v, ref u, ref c);
//                //	break;

//                case Type.Filled:
//                    FilledFill(uvOuter);
//                    break;

//                case Type.Tiled:
//                    //	TiledFill(verts, uvs, cols, ref v, ref c);
//                    //	break;

//                    //TODO 这种每帧一刷的，改成UI提示错误
//                    Debug.Log($"{type} Not implemented");
//                    break;
//            }
//        }





//        /*
//		protected void SlicedFill(List<Vector3> verts, List<Vector2> uvs, List<Color> cols, ref Vector4 v, ref Vector4 u, ref Color gc)
//		{
//			var br = border * pixelSize;

//			if (br.x == 0f && br.y == 0f && br.z == 0f && br.w == 0f)
//			{
//				SimpleFill(verts, uvs, cols, ref v, ref u, ref gc);
//				return;
//			}

//			mTempPos[0].x = v.x;
//			mTempPos[0].y = v.y;
//			mTempPos[3].x = v.z;
//			mTempPos[3].y = v.w;

//			if (mFlip == Flip.Horizontally || mFlip == Flip.Both)
//			{
//				mTempPos[1].x = mTempPos[0].x + br.z;
//				mTempPos[2].x = mTempPos[3].x - br.x;

//				mTempUVs[3].x = mOuterUV.xMin;
//				mTempUVs[2].x = mInnerUV.xMin;
//				mTempUVs[1].x = mInnerUV.xMax;
//				mTempUVs[0].x = mOuterUV.xMax;
//			}
//			else
//			{
//				mTempPos[1].x = mTempPos[0].x + br.x;
//				mTempPos[2].x = mTempPos[3].x - br.z;

//				mTempUVs[0].x = mOuterUV.xMin;
//				mTempUVs[1].x = mInnerUV.xMin;
//				mTempUVs[2].x = mInnerUV.xMax;
//				mTempUVs[3].x = mOuterUV.xMax;
//			}

//			if (mFlip == Flip.Vertically || mFlip == Flip.Both)
//			{
//				mTempPos[1].y = mTempPos[0].y + br.w;
//				mTempPos[2].y = mTempPos[3].y - br.y;

//				mTempUVs[3].y = mOuterUV.yMin;
//				mTempUVs[2].y = mInnerUV.yMin;
//				mTempUVs[1].y = mInnerUV.yMax;
//				mTempUVs[0].y = mOuterUV.yMax;
//			}
//			else
//			{
//				mTempPos[1].y = mTempPos[0].y + br.y;
//				mTempPos[2].y = mTempPos[3].y - br.w;

//				mTempUVs[0].y = mOuterUV.yMin;
//				mTempUVs[1].y = mInnerUV.yMin;
//				mTempUVs[2].y = mInnerUV.yMax;
//				mTempUVs[3].y = mOuterUV.yMax;
//			}

//			for (int x = 0; x < 3; ++x)
//			{
//				int x2 = x + 1;

//				for (int y = 0; y < 3; ++y)
//				{
//					if (centerType == AdvancedType.Invisible && x == 1 && y == 1) continue;

//					int y2 = y + 1;

//					verts.Add(new Vector3(mTempPos[x].x, mTempPos[y].y));
//					verts.Add(new Vector3(mTempPos[x].x, mTempPos[y2].y));
//					verts.Add(new Vector3(mTempPos[x2].x, mTempPos[y2].y));
//					verts.Add(new Vector3(mTempPos[x2].x, mTempPos[y].y));

//					uvs.Add(new Vector2(mTempUVs[x].x, mTempUVs[y].y));
//					uvs.Add(new Vector2(mTempUVs[x].x, mTempUVs[y2].y));
//					uvs.Add(new Vector2(mTempUVs[x2].x, mTempUVs[y2].y));
//					uvs.Add(new Vector2(mTempUVs[x2].x, mTempUVs[y].y));

//					if (!mApplyGradient)
//					{
//						cols.Add(gc);
//						cols.Add(gc);
//						cols.Add(gc);
//						cols.Add(gc);
//					}
//					else
//					{
//						AddVertexColours(cols, gc, x, y);
//						AddVertexColours(cols, gc, x, y2);
//						AddVertexColours(cols, gc, x2, y2);
//						AddVertexColours(cols, gc, x2, y);
//					}
//				}
//			}
//		}

//		*/

//        /// <summary>
//        /// Tiled sprite fill function.
//        /// </summary>

//        //protected void TiledFill(List<Vector3> verts, List<Vector2> uvs, List<Color> cols, ref Vector4 v, ref Color c)
//        //{
//        //	var tex = mainTexture;
//        //	if (tex == null) return;

//        //	var size = new Vector2(mInnerUV.width * tex.width, mInnerUV.height * tex.height);
//        //	if (size.x < 2f || size.y < 2f) return;

//        //	Vector4 u;
//        //	Vector4 p;
//        //	var padding = this.padding;

//        //	{
//        //		u.x = mInnerUV.xMin;
//        //		u.z = mInnerUV.xMax;

//        //		p.x = padding.x ;
//        //		p.z = padding.z ;
//        //	}

//        //	{
//        //		u.y = mInnerUV.yMin;
//        //		u.w = mInnerUV.yMax;

//        //		p.y = padding.y;
//        //		p.w = padding.w;
//        //	}

//        //	float x0 = v.x;
//        //	float y0 = v.y;
//        //	float u0 = u.x;
//        //	float v0 = u.y;

//        //	while (y0 < v.w)
//        //	{
//        //		y0 += p.y;
//        //		x0 = v.x;
//        //		float y1 = y0 + size.y;
//        //		float v1 = u.w;

//        //		if (y1 > v.w)
//        //		{
//        //			v1 = Mathf.Lerp(u.y, u.w, (v.w - y0) / size.y);
//        //			y1 = v.w;
//        //		}

//        //		while (x0 < v.z)
//        //		{
//        //			x0 += p.x;
//        //			float x1 = x0 + size.x;
//        //			float u1 = u.z;

//        //			if (x1 > v.z)
//        //			{
//        //				u1 = Mathf.Lerp(u.x, u.z, (v.z - x0) / size.x);
//        //				x1 = v.z;
//        //			}

//        //			verts.Add(new Vector3(x0, y0));
//        //			verts.Add(new Vector3(x0, y1));
//        //			verts.Add(new Vector3(x1, y1));
//        //			verts.Add(new Vector3(x1, y0));

//        //			uvs.Add(new Vector2(u0, v0));
//        //			uvs.Add(new Vector2(u0, v1));
//        //			uvs.Add(new Vector2(u1, v1));
//        //			uvs.Add(new Vector2(u1, v0));

//        //			cols.Add(c);
//        //			cols.Add(c);
//        //			cols.Add(c);
//        //			cols.Add(c);

//        //			x0 += size.x + p.z;
//        //		}

//        //		y0 += size.y + p.w;
//        //	}
//        //}

//        // Filled sprite fill function.
//        protected UIGeometry FilledFill(Rect uvOut)
//        {

//            var rectCorners = RenderTargetWorldCorners;

//            var geometry = new UIGeometry();

//            var verts = geometry.vertextList;
//            var uvs = geometry.uvList;
//            var cols = geometry.colorList;

//            if (mFillDirection == FillDirection.Horizontal)
//            {
//                var width = rectCorners.Size.x;
//                rectCorners.TopRight = rectCorners.TopLeft + width * Vector3.right * fillAmount;
//                rectCorners.BottomRight = rectCorners.BottomLeft + width * Vector3.right * fillAmount;

//                uvOut.xMax = uvOut.xMin + (uvOut.width) * mFillAmount;
//            }
//            if (mFillDirection == FillDirection.Vertical)
//            {
//                var height = rectCorners.Size.y;
//                rectCorners.TopRight = rectCorners.BottomRight + height * Vector3.up * fillAmount;
//                rectCorners.TopLeft = rectCorners.BottomLeft + height * Vector3.up * fillAmount;

//                uvOut.yMax = uvOut.yMin + (uvOut.height) * mFillAmount;
//            }


//            verts.Add(rectCorners.BottomLeft);
//            verts.Add(rectCorners.TopLeft);
//            verts.Add(rectCorners.TopRight);
//            verts.Add(rectCorners.BottomRight);

//            uvs.Add(new Vector2(uvOut.xMin, uvOut.yMin)); //Left Bottom
//            uvs.Add(new Vector2(uvOut.xMin, uvOut.yMax)); //Left Top
//            uvs.Add(new Vector2(uvOut.xMax, uvOut.yMax));
//            uvs.Add(new Vector2(uvOut.xMax, uvOut.yMin));

//            {
//                AddVertexColours(cols, color, 1, 1);
//                AddVertexColours(cols, color, 1, 2);
//                AddVertexColours(cols, color, 2, 2);
//                AddVertexColours(cols, color, 2, 1);
//            }

//            return geometry;

//        }

//        #endregion // Fill functions
//    }
//}
