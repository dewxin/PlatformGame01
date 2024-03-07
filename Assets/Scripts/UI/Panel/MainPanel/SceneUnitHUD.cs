using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using WildBoar.GUIModule;

public class SceneUnitHUD : MonoBehaviour
{
    public UISprite HpBar;
    public UISprite Head;
    public TextMeshPro EnemyName;


    private SceneUnitInfo sceneUnitInfo;
    private SceneUnitInfo SceneUnitInfo 
    {
        get => sceneUnitInfo;
        set 
        {
            if (sceneUnitInfo != null)
                sceneUnitInfo.OnHPChange -= OnHPChange;
            sceneUnitInfo= value; 
            sceneUnitInfo.OnHPChange += OnHPChange; 
        } 
    }

    public void SetInfo(SceneUnitInfo sceneUnitInfo)
    {
        if (this.SceneUnitInfo == sceneUnitInfo)
            return;

        this.SceneUnitInfo = sceneUnitInfo;

        EnemyName.text = sceneUnitInfo.Name;
        Head.SetSprite(sceneUnitInfo.HeadIcon);
        HpBar.SetFillAmount(sceneUnitInfo.HPercent);


    }

    public void OnHPChange(int hp)
    {
        HpBar.SetFillAmount(SceneUnitInfo.HPercent);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
