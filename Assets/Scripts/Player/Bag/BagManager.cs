using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BagManager
{
    private PlayerSingleton playerManager;
    public Action OnBagContentChange = delegate { };

    private System.Collections.Generic.List<Equip> equipList = new System.Collections.Generic.List<Equip>();

    public BagManager(PlayerSingleton playerManager) {
        this.playerManager = playerManager;
    }

    public void Init()
    {
        AddEquip(Equip.Generate(1));
        AddEquip(Equip.Generate(6));

    }


    public void AddEquip(Equip equip)
    {
        if (equip == null)
            return;

        equipList.Add(equip);
        OnBagContentChange();
    }

    public void RemoveEquip(Equip equip)
    {
        if (equip == null)
            return;
        equipList.Remove(equip);
        OnBagContentChange();
    }


    public System.Collections.Generic.List<Equip> GetEquipList()
    {
        var list = new System.Collections.Generic.List<Equip>();
        list.AddRange(equipList);
        return list;
    }



}
