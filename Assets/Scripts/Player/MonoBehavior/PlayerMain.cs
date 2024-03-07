using Assets.Scripts.ScriptableObj;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

[RequireComponent(typeof(GravityRigidBody))]
[RequireComponent(typeof(PlatformRigidBody))]
[RequireComponent(typeof(PlayerInputMove))]
[RequireComponent(typeof(PlayerTrack))]
public class PlayerMain : MonoBehaviour, IHasSceneUnitInfo
{
    public GravityRigidBody GravityRigidBody { get; private set; }
    public PlatformRigidBody PlatformRigidBody { get; private set; }
    public PlayerInputMove PlayerMoveController { get; private set; }
    public PlayerTrack PlayerTrack { get; private set; }
    public SceneUnitSkill SceneUnitSkill { get; private set; }

    public SceneUnitAvatarHolder AvatarHolder { get; private set; }
    public SceneUnitEffectManager SceneEffectManager { get; private set; }

    public CameraFollowPlayer CameraMove { get; set; }

    public SceneUnitInfo SceneUnitInfo { get; private set; }
    private void Awake()
    {
        GravityRigidBody= GetComponent<GravityRigidBody>();
        PlatformRigidBody = GetComponent<PlatformRigidBody>();
        PlatformRigidBody.OnEnterPlatform += UpdatePlatform;
        PlayerMoveController = GetComponent<PlayerInputMove>();
        PlayerTrack= GetComponent<PlayerTrack>();

        AvatarHolder = gameObject.AddComponent<SceneUnitAvatarHolder>();
        AvatarSlotTypeEnum[] slotList= (AvatarSlotTypeEnum[])Enum.GetValues(typeof(AvatarSlotTypeEnum));
        AvatarHolder.GenerateSlotAvatar(slotList.ToList());

        SceneEffectManager = gameObject.AddComponent<SceneUnitEffectManager>();
        SceneUnitSkill = gameObject.AddComponent<SceneUnitSkill>();

        InitSceneUnitInfo();
    }

    private void OnEnable()
    {
        PlayerSingleton.Instance.PlayerMain = this;
        MouseManager.OnClickSceneUnitMaybeNull += OnMouseClickSceneUnit;
    }

    private void OnDisable()
    {
        PlayerSingleton.Instance.PlayerMain = null;
        MouseManager.OnClickSceneUnitMaybeNull -= OnMouseClickSceneUnit; 
    }


    private void Start()
    {
        SetInitAvatar();
    }

    private void Update()
    {
        PlayerSingleton.Instance.Update();

    }

    private void OnMouseClickSceneUnit(IHasSceneUnitInfo sceneUnitInfoOwner)
    {
        if (sceneUnitInfoOwner == null)
        {
            this.SceneUnitInfo.Target = null;
            return;
        }

        var unit = sceneUnitInfoOwner.GetSceneUnit();
        this.SceneUnitInfo.Target = unit;
    }



    private void InitSceneUnitInfo()
    {
        SceneUnitInfo = new SceneUnitInfo
        {
            HeadIcon = "HeadJsMale",
            MaxHP = 10000,
            HP = 10000,
            MaxMP= 6000,
            MP= 6000,
            Name ="Íæ¼Ò1",
            SelfGameObj = this.gameObject,
            ///TODO Ìí¼Ó <see cref="StateManagerBase"/> ?
        };
    }

    public void UpdatePlatform(Platform platform)
    {
        SceneUnitInfo.Platform = platform;

    }

    public void SetInitAvatar()
    {

        foreach (var slotTypeAndAvatarId in AvatarConfig.Instance.GetDefaultAvatarSlotList(AvatarGenderEnum.M))
        {
            AvatarHolder.ChangeSlotAvatarId(slotTypeAndAvatarId.Item1, slotTypeAndAvatarId.Item2);
        }

    }


    public void SetPosWhenChangeScene(Vector3 pos)
    {
        this.transform.position = pos;
        PlayerTrack.ResetTrackPos();
        if (CameraMove != null)
            CameraMove.SetCameraPosAsPlayer();
    }

    public SceneUnitInfo GetSceneUnit()
    {
        SceneUnitInfo.SceneUnitInfoOwner= this;
        return SceneUnitInfo;
    }

    public SpriteRenderer GetSpriteRenderer()
    {
        return null;
    }

    public Transform GetViewRoot()
    {
        return AvatarHolder.transform;
    }
}
