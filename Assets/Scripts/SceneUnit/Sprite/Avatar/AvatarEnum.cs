using Assets.Scripts.ScriptableObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum AvatarSlotTypeEnum
{
    back,
    body,
    dbody, //?
    face,
    foot,
    hair,
    jawel, //?
    lhair, //?
    lhand,
    lweapon,
    rhand,
    rweapon,
}

public enum AvatarGenderEnum
{
    F,  //femal
    M,  //male
    N,  // nomral?
    O, //monster?
}


[Serializable]
public class AvatarAnimFrame
{
    public AvatarAnimEnum AnimEnum;
    public int FrameID;
}

[Serializable]
public class AvatarAnimEvent
{
    public int FrameID;
    public AnimEventEnum Event;
}

[Serializable]
public class AnimEventEffect
{
    public AnimEventEnum Event;
    public BaseEffect Effect;
}

public enum AnimEventEnum
{
    None = 0,
    PrepareToAttack,
    ReadyToAttack,
    Attacking,
    AttackFinished,

    SectionMark1 = 10000,
    PrepareToAttack1,
    ReadyToAttack1,
    Attacking1,
    AttackFinished1,

    SectionMark2 = 20000,
    PrepareToAttack2,
    ReadyToAttack2,
    Attacking2,
    AttackFinished2,
}

public enum AvatarAnimEnum:byte
{
    Idle00 = 0,
    BattleIdle01 = 1,
    Walk02 = 2,
    Run03 = 3,
    Fall04 = 4,
    Jump05 = 5,
    Climb06 = 6,
    Hit07 = 7,
    Die08 = 8,
    Dead09 = 9,
    Other10 = 10, // 0x0000000A
    Gesture15 = 15, // 0x0000000F
    Other20 = 20, // 0x00000014
    Other21 = 21, // 0x00000015
    Attack22 = 22, // 0x00000016
    Attack23 = 23, // 0x00000017
    Attack24 = 24, // 0x00000018
    Attack25 = 25, // 0x00000019
    Attack26 = 26, // 0x0000001A
    Other27 = 27, // 0x0000001B
    Other28 = 28, // 0x0000001C
    Other29 = 29, // 0x0000001D
    Other30 = 30, // 0x0000001E
    Attack31 = 31, // 0x0000001F
    Attack32 = 32, // 0x00000020
    Attack33 = 33, // 0x00000021
    Attack34 = 34, // 0x00000022
    Attack35 = 35, // 0x00000023
    Other36 = 36, // 0x00000024
    Other37 = 37, // 0x00000025
    Other38 = 38, // 0x00000025
    Other54 = 54, // 0x00000036
    ChantStart56 = 56, // 0x00000038
    Chant57 = 57, // 0x00000039
    ChantEnd58 = 58, // 0x0000003A
    ChantEnd59 = 59, // 0x0000003B
    Other63 = 63, // 0x0000003F
    Other64 = 64, // 0x00000040
}

