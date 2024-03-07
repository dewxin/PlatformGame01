using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public partial class SceneUnitInfo
{
    public string Name;
    public int Level;
    public string HeadIcon;

    public int MaxHP;
    public int HP; 
    public int MaxMP;
    public int MP; 

    public float HPercent => (float)HP / MaxHP;
    public float MPercent => (float)MP / MaxMP;

    public string AttackSoundID;
    public string HitSoundID;
    public string DeathSoundID;


    //攻击 施法 对象
    public SceneUnitInfo Target { get; set; }
    public GameObject SelfGameObj { get; set; }

    public Platform Platform { get; set; }

    public StateManagerBase StateManager { get; set; }

    public IHasSceneUnitInfo SceneUnitInfoOwner { get; set; }
}

public interface IHasSceneUnitInfo
{
    public SceneUnitInfo GetSceneUnit();

    //todo 感觉获取avatarHolder会更好
    public SpriteRenderer GetSpriteRenderer();
    public Transform GetViewRoot();
}
