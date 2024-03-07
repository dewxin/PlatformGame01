using Assets.Scripts.ScriptableObj;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class SceneUnitAvatarHolder : MonoBehaviour
{
    public AvatarAnimFrame currentAnimFrame => currentAnimation.SpriteAnimIdList[CurrentFrameId];

    private ActionAnimation currentAnimation;

    public int CurrentFrameId { get; private set; }
    private float currentFrameTime = 0;
    private int animFrameCount => currentAnimation.SpriteAnimIdList.Count;

    public float second1Frame => 1f / currentAnimation.FramePerSecond; 

    private bool paused = false;
    public bool Paused => paused;

    private Dictionary<AvatarSlotTypeEnum, SpriteHolder> slotType2SpriteHolderDict = new Dictionary<AvatarSlotTypeEnum, SpriteHolder>();
    public bool Once { get; private set; }
    public bool Finished { get; private set; }

    private Action OnPlayOnceOver = delegate { };
    public Action<AnimEventEnum> OnTriggerAnimeEvent = delegate { };


    public string SortingLayer { get; set; } = "Middle";
    public int SortingOrder { get; set; } = -1;

    private void Awake()
    {
    }


    private void Start()
    {
        //if (slotType2SpriteHolderDict.Count == 0)
        //    Debug.LogError("slot dict is not supposed to be empty");
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    private void Update()
    {
        UpdateFrameId();
    }

    private void UpdateFrameId()
    {
        if (paused)
            return;
        if (Finished)
            return;
        currentFrameTime += Time.deltaTime;
        if (currentFrameTime > second1Frame)
        {
            currentFrameTime -= second1Frame;
            CurrentFrameId++;

            var eventList = currentAnimation.GetEventsAtFrame(CurrentFrameId);
            foreach(var oneEvent in eventList)
            {
                OnTriggerAnimeEvent(oneEvent.Event);
            }

            if (CurrentFrameId >= animFrameCount)
            {
                CurrentFrameId = 0;

                if (Once)
                {
                    //reset the frame id as the last
                    CurrentFrameId = animFrameCount- 1;
                    Finished = true;

                    if (OnPlayOnceOver != null)
                        OnPlayOnceOver();
                }

            }
        }
    }

    public void GenerateSlotAvatar(List<AvatarSlotTypeEnum> slotTypeList)
    {
        GameObject viewGO = new GameObject("View");
        viewGO.transform.SetParent(transform, false);
        //viewGO.AddComponent<SpriteMerge>();

        foreach (AvatarSlotTypeEnum slotTypeEnum in slotTypeList )
        {
            Generate1Slot(viewGO,slotTypeEnum);
        }

    }

    private void Generate1Slot(GameObject slotParent,AvatarSlotTypeEnum slotTypeEnum)
    {
        GameObject gameObject = new GameObject();
        gameObject.name = slotTypeEnum.ToString();
        gameObject.layer = LayerMask.NameToLayer("Player");

        var spriteRender = gameObject.AddComponent<SpriteRenderer>();
        spriteRender.sortingLayerName = SortingLayer;
        spriteRender.sortingOrder = SortingOrder;
        if(SortingOrder < 0)
        {
            spriteRender.sortingOrder = AvatarConfig.Instance.GetInitSortOrder(slotTypeEnum);
        }

        var spriteHolder = gameObject.AddComponent<SpriteHolder>();
        spriteHolder.avatarHolder = this;
        spriteHolder.slotType = slotTypeEnum;
        spriteHolder.SpriteRender = spriteRender;


        slotType2SpriteHolderDict.Add(slotTypeEnum, spriteHolder);

        gameObject.transform.SetParent(slotParent.transform, false);
    }


    public void Play(AvatarAnimEnum animEnum, bool once = false, Action callbackOnFinishing = null, bool ignoreSameAnimation = true)
    {
        var asset = Path.Combine("ScriptableObj/Animation", animEnum.ToString());
        var animation = Resources.Load<ActionAnimation>(asset);

        if(animation == null)
        {
            Debug.LogError($"cannot find asset {asset}");
            return;
        }

        if (ignoreSameAnimation && currentAnimation == animation)
            return;

        Play(animation, once, callbackOnFinishing);
    }

    public void Play(ActionAnimation animEnum)
    {
        Play(animEnum, false);
    }

    public void Play(ActionAnimation actionAnimation, bool once = false, Action callbackOnFinishing = null)
    {
        //Debug.Log($"play animation {actionAnimation.name}");

        if (once)
        {
            this.OnPlayOnceOver = callbackOnFinishing;
        }

        this.Once = once;
        paused = false;
        Finished = false;
        CurrentFrameId = 0;
        currentFrameTime = 0;
        currentAnimation = actionAnimation;
    }


    public void Pause()
    {
        paused= true;
    }

    public void Resume()
    {
        paused = false;
    }

    public void ChangeGender(AvatarGenderEnum gender)
    {
        foreach(var spriteHolder in slotType2SpriteHolderDict.Values)
        {
            spriteHolder.gender = gender;
        }

    }

    public void ChangeSlotAvatarId(AvatarSlotTypeEnum slotType, uint avatarId)
    {
        var spriteHolder = slotType2SpriteHolderDict[slotType];
        spriteHolder.ChangeAvatarId(avatarId);
    }

    public List<SpriteHolder> GetSpriteHolderList()
    {
        return slotType2SpriteHolderDict.Values.ToList();
    }
}
