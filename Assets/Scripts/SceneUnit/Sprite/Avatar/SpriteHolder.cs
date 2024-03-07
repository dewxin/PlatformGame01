using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteHolder : MonoBehaviour
{
    public AvatarSlotTypeEnum slotType;
    public AvatarGenderEnum gender = AvatarGenderEnum.M;

    public uint avatarId { get; private set; }//具体分类下的id


    public SpriteRenderer SpriteRender { get; set; }

    [SerializeField]
    private int defaultSortOrder = 0;


    public SceneUnitAvatarHolder avatarHolder;
    public AvatarAnimFrame CurrentAnimFrame
    {
        get => avatarHolder.currentAnimFrame;
    }


    public bool IsAvatarInvalid => avatarId== 0;


    void Start()
    {
        SpriteRender = GetComponent<SpriteRenderer>();
        defaultSortOrder = SpriteRender.sortingOrder;

    }

    // Update is called once per frame
    void Update()
    {
        Animate();
    }

    private void Animate()
    {
        if (avatarHolder.Paused)
            return;

        SpriteRender.sprite = CurrentSprite();
        SpriteRender.sortingOrder = CurrentOrder();

    }

    private Sprite CurrentSprite()
    {
        if(avatarId==0) return null;
        //ResetFrameIdIfInvalid();
        return SpriteCollector.LoadSprite(slotType, gender, avatarId, CurrentAnimFrame.AnimEnum,CurrentAnimFrame.FrameID);
    }


    private int CurrentOrder()
    {

        return defaultSortOrder;
    }


    public void ChangeAvatarId(uint avatarId)
    {
        this.avatarId = avatarId;

#if UNITY_EDITOR
        SetSpriteIdle();
#endif


    }

    private void SetSpriteIdle()
    {
        SpriteRender.sprite = 
        SpriteCollector.LoadSprite(slotType, gender, avatarId, AvatarAnimEnum.Idle00, 0);
    }
}



