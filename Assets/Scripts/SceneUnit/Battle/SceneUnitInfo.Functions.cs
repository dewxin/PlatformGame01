using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class SceneUnitInfo
{
    public Action<int> OnHPChange = delegate{};
    public Action<int> OnHPReduction = delegate{};
    public Action OnDeath = delegate {};

    public void DecreaseHP(int point)
    {
        HP -= point;
        OnHPChange(HP);
        OnHPReduction(HP);
        if (HP <= 0)
        {
            OnDeath();
        }
    }



}

