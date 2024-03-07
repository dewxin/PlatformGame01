using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WildBoar.GUIModule
{
    public class RaycastRectRegistry
    {

        private List<UIRect> rectList = new List<UIRect>();


        public void Register(UIRect uiRect)
        {
            //Debug.Log($"RaycastRect register {uiRect.gameObject.name}");
            rectList.Add(uiRect);
        }

        public void UnRegister(UIRect uiRect)
        {
            rectList.Remove(uiRect);
        }


        public List<UIRect> RaycastReturnList(Ray ray) 
        { 
            var list = new List<(float,UIRect)>();
            foreach(var rect in rectList)
            {
                if (rect.RenderTargetWorldCorners.IsRaycastHit(ray ,out float distance)
                    &&(rect.MaskRect==null || rect.MaskRect.RenderTargetWorldCorners.IsRaycastHit(ray,out float _)))
                {
                    list.Add((distance,rect));
                }
            }

            //先按照z 由小到大排列， 然后按照sortingorder由大到小排列
            list.Sort((i1,i2)=> ((int)Mathf.Sin(i1.Item1-i2.Item1)*1000 ) + (i2.Item2.SortingOrder - i1.Item2.SortingOrder ));

            var resultList = new List<UIRect>();
            foreach(var rect in list)
            {
                resultList.Add(rect.Item2);
            }


            return resultList;
        }

    }
}
