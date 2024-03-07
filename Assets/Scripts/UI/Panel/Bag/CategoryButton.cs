using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WildBoar.GUIModule;

public class CategoryButton : MonoBehaviour
{
    public BagItemType visibleType;
    private const string suffix_SELECTED = "Selected";
    private UISprite sprite;

    private BagPanel bagPanel;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<UISprite>();

        bagPanel = transform.GetComponentInParent<BagPanel>();
        bagPanel.OnVisibleTypeChange += OnVisibleTypeChange;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnVisibleTypeChange(BagItemType type)
    {
        if(type == this.visibleType)
        {
            if (!sprite.spriteName.EndsWith(suffix_SELECTED))
                sprite.SetSprite( sprite.spriteName + suffix_SELECTED);
        }

        else
        {
            if (sprite.spriteName.EndsWith(suffix_SELECTED))
                sprite.SetSprite( sprite.spriteName.Replace(suffix_SELECTED,""));
        }

    }

    private void OnClick()
    {
        bagPanel.VisibleType = visibleType;


    }
}
