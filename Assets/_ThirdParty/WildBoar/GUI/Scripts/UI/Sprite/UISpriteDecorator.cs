using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WildBoar.GUIModule
{
    [RequireComponent(typeof(UISprite))]
    public class UISpriteDecorator : UIRectUser, IPointerEnterHandler, IPointerExitHandler
    {

        private UISprite uiSprite;

        [SerializeField]
        public string normalSpriteName;
        [SerializeField]
        public string hoverSpriteName;

        public void Awake()
        {
            uiSprite= GetComponent<UISprite>();
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            uiSprite.SetSprite(hoverSpriteName);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            uiSprite.SetSprite(normalSpriteName);
        }



    }
}
