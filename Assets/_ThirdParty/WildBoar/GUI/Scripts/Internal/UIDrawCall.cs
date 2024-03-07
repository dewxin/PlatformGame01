//-------------------------------------------------
//			  NGUI: Next-Gen UI kit
// Copyright © 2011-2023 Tasharen Entertainment Inc
//-------------------------------------------------

//#define SHOW_HIDDEN_OBJECTS

using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

namespace WildBoar.GUIModule
{

    [ExecuteAlways]
    [AddComponentMenu("NGUI/Internal/Draw Call")]
    public class UIDrawCall : MonoBehaviour
    {

        public Mesh mMesh;          // First generated mesh
        public MeshRenderer mRenderer;      // Mesh renderer for this screen

        public UIDrawCallMaker DrawCallMaker;

        public MaterialPropertyBlock PropertyBlock;
        void Awake()
        {
        }

        void OnEnable() { }

        void OnDisable()
        {

        }

        void OnDestroy()
        {
            NGUITools.DestroyImmediate(mMesh);
            mMesh = null;
        }


        void OnWillRenderObject()
        {

        }

        public void SetPropertyBlock(MaterialPropertyBlock propertyBlock)
        {
            mRenderer.SetPropertyBlock(propertyBlock);
            PropertyBlock = propertyBlock;
        }


        public static UIDrawCall Create(UIDrawCallMaker uiDrawCallMaker)
        {
            var name =  $"_UIDrawCall[{ uiDrawCallMaker.gameObject.name}]";


#if UNITY_EDITOR
            var go = UnityEditor.EditorUtility.CreateGameObjectWithHideFlags(name, HideFlags.DontSave | HideFlags.NotEditable, typeof(UIDrawCall));
            var drawCall = go.GetComponent<UIDrawCall>();
#else
            GameObject go = new GameObject(name);
            UIDrawCall drawCall = go.AddComponent<UIDrawCall>();
            DontDestroyOnLoad(go);
#endif

            drawCall.DrawCallMaker = uiDrawCallMaker;
            drawCall.TryMoveToPrefabScene();

            //这里需要将prefabScene中的drawCall移动到prefabObj子对象。[drawcall自己不能是root]
            //不然prefab scene刷新的时候会清空prefabObj之外的rootObj
            drawCall.transform.SetParent(uiDrawCallMaker.transform);
            drawCall.transform.localPosition = Vector3.zero;
            drawCall.transform.localScale = Vector3.one;

            drawCall.Init(uiDrawCallMaker);


            return drawCall;
        }

        private void Init(UIDrawCallMaker uiDrawCallMaker)
        {
            var drawCall = this;
            drawCall.gameObject.layer = uiDrawCallMaker.gameObject.layer;

            var mFilter = drawCall.gameObject.GetComponent<MeshFilter>();
            if (mFilter == null)
                mFilter = drawCall.gameObject.AddComponent<MeshFilter>();

            if (drawCall.mMesh == null)
            {
                drawCall.mMesh = new Mesh();
                drawCall.mMesh.name = "[NGUI] Mesh";
            }

            var meshInfo = uiDrawCallMaker.GetMeshInfo();

            drawCall.UpdateMesh(meshInfo);

            mFilter.mesh = drawCall.mMesh;

            if (drawCall.mRenderer == null)
                drawCall.mRenderer = drawCall.gameObject.AddComponent<MeshRenderer>();

            drawCall.mRenderer.sortingOrder = uiDrawCallMaker.SortingOrder;
        }


        public void Destroy()
        {
            NGUITools.DestroyImmediate(this.gameObject);
        }

        public void UpdateOnNewRect(UIRect uiRect)
        {
            var meshInfo = DrawCallMaker.GetMeshInfo();

            UpdateMesh(meshInfo);

            mRenderer.sortingOrder = uiRect.SortingOrder;
        }



        public void UpdateMesh(UIMeshInfo meshInfo)
        {
            var vertexList = meshInfo.vertexList;
            if (vertexList != null)
            {
                //这里需要设置为 localspace 坐标
                var localVertexList = new List<Vector3>();
                foreach(var v in vertexList)
                    localVertexList.Add(transform.InverseTransformPoint(v));

                mMesh.Clear();

                mMesh.SetVertices(localVertexList);
                mMesh.SetTriangles(meshInfo.indexList, 0, false);

                mMesh.SetUVs(0, meshInfo.uvList);

                mMesh.SetColors(meshInfo.colorList);

                mMesh.RecalculateNormals();
                mMesh.RecalculateBounds();
            }
        }



        private void TryMoveToPrefabScene()
        {
#if UNITY_EDITOR && UNITY_2018_3_OR_NEWER
            // We need to perform this check here and not in Create (string) to get to manager reference
            var prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
            {
                // If prefab stage exists and new daw call
                var stage = UnityEditor.SceneManagement.StageUtility.GetStageHandle(DrawCallMaker.gameObject);
                if (stage == prefabStage.stageHandle)
                {
                    UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, prefabStage.scene);
                    UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(prefabStage.scene);
                }
            }

#endif
        }

    }
}
