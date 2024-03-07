using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WildBoar.GUIModule;

public class BagEquipButton : MonoBehaviour
{

    public BagItem BagItem;

    private Equip Equip => BagItem as Equip;


    [SerializeField]
    private UISprite icon;
    //[SerializeField]
    //private UILabel label;

    [SerializeField]
    private EquipInfoPanel equipInfoPanel;


    private void Start()
    {
        InitIcon();
        InitLabel();

    }

    private void InitIcon()
    {
        if (Equip != null)
        {
            icon.SetSprite( Equip.Config.IconIdStr);
        }
    }

    private void InitLabel()
    {
        if (Equip != null)
        {
            //label.text = Equip.Config.Name;

            //label.color = Equip.QualityColor();
        }


    }



    private void OnClick()
    {
        if (BagItem is Equip)
        {
            var equip = (Equip)BagItem;

            equipInfoPanel.ShowPanel(equip, EquipInfoPanel.EquipPosition.Bag);
        }

    }
}
