using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BagPanel : PanelBase
{

    private BagItemType _visibleType;
    public BagItemType VisibleType 
    { 
        get => _visibleType; 
        set { _visibleType = value; OnVisibleTypeChange(value); } 
    }

    public Action<BagItemType> OnVisibleTypeChange = delegate { };
    
    // Start is called before the first frame update
    void Start()
    {
        OnVisibleTypeChange += RefreshWhenVisibleTypeChange;

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void RefreshWhenVisibleTypeChange(BagItemType bagItemType)
    {
    }

    public void Refresh()
    {

    }

}
