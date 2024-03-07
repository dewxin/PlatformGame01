using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WildBoar.GUIModule
{
    internal class EventSystemState
    {
        public bool HasRaycastedThisFrame { get; set; }
        public GameObject LastRaycastGameObj { get; set; }
        public GameObject RaycastGameObj { get; set; }

        public GameObject PressedGameObj { get; set; }
        public GameObject DraggingGameObj { get; set; }
    }
}
