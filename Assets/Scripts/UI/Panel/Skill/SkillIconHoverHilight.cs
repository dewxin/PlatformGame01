using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WildBoar.GUIModule;

[RequireComponent(typeof(UISprite))]
public class SkillIconHoverHilight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [ColorUsageAttribute(true,true)]
    public Color HighlightColor = Color.white *1.2f;

    private Color prevColor;

    private UISprite sprite;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<UISprite>();
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        prevColor = sprite.Color;
        sprite.SetColor(HighlightColor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        sprite.SetColor(prevColor);
    }
}
