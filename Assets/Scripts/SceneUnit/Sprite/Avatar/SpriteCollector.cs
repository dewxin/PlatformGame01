using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SpriteCollector
{

    private static string PathPrefix = @"Visual\Sprites\Avatar";
 

    public static Sprite LoadSprite(AvatarSlotTypeEnum slotType, AvatarGenderEnum gender, uint avatarId, AvatarAnimEnum animId, int animFrameId)
    {
        return DataCache.Instance.GetSprite(slotType, gender, avatarId,  animId , animFrameId);
    }

    public static int QueryCount(AvatarSlotTypeEnum slotType, AvatarGenderEnum gender, uint avatarId, AvatarAnimEnum animID)
    {
        return DataCache.Instance.QuerySpriteCount(slotType, gender, avatarId, animID);
    }


    private class DataCache:Singleton<DataCache>
    {

        private Dictionary<(AvatarSlotTypeEnum, AvatarGenderEnum, uint), OneAvatarSprites> condition2AvatarDict 
            = new Dictionary<(AvatarSlotTypeEnum, AvatarGenderEnum, uint), OneAvatarSprites>();


        public void Load(AvatarSlotTypeEnum slotType, AvatarGenderEnum gender, uint avatarId)
        {
            var resourcePath = Path.Combine(PathPrefix, slotType.ToString(), gender.ToString(), avatarId.ToString());
            var spriteArray = Resources.LoadAll<Sprite>(resourcePath);

            //Debug.Log($"Total loaded sprite count is {spriteArray.Length}");

            if (spriteArray.Length == 0)
                Debug.LogWarning($"{resourcePath} cannot find any thing");

            OneAvatarSprites avatar = new OneAvatarSprites();
            avatar.Init(spriteArray);

            condition2AvatarDict.Add((slotType, gender, avatarId), avatar);
        }

        public Sprite GetSprite(AvatarSlotTypeEnum slotType, AvatarGenderEnum gender, uint avatarId, AvatarAnimEnum animId, int animFrameId)
        {
            if(!condition2AvatarDict.ContainsKey((slotType,gender,avatarId)))
            {
                //Debug.Log($"try load Avatar slotType={slotType} gender={gender} avatarId={avatarId} animId={animId}");
                Load(slotType,gender,avatarId);
            }

            return condition2AvatarDict[(slotType,gender,avatarId)].GetSprite(animId,animFrameId);
        }

        //for monster sprite
        public int QuerySpriteCount(AvatarSlotTypeEnum slotType, AvatarGenderEnum gender, uint avatarId, AvatarAnimEnum animID)
        {
            if (!condition2AvatarDict.ContainsKey((slotType, gender, avatarId)))
            {
                //Debug.Log($"try load Avatar slotType={slotType} gender={gender} avatarId={avatarId} animId={animId}");
                Load(slotType, gender, avatarId);
            }

            return condition2AvatarDict[(slotType, gender, avatarId)].GetAnimCount(animID);

        }

    }


    public class OneAvatarSprites
    {

        private Dictionary<AvatarAnimEnum, List<Sprite>> animId2SpriteListDict = new Dictionary<AvatarAnimEnum, List<Sprite>>();

        public void Init(Sprite[] sprites)
        {
            foreach (var sprite in sprites)
            {
                var spriteAnimEnumStr = sprite.name.Split('-')[0];

                var animEnum = (AvatarAnimEnum)(int.Parse(spriteAnimEnumStr));

                if(!animId2SpriteListDict.ContainsKey(animEnum))
                {
                    animId2SpriteListDict.Add(animEnum, new List<Sprite>());
                }

                animId2SpriteListDict[animEnum].Add(sprite);

            }

        }

        public Sprite GetSprite(AvatarAnimEnum animEnum, int animFrameId)
        {
            var spriteList = animId2SpriteListDict[animEnum];
            if (spriteList == null)
                return null;

            if(animFrameId >= spriteList.Count)
            {
                Debug.LogWarning($"frame ID {animFrameId} exceeds the range of array");
                return null;
            }

            return spriteList[animFrameId];
        }

        public int GetAnimCount(AvatarAnimEnum animEnum)
        {
            var spriteList = animId2SpriteListDict[animEnum];
            if (spriteList == null)
                return 0;
            return spriteList.Count;
        }

    }

}
