using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanel : PanelBase
{
    public SceneUnitHUD EnemyHUD;
    public void Start()
    {
        //TODO 是不是可以直接放在一个场景里，而不用调用这个函数
        //因为调用后，这些对象好像是被放在了另外一个场景里。
        DontDestroyOnLoad(this.gameObject);
        EnemyHUD.Hide();

    }

    public void OnEnable()
    {
        MouseManager.OnClickSceneUnitMaybeNull += OnClickSceneObj;
    }

    public void OnDisable()
    {
        MouseManager.OnClickSceneUnitMaybeNull -= OnClickSceneObj;
    }

    public void OnClickSceneObj(IHasSceneUnitInfo sceneUnitInfoOwner)
    {
        //检查是不是需要显示HUD的GameObj
        //TODO 将该物体放置到其他sprite前面，突出显示
        if (sceneUnitInfoOwner == null)
        {
            EnemyHUD.Hide();
            return;
        }

        EnemyHUD.Show();
        EnemyHUD.SetInfo(sceneUnitInfoOwner.GetSceneUnit());
    }


}
