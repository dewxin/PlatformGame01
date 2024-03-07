using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum BagItemType
{
    Common, //道具
    Equip, //装备
}


public abstract class BagItem
{
    public abstract BagItemType Type { get; }

}

