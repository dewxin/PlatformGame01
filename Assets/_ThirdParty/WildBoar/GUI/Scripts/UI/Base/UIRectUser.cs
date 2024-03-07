using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WildBoar.GUIModule
{
    [RequireComponent(typeof(UIRect))]
    public abstract class UIRectUser : MonoBehaviour
    {
        private UIRect rect;
        public UIRect Rect
        {
            get
            {
                if (rect == null)
                    rect = GetComponent<UIRect>();

                return rect;
            }
        }


        // Widget's width in pixels.
        public int Width
        {
            get => Rect.Width;
            set => Rect.Width = value;
        }

        // Widget's height in pixels.
        public int Height
        {
            get => Rect.Height;
            set => Rect.Height = value;
        }


        public Vector2 RenderTargetOffset
        {
            get => Rect.RenderTargetOffset;
            set => Rect.RenderTargetOffset = value;
        }


        public Vector2Int Size
        {
            get => Rect.Size;
            set => Rect.Size = value;
        }

        public RectCorners RenderTargetWorldCorners => Rect.RenderTargetWorldCorners;
        public RectCorners RenderTargetLocalCorners => Rect.RenderTargetLocalCorners;

        public UIRect.OnOffsetChange onOffestChange
        {
            get => Rect.onOffestChange;
            set => Rect.onOffestChange = value;
        }

        public Vector2 Pivot
        {
            get => Rect.RenderTargetPivot;
            set => Rect.SetPivotKeepRenderTargetStill(value);
        }

        public int SortingOrder => Rect.SortingOrder;

        public virtual void OnRectChange()
        {

        }



    }
}
