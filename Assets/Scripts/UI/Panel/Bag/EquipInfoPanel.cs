using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WildBoar.GUIModule;

public class EquipInfoPanel : PanelBase
{
    public enum EquipPosition { Bag, Character}

    public Equip Equip { get; private set; }

    [SerializeField]
    private UISprite icon;
    //[SerializeField]
    //private UILabel equipName;
    //[SerializeField]
    //private UILabel equipDescription;
    //[SerializeField]
    //private UILabel equipBasicStats;
    //[SerializeField]
    //private UILabel equipExtraStats;

    [SerializeField]
    private GameObject BagButtonGroup;
    [SerializeField]
    private GameObject CharacterButtonGroup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnClick()
    {
        HidePanel();

        Debug.Log("hide panel on click");

    }

    public void ShowPanel(Equip equip, EquipPosition equipPosition)
    {
        this.gameObject.SetActive(true);
        this.Equip = equip;

        icon.SetSprite( equip.Config.IconIdStr);

        //equipName.text = equip.Config.Name;
        //equipName.gradientTop = equipName.gradientBottom = equip.QualityColor();
        
        //equipDescription.text = equip.Config.Description;
        //equipBasicStats.text = equip.BasicStatsStr();
        //equipExtraStats.text = equip.ExtraStatsStr();

        if(equipPosition == EquipPosition.Bag)
        {
            BagButtonGroup.SetActive(true); 
            CharacterButtonGroup.SetActive(false);
        }
        else if(equipPosition == EquipPosition.Character)
        {
            CharacterButtonGroup.SetActive(true);
            BagButtonGroup.SetActive(false); 
        }

    }
}
