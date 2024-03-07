using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CharacterManager
{
    private PlayerSingleton playerManager;
    private Equip weapon;
    //old, new
    public Action<Equip> OnWearEquip = delegate { };
    public Action<Equip> OnUnWearEquip = delegate { };

    public CharacterManager(PlayerSingleton playerManager)
    {
        this.playerManager = playerManager;
    }

    public void Init()
    {

    }

    public void WearEquip(Equip equip)
    {
        if(equip.Config.EquipSlot == EquipSlotEnum.SingleWeapon)
        {
            if(weapon != null)
            {
                OnUnWearEquip(weapon);
                playerManager.Bag.AddEquip(weapon);
            }

            playerManager.Bag.RemoveEquip(equip);
            weapon= equip;

            OnWearEquip(equip);
        }
    }

    public void UnWearEquip(EquipSlotEnum slotEnum)
    {
        if (slotEnum == EquipSlotEnum.SingleWeapon)
        {
            if (weapon != null)
            {
                OnUnWearEquip(weapon);
                playerManager.Bag.AddEquip(weapon);
            }


            weapon = null;
        }
    }

}
