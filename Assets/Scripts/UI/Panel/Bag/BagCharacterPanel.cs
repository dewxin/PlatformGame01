using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagCharacterPanel : MonoBehaviour
{
    private Dictionary<EquipSlotEnum, CharacterEquipButton> slot2ButtonDict = new Dictionary<EquipSlotEnum, CharacterEquipButton>();

    void Start()
    {
        FindEquipButton();

        PlayerSingleton.Instance.Character.OnWearEquip += OnWearEquip;
        PlayerSingleton.Instance.Character.OnUnWearEquip += OnUnWearEquip;
    }

    private void FindEquipButton()
    {
        var buttonList = GetComponentsInChildren<CharacterEquipButton>();
        foreach (CharacterEquipButton equipButton in buttonList)
        {
            slot2ButtonDict.Add(equipButton.equipSlot, equipButton);
        }
    }

    private void OnUnWearEquip(Equip equip)
    {
        if (slot2ButtonDict.TryGetValue(equip.Config.EquipSlot, out var button))
        {
            button.UpdateEquip(null);
            return;
        }


        throw new ArgumentException();
    }

    private void OnWearEquip(Equip equip)
    {
        if(slot2ButtonDict.TryGetValue(equip.Config.EquipSlot, out var button))
        {
            button.UpdateEquip(equip);
            return;
        }


        throw new ArgumentException();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
