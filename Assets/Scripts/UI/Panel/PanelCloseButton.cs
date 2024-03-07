using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WildBoar.GUIModule;

public class PanelCloseButton:MonoBehaviour,IPointerClickHandler
{
    PanelBase panel;

    private void OnEnable()
    {
        panel = GetComponentInParent<PanelBase>();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        panel.HidePanel();
    }
}
