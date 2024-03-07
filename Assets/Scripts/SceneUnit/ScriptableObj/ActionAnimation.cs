using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ScriptableObj
{

    [CreateAssetMenu(fileName = "ActionAnim", menuName = "PlatformGameObj/Skill/ActionAnim", order = 1)]
    public class ActionAnimation : ScriptableObject
    {
        public string ActionAnimName;
        public float Duration;
        public List<AvatarAnimFrame> SpriteAnimIdList = new List<AvatarAnimFrame>();
        public List<AvatarAnimEvent> EventList = new List<AvatarAnimEvent>();

        public float FramePerSecond => SpriteAnimIdList.Count / Duration;


        public List<AvatarAnimEvent> GetEventsAtFrame(int frameId)
        {
            var list = new List<AvatarAnimEvent>();
            foreach(var animEvent in EventList)
            {
                //TODO要不要换成string，感觉还是string好一点
                if(animEvent.FrameID == frameId)
                {
                    list.Add(animEvent);
                }
            }

            return list;
        }


        public static ActionAnimation CreateMonsterAnimation(uint avatarId,AvatarAnimEnum avatarAnimEnum, float FramePerSecond)
        {

            var spriteCount = SpriteCollector.QueryCount(AvatarSlotTypeEnum.body, AvatarGenderEnum.O, avatarId, avatarAnimEnum);

            var secondPerFrame = 1 / FramePerSecond;

            var duration = secondPerFrame * spriteCount;

            var animation = CreateInstance<ActionAnimation>();
            animation.Duration = duration;

            for(int i = 0; i < spriteCount; i++)
            {
                animation.SpriteAnimIdList.Add(new AvatarAnimFrame { AnimEnum= avatarAnimEnum, FrameID = i });
            }

            return animation;
            
        }
    }
}
