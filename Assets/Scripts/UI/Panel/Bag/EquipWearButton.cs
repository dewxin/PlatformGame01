using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipWearButton : MonoBehaviour
{
    // Start is called before the first frame update
    private EquipInfoPanel equipInfoPanel;
    void Start()
    {
        equipInfoPanel = GetComponentInParent<EquipInfoPanel>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnClick()
    {
        equipInfoPanel.HidePanel();
        PlayerSingleton.Instance.Character.WearEquip(equipInfoPanel.Equip);
    }
}