using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PanelManager:Singleton<PanelManager>
{

    private Dictionary<Type, PanelBase> type2PanelDict = new Dictionary<Type, PanelBase>();

    public bool TryGetPanel<T>(out T panel) where T: PanelBase
    {

        bool result = type2PanelDict.TryGetValue(typeof(T), out var panelBase);

        panel = panelBase as T;

        return result; 
    }


    public PanelBase ShowPanelUnderParent(PanelBase panelPrefab, GameObject parentGo)
    {
        if (!type2PanelDict.TryGetValue(panelPrefab.GetType(), out var panel))
        {

            if (parentGo == null)
                panel = MonoBehaviour.Instantiate(panelPrefab);
            else
                panel = MonoBehaviour.Instantiate(panelPrefab, parentGo.transform);


            type2PanelDict.Add(panelPrefab.GetType(), panel);

            if (parentGo != null)
            {
                panel.transform.SetParent(parentGo.transform);
                panel.transform.localPosition = Vector3.zero;
                panel.transform.localScale = Vector3.one;
            }
        }

        panel.ShowPanel();
        return panel ;
    }


}
