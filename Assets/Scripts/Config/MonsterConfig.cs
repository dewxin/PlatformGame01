using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MonsterConfig
{
    public int TemplateId;
    public string Name;
    public int Level;

    //targetHudÕº±Íid “‘º∞ prefabId
    public uint ViewId;

    public int MaxHP;
    public int MaxMP;

    public string AttackSoundID;
    public string HitSoundID;
    public string DeathSoundID;

    public static MonsterConfig Get(int id)
    {
        return MonsterConfigManager.Get(id);
    }

    public static MonsterConfig Get(MonsterNameEnum id)
    {
        return Get((int)id);
    }
}

public enum MonsterNameEnum
{
    ÷Ò“∂«‡=1,
    –°‚®∫Ô =2,
    ◊œıı = 3,
    –°∫Ï÷Ì = 4,
}

public class MonsterConfigManager
{
    private static Dictionary<int, MonsterConfig> dict = new Dictionary<int, MonsterConfig>();


    static MonsterConfigManager()
    {
        AddConfig(new MonsterConfig() {TemplateId = 1, Name="÷Ò“∂«‡", ViewId = 3701,Level = 1, MaxHP = 88,MaxMP=0, AttackSoundID="3701",HitSoundID="7001", DeathSoundID="7001" });
        AddConfig(new MonsterConfig() {TemplateId = 2, Name="–°‚®∫Ô", ViewId = 3703,Level = 2, MaxHP = 145,MaxMP=0, AttackSoundID="3703",HitSoundID="7002", DeathSoundID="7002" });
        AddConfig(new MonsterConfig() {TemplateId = 3, Name="◊œıı", ViewId = 3707,Level = 3, MaxHP = 178,MaxMP=0, AttackSoundID = "3707",HitSoundID="7003",DeathSoundID="7003"});
        AddConfig(new MonsterConfig() {TemplateId = 4, Name="–°∫Ï÷Ì", ViewId = 3714,Level = 4, MaxHP = 231,MaxMP=0 ,AttackSoundID="3714",HitSoundID="7004",DeathSoundID="7004"});
    }

    private static void AddConfig(MonsterConfig equipConfig)
    {
        dict.Add(equipConfig.TemplateId, equipConfig);
    }

    public static MonsterConfig Get(int id)
    {
        return dict[id];
    }

}


