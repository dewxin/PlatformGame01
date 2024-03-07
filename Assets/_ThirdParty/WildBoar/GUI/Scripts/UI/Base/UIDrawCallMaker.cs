//-------------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2023 Tasharen Entertainment Inc
//-------------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using System.Linq;


namespace WildBoar.GUIModule
{
	[ExecuteAlways]
	[RequireComponent(typeof(UIRect))]
	public abstract class UIDrawCallMaker : UIRectUser
	{

		public UIDrawCall DrawCall { get; set; }


		protected virtual void Start()
		{
            if (DrawCall == null)
                DrawCall = TryGenerateDrawCall();
        }




        public abstract void SetStencilMask(int maskID);
        public abstract UIDrawCall TryGenerateDrawCall();
		public abstract UIMeshInfo GetMeshInfo();

        public override void OnRectChange()
        {
			if(DrawCall != null)
				DrawCall.UpdateOnNewRect(Rect);

        }


		public void SetPropertyBlock(MaterialPropertyBlock propertyBlock)
		{
			if(DrawCall== null)
			{
                DrawCall = TryGenerateDrawCall();
            }
			DrawCall.SetPropertyBlock(propertyBlock);
		}



    }
}
