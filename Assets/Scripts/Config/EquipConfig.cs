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

    //������ֵ
    public int Attack;
    public int MagicAttack;
    public int Defense;
    public int MagicDefense;

    //���������ֵ
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
        AddConfig(new EquipConfig() { TemplateId = 1, Name = "����", Description="һ�Ѳ���Ľ���", IconId = 1, AvatarId=1, EquipSlot = EquipSlotEnum.SingleWeapon, Attack = 52, Defense = 20, ExtraAttakMinInclusive = 6, ExtraAttakMaxExclusive =12 });
        AddConfig(new EquipConfig() { TemplateId = 6, Name = "������", Description="���ʺϽ���죬�����ϳ�֮����", IconId = 2, AvatarId=2, EquipSlot = EquipSlotEnum.SingleWeapon, Attack = 108, ExtraAttakMinInclusive = 12, ExtraAttakMaxExclusive = 24 });
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
