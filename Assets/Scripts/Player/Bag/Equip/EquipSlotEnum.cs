using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum EquipSlotEnum
{
    Helmet = 0,
    Mask =1, //面具
    Necklace = 2,
    Wings = 3, //翅膀
    Top = 4, //上装
    Bottoms = 5, //下装
    Glove = 6, //护手
    Shoes = 7,

    SingleWeapon = 9, //单持右手武器

    Rings = 10, //戒指
    Amulets = 11, //护身符
    //DuleWeapon = 20, //双持武器
}

public enum EquipQualityEnum
{
    Min = White,
    White = 0,
    Green,
    Blue,
    Purple,
    Gold,
    Red,
    Max = Red,

}

//public enum AvatarBodyEnum
//{
//    Chest,
//    Feet,
//    Hands,
//    Head,
//    Legs,
//    Shoulder,
//    Waist,
//    Wrist,
//    Back,
//    Rings,
//    Amulets,
//    Trinkets,

//}
