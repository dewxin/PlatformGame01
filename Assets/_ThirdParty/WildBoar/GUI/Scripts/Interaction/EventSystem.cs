using System.Collections.Generic;
using UnityEngine;

namespace WildBoar.GUIModule
{
	[ExecuteAlways]
	public class EventSystem : MonoBehaviour
	{

        public float mouseDragThreshold = 4f;
        public float mouseClickThreshold = 10f;

        public string horizontalAxisName = "Horizontal";
        public string verticalAxisName = "Vertical";
        public string horizontalPanAxisName = null;
        public string verticalPanAxisName = null;
        public string scrollAxisName = "Mouse ScrollWheel";

		public NUICanvas canvas;
        public Camera currentCamera = null;

		public RaycastRectRegistry RaycastRectRegistry { get; private set; } = new RaycastRectRegistry();

        public PointerEventData pointerEventData;

		private EventSystemState state = new EventSystemState();

        public static GameObject currentSelectedGameObject;

		private List<Component> cacheComponentList = new List<Component>();


        void Awake()
        {

            pointerEventData = new PointerEventData(this);
            // Save the starting mouse position
            pointerEventData.PointerCurrentPos = Input.mousePosition;

        }

        void Start()
        {
            canvas = GetComponentInParent<NUICanvas>();
			currentCamera = canvas.Camera;
        }

        void OnEnable()
        {

        }

        void OnDisable()
        {
        }



        void OnValidate() 
		{ 
		}


        void Update()
        {
            // Only the first UI layer should be processing events
            if (!Application.isPlaying) return;

			state.HasRaycastedThisFrame = false; 
            ProcessEvents();

			if(state.HasRaycastedThisFrame)
			{
				state.LastRaycastGameObj = state.RaycastGameObj;
				state.RaycastGameObj = null;
			}

        }


        void LateUpdate()
        {
        }

		public delegate float GetAxisFunc(string name);


		static public GetAxisFunc GetAxis = delegate (string axis)
		{
			return Input.GetAxis(axis);
		};




		public void Raycast(Vector3 inPos)
		{
			var ray = currentCamera.ScreenPointToRay(inPos);

			state.HasRaycastedThisFrame = true;

			var rectList = RaycastRectRegistry.RaycastReturnList(ray);
			if(rectList.Count > 0)
			{
				state.RaycastGameObj = rectList[0].gameObject;
			}

           
			if(state.LastRaycastGameObj != null)
			{
                if (state.LastRaycastGameObj != state.RaycastGameObj)
                    ExecutePointerExit(state.LastRaycastGameObj, pointerEventData);
            }

			if(state.RaycastGameObj != null)
			{
                if (state.LastRaycastGameObj != state.RaycastGameObj)
                    ExecutePointerEnter(state.RaycastGameObj, pointerEventData);

            }

        }


        void ProcessEvents()
		{
			ProcessMouse();
		}

		public void ProcessMouse()
		{
			bool mousePressed = Input.GetMouseButton(0);
			bool mouseDown = Input.GetMouseButtonDown(0);
			bool mouseUp = Input.GetMouseButtonUp(0);

			Vector2 mousePosition = Input.mousePosition;

			pointerEventData.PointerPosDelta = mousePosition - pointerEventData.PointerCurrentPos;
			pointerEventData.PointerCurrentPos = mousePosition;
			pointerEventData.MouseButton = 0;


			if (mousePressed || pointerEventData.PointerPosDelta.magnitude > 0 )
			{
				Raycast(pointerEventData.PointerCurrentPos);
			}


			if (mouseUp) 
				ProcessMouseUp(pointerEventData);
			if (mouseDown)
				ProcessMouseDown(pointerEventData);

			if(mousePressed && pointerEventData.PointerPosDelta.sqrMagnitude > 0)
				ProcessMouseDrag(pointerEventData);
			//ProcessMouseMove(pointerEventData);


			float scroll = !string.IsNullOrEmpty(scrollAxisName) ? GetAxis(scrollAxisName) : 0f;
			//scroll = Input.mouseScrollDelta.y * 0.1f;

			//if (scroll != 0f)
			//{
			//	Notify(mHover, "OnScroll", scroll);
			//}

			pointerEventData.LastPointerDownGO = pointerEventData.PointerDownGO;
		}


		void ProcessMouseDown(PointerEventData pointerEventData)
		{
			if (state.RaycastGameObj == null)
			{
				pointerEventData.PointerDownGO = null;
                return;
			}

			pointerEventData.PointerDownGO = state.RaycastGameObj;
            state.PressedGameObj = state.RaycastGameObj;

            ExecutePointerDown(pointerEventData.PointerDownGO, pointerEventData);

		}


		void ProcessMouseUp(PointerEventData pointerEventData)
		{
			float drag = mouseDragThreshold;
			float click = mouseClickThreshold;

			// Send out the unpress message

			if (pointerEventData.LastPointerDownGO != null)
			{

                ExecutePointerUp(pointerEventData.LastPointerDownGO, pointerEventData);
                ExecutePointerClick(pointerEventData.LastPointerDownGO, pointerEventData);

			}

			state.PressedGameObj = null;
            if(state.DraggingGameObj != null)
            {
                ExecutePointerEndDrag(state.DraggingGameObj, pointerEventData);
			    state.DraggingGameObj = null;
            }
		}


		void ProcessMouseDrag(PointerEventData pointerEventData)
		{

			if(state.DraggingGameObj== null && state.PressedGameObj!=null)
			{
				state.DraggingGameObj = state.PressedGameObj;
                ExecutePointerBeginDrag(state.DraggingGameObj, pointerEventData);
			}

            if(state.DraggingGameObj != null)
            {
                ExecutePointerOnDrag(state.DraggingGameObj, pointerEventData);
            }

        }

        #region Execute Events

        private void ExecutePointerUp(GameObject gameObject, PointerEventData pointerEventData)
		{
			var componentList = GetComponentsImplement<IPointerUpHandler>(gameObject);
			foreach(var component in componentList)
			{
				var handler = component as IPointerUpHandler;
                handler.OnPointerUp(pointerEventData);
			}
		}

        private void ExecutePointerDown(GameObject gameObject, PointerEventData pointerEventData)
        {
            var componentList = GetComponentsImplement<IPointerDownHandler>(gameObject);
            foreach (var component in componentList)
            {
                var handler = component as IPointerDownHandler;
                handler.OnPointerDown(pointerEventData);
            }
        }

        private void ExecutePointerEnter(GameObject gameObject, PointerEventData pointerEventData)
        {
            var componentList = GetComponentsImplement<IPointerEnterHandler>(gameObject);
            foreach (var component in componentList)
            {
                var handler = component as IPointerEnterHandler;
                handler.OnPointerEnter(pointerEventData);
            }
        }

        private void ExecutePointerExit(GameObject gameObject, PointerEventData pointerEventData)
        {
            var componentList = GetComponentsImplement<IPointerExitHandler>(gameObject);
            foreach (var component in componentList)
            {
                var handler = component as IPointerExitHandler;
                handler.OnPointerExit(pointerEventData);
            }
        }


        private void ExecutePointerClick(GameObject gameObject, PointerEventData pointerEventData)
        {
            var componentList = GetComponentsImplement<IPointerClickHandler>(gameObject);
            foreach (var component in componentList)
            {
                var handler = component as IPointerClickHandler;
                handler.OnPointerClick(pointerEventData);
            }
        }

        private void ExecutePointerBeginDrag(GameObject gameObject, PointerEventData pointerEventData)
        {
            var componentList = GetComponentsImplement<IBeginDragHandler>(gameObject);
            foreach (var component in componentList)
            {
                var handler = component as IBeginDragHandler;
                handler.OnBeginDrag(pointerEventData);
            }
        }

        private void ExecutePointerOnDrag(GameObject gameObject, PointerEventData pointerEventData)
        {
            var componentList = GetComponentsImplement<IOnDragHandler>(gameObject);
            foreach (var component in componentList)
            {
                var handler = component as IOnDragHandler;
                handler.OnDrag(pointerEventData);
            }
        }

        private void ExecutePointerEndDrag(GameObject gameObject, PointerEventData pointerEventData)
        {
            var componentList = GetComponentsImplement<IEndDragHandler>(gameObject);
            foreach (var component in componentList)
            {
                var handler = component as IEndDragHandler;
                handler.OnEndDrag(pointerEventData);
            }
        }

        #endregion

        private List<Component> GetComponentsImplement<T>(GameObject gameObject) where T : IEventSystemHandler
		{
			var componentList = GetCacheComponentList();

			gameObject.GetComponents(typeof(T),componentList);

			return componentList;

		}


		private List<Component> GetCacheComponentList()
		{
			cacheComponentList.Clear();
			return cacheComponentList;
		}

	}
}

