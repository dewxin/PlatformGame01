//-------------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2023 Tasharen Entertainment Inc
//-------------------------------------------------

using UnityEngine;
using System.Collections.Generic;

namespace WildBoar.GUIModule
{

	[ExecuteInEditMode]
	[AddComponentMenu("NGUI/UI/NUICanvas")]
	public class NUICanvas : MonoBehaviour
	{
		static public List<NUICanvas> list = new List<NUICanvas>();

		[SerializeField]
		public EventSystem EventSystem;
		[SerializeField]
		public Camera Camera;

		public int screenHeightPixel
		{
			get
			{
				Vector2 screen = NGUITools.screenSize;
				int height = Mathf.RoundToInt(screen.y);
				return height;
			}
		}

		protected virtual void Awake() {

			Debug.Assert(EventSystem != null);
			Debug.Assert(Camera != null);
            Camera.orthographicSize = 1f;
        }
		protected virtual void OnEnable() { list.Add(this); }
		protected virtual void OnDisable() { list.Remove(this); }

		protected virtual void Start()
		{


			SetSize();

			UpdateScale();
		}

		void Update()
		{
#if UNITY_EDITOR
			if (!Application.isPlaying && gameObject.layer != 0)
				UnityEditor.EditorPrefs.SetInt("NGUI Layer", gameObject.layer);
#endif
			UpdateScale();
		}

		private void SetSize()
		{
			var rect = GetComponent<UIRect>();
			rect.Size = NGUITools.screenSize;

		}

		//通过调整scale来实现子元素1 local unit 就是一个像素
		public void UpdateScale()
		{
			if(Camera == null) return;
			//这里需要设置Camera OrthgraphicSize = 1;

			//CameraSize = 2*OrthgraphicSize
			//localSize * localScale == worldSzie(Unit)
			// worldSize(Unit) / CameraSize(Unit) * CameraPixelCount == Pixel
			// 需要满足 1 localSize == 1 Pixel

			var screenSizePixel = NGUITools.screenSize;
			float scale = 2 * Camera.orthographicSize / screenSizePixel.y;

			Vector3 localScale = transform.localScale;

			if (Mathf.Abs(localScale.x - scale) > float.Epsilon)
			{
				transform.localScale = new Vector3(scale, scale, 1);

				//TODO 剔除BroadcastMessage
				BroadcastMessage("UpdateAnchors", SendMessageOptions.DontRequireReceiver);
			}
		}

	}
}
