using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanel : PanelBase
{
    public SceneUnitHUD EnemyHUD;
    public void Start()
    {
        //TODO �ǲ��ǿ���ֱ�ӷ���һ������������õ����������
        //��Ϊ���ú���Щ��������Ǳ�����������һ�������
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
        //����ǲ�����Ҫ��ʾHUD��GameObj
        //TODO ����������õ�����spriteǰ�棬ͻ����ʾ
        if (sceneUnitInfoOwner == null)
        {
            EnemyHUD.Hide();
            return;
        }

        EnemyHUD.Show();
        EnemyHUD.SetInfo(sceneUnitInfoOwner.GetSceneUnit());
    }


}
