//-------------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2023 Tasharen Entertainment Inc
//-------------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using System.IO;

namespace WildBoar.GUIModule
{

	[ExecuteAlways]
	[AddComponentMenu("NGUI/UI/UISprite")]
	public partial class UISprite : UIDrawCallMaker
	{

		public SpriteData SpriteData { get; private set; }

		[SerializeField] 
		private SpriteAtlas spriteAtlas;
		public SpriteAtlas SpriteAtlas => spriteAtlas;


        [SerializeField]
		private Color color = Color.white;
		public Color Color => color;

		public Texture2D atlasTexture => spriteAtlas==null? null: spriteAtlas.Texture  ;

		public string spriteName; 

		public Vector4 SlicePadding => SpriteData.Slice9Padding;

		[SerializeField]
		public UISpriteMeshType SpriteMeshType;


		public void SetAtlas(SpriteAtlas spriteAtlas)
		{
			this.spriteAtlas= spriteAtlas;
            SpriteData = spriteAtlas.GetData(0);
		}

		public void SetSprite(string spriteName)
		{
			if(spriteAtlas == null)
			{
				Debug.LogError("sprite atlas is null");
				return;
			}

			this.spriteName = spriteName;
			TryGetSpriteDataByName();

            Width = (int)SpriteData.Rect.width;
			Height = (int)SpriteData.Rect.height;

			if (DrawCall == null)
				DrawCall = TryGenerateDrawCall();

			UpdateDrawCallOnNewSprite(DrawCall);
        }

		public void SetColor(Color color)
		{
			this.color = color;
			var geometry = GetMeshInfo();

			DrawCall.mMesh.SetColors(geometry.colorList);

		}


		protected override void Start()
		{
            TryGetSpriteDataByName();
			base.Start();

        }

		protected void Update()
		{
		}


        public void TryGetSpriteDataByName()
		{
            SpriteData spriteData;
            if (!spriteAtlas.TryGetData(spriteName, out spriteData))
            {
                Debug.LogWarning($"{spriteName} cannot be found");
                return;
            }

            SpriteData = spriteData;
        }

		public override UIDrawCall TryGenerateDrawCall()
		{
			if (spriteAtlas == null)
			{
				Debug.LogWarning($"{gameObject.name} Sprite is null");
				return null;
			}

			if(DrawCall != null)
			{
				Destroy(DrawCall);
			}

			var drawCall = UIDrawCall.Create(this);
			UpdateDrawCallOnNewAtlas(drawCall);

            UpdateDrawCallOnNewSprite(drawCall);

			return drawCall;
		}

		public void RefreshDrawCall()
		{
			if(DrawCall != null)	
				UpdateDrawCallOnNewSprite(DrawCall);
		}

		public void UpdateDrawCallOnNewAtlas(UIDrawCall drawCall)
		{
            var material = this.SpriteAtlas.Material;
            drawCall.mRenderer.sharedMaterial = material;
        }

        public void UpdateDrawCallOnNewSprite(UIDrawCall drawCall)
        {
			//这里更新了drawcall mesh 的uv 和 size 
            drawCall.UpdateOnNewRect(Rect);
            var geometry = GetMeshInfo();

            if (geometry.uvList != null)
                drawCall.mMesh.SetUVs(0, geometry.uvList);

        }

        public override UIMeshInfo GetMeshInfo()
		{
			var tex = atlasTexture;
			if (tex == null)
			{
				Debug.LogWarning("Atlas texture is null");
				return null;
			}

			if (SpriteData == null)
			{
				Debug.LogWarning($"{name} Sprite Data is null");
				return null;
			}

			if(SpriteMeshType == UISpriteMeshType.Filled)
				return GetMeshInfoFilled();
			else
				return GetMeshInfoSimple();

        }


        public override void SetStencilMask(int maskID)
        {
			if (DrawCall == null)
				DrawCall = TryGenerateDrawCall();

			var material = DrawCall.mRenderer.sharedMaterial;
			var newMaterial = new Material(material);
			newMaterial.SetInt("_StencilRef", maskID);

			DrawCall.mRenderer.sharedMaterial = newMaterial;
        }

		//TODO 似乎有些问题
		public void SetRendererSortingOrder(int order)
		{
			DrawCall.mRenderer.sortingOrder = order;
		}
    }
}
