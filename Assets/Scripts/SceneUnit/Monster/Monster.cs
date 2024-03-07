using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    [SerializeField]
    public SceneUnitInfo sceneUnitInfo;

    public MonsterConfig config;

    SimpleStateManager stateManager;

    // Start is called before the first frame update
    void Start()
    {
        Init(config);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Init(MonsterConfig monsterConfig)
    {
        stateManager = GetComponent<SimpleStateManager>();

        sceneUnitInfo = new SceneUnitInfo
        {
            HeadIcon = monsterConfig.ViewId.ToString(),
            MaxHP = monsterConfig.MaxHP,
            HP = monsterConfig.MaxHP,
            MaxMP = monsterConfig.MaxMP,
            MP = monsterConfig.MaxMP,
            Level = monsterConfig.Level,
            Name = monsterConfig.Name,

            AttackSoundID= monsterConfig.AttackSoundID,
            HitSoundID= monsterConfig.HitSoundID,
            DeathSoundID= monsterConfig.DeathSoundID,

            SelfGameObj = this.gameObject,
            StateManager = stateManager,
            Platform = stateManager.Platform,
        };

        stateManager.SceneUnitInfo= sceneUnitInfo;
        stateManager.SceneUnitInfo.OnHPReduction += (hpPoint) => stateManager.EnterNewState(StateType.HitRecovery);
        stateManager.SceneUnitInfo.OnDeath += OnSceneUnitDeath;

        stateManager.OnDeathStateOver += () => Destroy(this.gameObject);

    }

    private void OnSceneUnitDeath()
    {
        stateManager.EnterNewState(StateType.Dead);

        //TODO 感觉还是有点耦合了
        if(MouseManager.FocusedSceneUnit == sceneUnitInfo)
            MouseManager.OnClickSceneUnitMaybeNull(null);

        GetComponentInChildren<BoxCollider2D>().enabled = false;
    }


}
