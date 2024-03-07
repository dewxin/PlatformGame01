using System;
using UnityEditor;
using UnityEngine;

namespace WildBoar.GUIModule
{

    [CanEditMultipleObjects]
    [CustomEditor(typeof(UIDrawCallMaker), true)]
    public partial class UIWidgetEditor :NGUIEditor 
    {
        protected UIDrawCallMaker targetWidget;

        protected static bool mUseShader = false;

        protected virtual void OnDisable()
        {
            UnityEditor.Tools.hidden = false;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            targetWidget = target as UIDrawCallMaker;
        }


        public override void OnInspectorNGUI()
        {
            DrawCustomProperties();
        }

        protected virtual void DrawCustomProperties()
        {
        }



    }
}