using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Equip:BagItem
{
    public override BagItemType Type => BagItemType.Equip;

    public int TemplateId { get; set; }
    public EquipQualityEnum Quality { get; private set; }
    public EquipConfig Config => EquipConfig.Get(TemplateId);

    public int ExtraAttack;

    public static Equip Generate(int id)
    {
        var equip = new Equip();
        equip.TemplateId = id;
        equip.Quality = RandomQuality();

        equip.ExtraAttack = Random.Range(equip.Config.ExtraAttakMinInclusive, equip.Config.ExtraAttakMaxExclusive); 

        return equip;
    }


    private static EquipQualityEnum RandomQuality()
    {
        var result = Random.Range((int)EquipQualityEnum.Min, (int)EquipQualityEnum.Max);
        return (EquipQualityEnum)result;
    }

    public Color QualityColor()
    {
        switch(Quality)
        {
            case EquipQualityEnum.White:
                return Color.white;

            case EquipQualityEnum.Green:
                return Color.green;

            case EquipQualityEnum.Blue:
                return new Color(0.40f, 0.60f, 0.90f);

            case EquipQualityEnum.Purple:
                return new Color(1f, 0.2f, 1f);

            default:
                return Color.white;
        }
    }

    public string BasicStatsStr()
    {
        var list = new System.Collections.Generic.List<string>();
        if(Config.Attack > 0)
            list.Add("物攻+" + Config.Attack);
        if(Config.Defense > 0)
            list.Add("防御+" + Config.Defense);

        return string.Join("\r\n", list);

    }

    public string ExtraStatsStr()
    {
        if(ExtraAttack > 0)
        {
            return $"物攻+{ExtraAttack}";
        }

        return "";

    }

}
