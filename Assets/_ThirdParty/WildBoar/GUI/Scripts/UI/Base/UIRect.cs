using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace WildBoar.GUIModule
{

    [ExecuteAlways]
    public sealed class UIRect : MonoBehaviour
    {
        public delegate void OnOffsetChange(Vector2 newOffset);
        public OnOffsetChange onOffestChange;

        public NUICanvas canvas;
        private EventSystem eventSystem;

        public bool useAnchor = false;

        public bool receiveRaycast = false;

        public Vector2 minAnchorRelative = Vector2.one * 0.5f; //常规 0f~1f
        public Vector2Int minAnchorAbsolute = Vector2Int.zero;

        public Vector2 maxAnchorRelative = Vector2.one * 0.5f;
        public Vector2Int maxAnchorAbsolute = Vector2Int.zero;

        //像素但是浮点数，合理吗？
        [SerializeField]
        private Vector2 offset = new Vector2(0f, 0f);

        [SerializeField]
        private Vector2 pivot = new Vector2(0.5f, 0.5f);

        [SerializeField]
        private int sortingOrder = 0;

        //既是像素，也是local Unit
        [SerializeField]
        private Vector2Int size = new Vector2Int(0, 0);



        // 射线检测和渲染都要和这个取交集
        public UIRect MaskRect { get; set; }

        private void Awake()
        {
            FindCanvasAndEventSystem();
        }

        private void Start()
        {

        }

        private void OnEnable()
        {
            if (eventSystem!= null && receiveRaycast)
                eventSystem.RaycastRectRegistry.Register(this);
        }

        private void OnDisable()
        {
            if(eventSystem!= null && receiveRaycast)
                eventSystem.RaycastRectRegistry.UnRegister(this);
        }


        private void Update()
        {
        }

        private void OnValidate()
        {

        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR

            //TODO 这里好像坏了
            {
                if (UnityEditor.Selection.activeGameObject == gameObject) return;

                Color outline = new Color(1f, 1f, 1f, 0.2f);

                //后面会被matrix矩阵转换缩小，所以这里z乘个大的数值
                Vector3 center = new Vector3(RenderTargetOffset.x, RenderTargetOffset.y, -sortingOrder*100);
                Vector3 size = new Vector3(Width, Height, 1f);

                // Draw the gizmo
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.color = (UnityEditor.Selection.activeGameObject == gameObject) ? Color.white : outline;
                Gizmos.DrawWireCube(center, size);

                // Make the widget selectable ** 很重要 **
                size.z = 100f;
                Gizmos.color = Color.clear;
                Gizmos.DrawCube(center, size);

                Gizmos.matrix = Matrix4x4.identity;
            }
#endif
        }

        public Vector2 RenderTargetOffset
        {
            get => offset;

            set
            {
                offset = value;
                InvokeOnRectChange();
                if (onOffestChange != null) onOffestChange(offset);
            }
        }

        public  Vector2 RenderTargetPivot
        {
            get => pivot;
        }

        public int SortingOrder
        {
            get => sortingOrder;
            set
            {
                sortingOrder = value;
                InvokeOnRectChange();
            }
        }

        public Vector2Int Size
        {
            get { return size; }
            set 
            {
                var originPivot = pivot;
                size = value;
                SetPivotKeepPivotPosStill(originPivot);
            }
        }
        public int Width
        {
            get { return Size.x; }
            set { Size = new Vector2Int(value,Size.y);}
        }
        public int Height
        {
            get { return size.y; }
            set { Size = new Vector2Int(Size.x,value);}
        }

        public RectCorners RenderTargetLocalCorners
        {
            get
            {
                RectCorners rectCorner = new RectCorners();

                float x0 = RenderTargetOffset.x - 0.5f * size.x;
                float y0 = RenderTargetOffset.y - 0.5f * size.y;
                float x1 = x0 + size.x;
                float y1 = y0 + size.y;

                rectCorner.BottomLeft = new Vector2(x0, y0);
                rectCorner.TopLeft = new Vector2(x0, y1);
                rectCorner.TopRight = new Vector2(x1, y1);
                rectCorner.BottomRight = new Vector2(x1, y0);

                return rectCorner;
            }
        }

        public RectCorners RenderTargetWorldCorners
        {
            get
            {
                var localBounds = RenderTargetLocalCorners;
                localBounds.BottomLeft = transform.TransformPoint(localBounds.BottomLeft);
                localBounds.TopLeft = transform.TransformPoint(localBounds.TopLeft);
                localBounds.TopRight = transform.TransformPoint(localBounds.TopRight);
                localBounds.BottomRight = transform.TransformPoint(localBounds.BottomRight);

                return localBounds;
            }
        }


        private void InvokeOnRectChange()
        {
            var rectUser = GetComponent<UIRectUser>();
            if (rectUser != null)
            {
                rectUser.OnRectChange();
            }
        }


        public void SetPivotKeepRenderTargetStill(Vector2 value)
        {
            //设置pivot需要保证 RenderTarget的world position保持不变
            pivot = value;
            var originOffset = RenderTargetOffset;

            RenderTargetOffset = (Vector2.one * 0.5f - value) * size;

            Vector3 movement = RenderTargetOffset - originOffset;
            this.transform.localPosition -= movement;
            InvokeOnRectChange();
        }

        public void SetPivotKeepPivotPosStill(Vector2 value)
        {
            pivot = value;
            RenderTargetOffset = (Vector2.one * 0.5f - value) * size;
            InvokeOnRectChange();
        }

        private void FindCanvasAndEventSystem()
        {
            canvas = GetComponentInParent<NUICanvas>();
            if(canvas != null ) 
                eventSystem = canvas.GetComponent<EventSystem>();
        }

    }
}
