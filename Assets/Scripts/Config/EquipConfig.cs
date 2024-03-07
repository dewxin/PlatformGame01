using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipConfig
{

    public int TemplateId;
    public string Name;
    public string Description;
    public int IconId;
    public string IconIdStr => IconId.ToString("D4");
    public uint AvatarId;

    //基础数值
    public int Attack;
    public int MagicAttack;
    public int Defense;
    public int MagicDefense;

    //随机额外数值
    public int ExtraAttakMinInclusive;
    public int ExtraAttakMaxExclusive;


    public EquipSlotEnum EquipSlot;

    public static EquipConfig Get(int id)
    {
        return EquipConfigManager.Get(id);
    }

}

public class EquipConfigManager
{
    private static Dictionary<int, EquipConfig> dict = new Dictionary<int, EquipConfig>();


    static EquipConfigManager()
    {
        AddConfig(new EquipConfig() { TemplateId = 1, Name = "铁剑", Description="一把不错的剑。", IconId = 1, AvatarId=1, EquipSlot = EquipSlotEnum.SingleWeapon, Attack = 52, Defense = 20, ExtraAttakMinInclusive = 6, ExtraAttakMaxExclusive =12 });
        AddConfig(new EquipConfig() { TemplateId = 6, Name = "裹银剑", Description="银质合金打造，乃是上乘之作。", IconId = 2, AvatarId=2, EquipSlot = EquipSlotEnum.SingleWeapon, Attack = 108, ExtraAttakMinInclusive = 12, ExtraAttakMaxExclusive = 24 });
    }


    private static void AddConfig(EquipConfig equipConfig)
    {
        dict.Add(equipConfig.TemplateId, equipConfig);
    }


    public static EquipConfig Get(int id)
    {
        return dict[id];
    }

}
