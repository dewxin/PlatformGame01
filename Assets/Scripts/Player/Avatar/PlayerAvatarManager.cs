using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerAvatarManager
{
    private PlayerSingleton playerManager;

    public PlayerAvatarManager(PlayerSingleton playerManager)
    {
        this.playerManager = playerManager;
    }

    public void Init()
    {
        playerManager.Character.OnWearEquip += OnWearEquip;
        playerManager.Character.OnUnWearEquip += OnUnWearEquip;

    }

    private void OnWearEquip(Equip equip)
    {
        if (equip == null)
            return;

        if (equip.Config.EquipSlot == EquipSlotEnum.SingleWeapon)
            playerManager.PlayerMain.AvatarHolder.ChangeSlotAvatarId(AvatarSlotTypeEnum.rweapon, equip.Config.AvatarId);

    }

    private void OnUnWearEquip(Equip equip)
    {
        if (equip == null)
            return;

        if (equip.Config.EquipSlot == EquipSlotEnum.SingleWeapon)
            ResetSlotAvatar(AvatarSlotTypeEnum.rweapon);
    }

    public void ResetSlotAvatar(AvatarSlotTypeEnum slotType)
    {
        var avatarId = AvatarConfig.Instance.GetDefaultAvatarId(AvatarGenderEnum.M, slotType);
        playerManager.PlayerMain.AvatarHolder.ChangeSlotAvatarId(slotType, avatarId);
    }
}
