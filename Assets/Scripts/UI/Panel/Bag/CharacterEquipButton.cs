using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using WildBoar.GUIModule;

public class CharacterEquipButton : MonoBehaviour
{
    public EquipSlotEnum equipSlot;

    private Equip equip;


    private UISprite icon;

    [SerializeField]
    private EquipInfoPanel equipInfoPanel;
    // Start is called before the first frame update
    void Start()
    {
        icon = GetComponent<UISprite>();
        UpdateIcon();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateEquip(Equip equip)
    {
        this.equip = equip;
        UpdateIcon();
    }

    private void UpdateIcon()
    {
        if (icon == null)
            return;

        if (equip == null)
        {
            icon.enabled = false;

            return;
        }

        icon.enabled = true;
        icon.SetSprite( equip.Config.IconIdStr);
    }

    private void OnClick()
    {
        if (equip != null)
        {
            equipInfoPanel.ShowPanel(equip, EquipInfoPanel.EquipPosition.Character);
        }

    }
}
