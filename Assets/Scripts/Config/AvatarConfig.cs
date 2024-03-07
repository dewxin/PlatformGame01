using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AvatarConfig
{
    private static AvatarConfig instance = new AvatarConfig();
    public static AvatarConfig Instance => instance;

    public AvatarConfig()
    {
        Init();
    }

    private void Init()
    {
    }

    //private void InitSortingOrderDict()
    //{
    //    foreach (AvatarSlotTypeEnum slotTypeEnum in Enum.GetValues(typeof(AvatarSlotTypeEnum)))
    //        slotSortingOrderDict.Add(slotTypeEnum, new Dictionary<AvatarAnimEnum, System.Collections.Generic.List<int>>());

    //    var rightWeaponDict = this.slotSortingOrderDict[AvatarSlotTypeEnum.rweapon];
    //    var rightHandDict = this.slotSortingOrderDict[AvatarSlotTypeEnum.rhand];
    //    var faceDict = this.slotSortingOrderDict[AvatarSlotTypeEnum.face];
    //    // 1, 2, 3, 4, 5, 6, 7, 8, 9,10,11,12,13,14
    //    rightWeaponDict.Add(AvatarAnimEnum.Attack22, new System.Collections.Generic.List<int> { 10, 10, 10, 10, 10, 10, 10 });
    //    rightWeaponDict.Add(AvatarAnimEnum.Attack23, new System.Collections.Generic.List<int> { 10, 10, 10, 10, 10, 10, 10 });
    //    faceDict.Add(AvatarAnimEnum.Attack23, new System.Collections.Generic.List<int> { 6, 6, 6, 1, 1, 1, 6 });
    //    rightWeaponDict.Add(AvatarAnimEnum.Attack24, new System.Collections.Generic.List<int> { 10, 4, 4, 4, 10, 3, 10, 4, 10, 10, 10, 3, 0, 0 });
    //    rightWeaponDict.Add(AvatarAnimEnum.Attack25, new System.Collections.Generic.List<int> { 10, 10, 10, 10, 10, 10, 0, 0, 0, 10, 10, 10, 10, 10 });
    //    rightWeaponDict.Add(AvatarAnimEnum.Attack26, new System.Collections.Generic.List<int> { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 });
    //    rightWeaponDict.Add(AvatarAnimEnum.Attack34, new System.Collections.Generic.List<int> { 10, 10, 10, 10, 10, 10, 10, 1, 1, 1, 10, 10, 10 });
    //    rightWeaponDict.Add(AvatarAnimEnum.Climb06, new System.Collections.Generic.List<int> { 10, 10 });

    //    rightHandDict.Add(AvatarAnimEnum.Attack25, new System.Collections.Generic.List<int>() { 6, 6, 6, 6, 6, 6, 0, 0, 0, 6, 6, 6, 6, 6 });


    //    slotSortingOrderDict[AvatarSlotTypeEnum.back].Add(AvatarAnimEnum.Climb06, new System.Collections.Generic.List<int> { 9, 9 });
    //    slotSortingOrderDict[AvatarSlotTypeEnum.lweapon].Add(AvatarAnimEnum.Climb06, new System.Collections.Generic.List<int> { 10, 10 });
    //    slotSortingOrderDict[AvatarSlotTypeEnum.jawel].Add(AvatarAnimEnum.Climb06, new System.Collections.Generic.List<int> { 8, 8 });
    //}

    public int GetInitSortOrder(AvatarSlotTypeEnum slotTypeEnum)
    {
        switch (slotTypeEnum)
        {
            case AvatarSlotTypeEnum.back: return 0;
            case AvatarSlotTypeEnum.jawel: return 10;
            case AvatarSlotTypeEnum.hair: return 9;
            case AvatarSlotTypeEnum.lhair: return 8;
            case AvatarSlotTypeEnum.face: return 7;
            case AvatarSlotTypeEnum.body: return 5;
            case AvatarSlotTypeEnum.dbody: return 4;
            case AvatarSlotTypeEnum.foot: return 1;
            case AvatarSlotTypeEnum.rhand: return 8;
            case AvatarSlotTypeEnum.lhand: return 1;
            case AvatarSlotTypeEnum.rweapon: return 7;
            case AvatarSlotTypeEnum.lweapon: return 6;
            default: return 0;
        }
    }

    //public float GetAnimFpsScale(AvatarAnimEnum animEnum)
    //{
    //    switch(animEnum)
    //    {
    //        case AvatarAnimEnum.Idle00:
    //        case AvatarAnimEnum.BattleIdle01:
    //            return 1.5f;

    //        case AvatarAnimEnum.Walk02:
    //        case AvatarAnimEnum.Run03:
    //        case AvatarAnimEnum.Climb06:
    //        case AvatarAnimEnum.Attack22:
    //        case AvatarAnimEnum.Attack23:
    //        case AvatarAnimEnum.Attack31:
    //        case AvatarAnimEnum.Attack32:
    //        case AvatarAnimEnum.ChantStart56:
    //        case AvatarAnimEnum.ChantEnd58:
    //        case AvatarAnimEnum.ChantEnd59:
    //            return 2f;

    //        case AvatarAnimEnum.Attack24:
    //        case AvatarAnimEnum.Attack25:
    //        case AvatarAnimEnum.Attack33:
    //        case AvatarAnimEnum.Attack34:
    //            return 1.2f;

    //        case AvatarAnimEnum.Attack26:
    //        case AvatarAnimEnum.Attack35:
    //            return 1.4f;

    //        default:
    //            return 1f;
    //    }
    //}

    //public int GetAnimMaxFrameIndex(AvatarAnimEnum animEnum)
    //{
    //    switch(animEnum)
    //    {
    //        case AvatarAnimEnum.Idle00:
    //        case AvatarAnimEnum.BattleIdle01:
    //        case AvatarAnimEnum.Die08:
    //            return 4;

    //        case AvatarAnimEnum.Walk02:
    //        case AvatarAnimEnum.Run03:
    //            return 8;

    //        case AvatarAnimEnum.Climb06:
    //            return 2;

    //        case AvatarAnimEnum.Attack22:
    //        case AvatarAnimEnum.Attack23:
    //        case AvatarAnimEnum.Attack31:
    //        case AvatarAnimEnum.Attack32:
    //            return 7;

    //        case AvatarAnimEnum.Attack24:
    //        case AvatarAnimEnum.Attack25:
    //            return 14;

    //        case AvatarAnimEnum.Attack26:
    //        case AvatarAnimEnum.Attack35:
    //            return 11;

    //        case AvatarAnimEnum.Attack33:
    //        case AvatarAnimEnum.Attack34:
    //            return 13;

    //        case AvatarAnimEnum.ChantStart56:
    //        case AvatarAnimEnum.ChantEnd58:
    //        case AvatarAnimEnum.ChantEnd59:
    //            return 6;


    //        default:
    //            return 1;
    //    }

    //}


    public List<(AvatarSlotTypeEnum, uint)> GetDefaultAvatarSlotList(AvatarGenderEnum gender)
    {
        if (gender == AvatarGenderEnum.M)
        {
            return new List<(AvatarSlotTypeEnum, uint)>()
            {
                (AvatarSlotTypeEnum.body, 1010),
                (AvatarSlotTypeEnum.dbody, 1010),
                (AvatarSlotTypeEnum.face, 8),
                (AvatarSlotTypeEnum.foot, 1012),
                (AvatarSlotTypeEnum.lhand, 1011),
                (AvatarSlotTypeEnum.rhand, 1011),
                (AvatarSlotTypeEnum.hair, 774),
                (AvatarSlotTypeEnum.rweapon, 3414),
            };
        }

        return null;

    }

    public uint GetDefaultAvatarId(AvatarGenderEnum gender, AvatarSlotTypeEnum slot)
    {
        var list = GetDefaultAvatarSlotList(gender);
        if (list == null)
            return 0;

        foreach ( var pair in list)
        {
            if(pair.Item1 == slot)
                return pair.Item2;
        }
        return 0;

    }

}
