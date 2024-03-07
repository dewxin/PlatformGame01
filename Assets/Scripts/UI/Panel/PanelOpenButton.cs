using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WildBoar.GUIModule;

public class PanelOpenButton:MonoBehaviour, IPointerClickHandler
{

    public PanelBase PanelPrefab;

    public void OnPointerClick(PointerEventData eventData)
    {

        if(PanelManager.Instance.TryGetPanel<MainPanel>(out var mainPanel))
        {
            PanelManager.Instance.ShowPanelUnderParent(PanelPrefab, mainPanel.gameObject);
        }
    
    }

}
