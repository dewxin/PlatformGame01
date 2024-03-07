using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ScriptableObj
{
    [CreateAssetMenu(fileName = "ChangeStateFightingEffect", menuName = "PlatformGameObj/Skill/Effect/ChangeStateFightingEffect")]
    public class ChangeStateFightingEffect : BaseEffect
    {
        public AudioClip AudioClip;
        public AudioClip OriginClip { get; private set; }

        public AudioSource OriginSource { get; private set; }

        public override bool IsUnique => true;

        public ChangeStateFightingEffect()
        {
            Duration = 1;
        }

        public override void Start()
        {
            OriginSource = SoundManager.Instance.MusicAudioSource;
            OriginClip = SoundManager.Instance.MusicAudioSource.clip;
            SoundManager.Instance.PlayMusic(AudioClip);

            PlayerSingleton.Instance.State.IsFighting = true;
            //todo 改成  CasterSceneUnit.StateManager.EnterNewState

        }

        public override void OnDestroy() 
        {
            //duplicate destory
            if (OriginClip == null)
                return;


            PlayerSingleton.Instance.State.IsFighting = false;

            //不是之前改的那个音源了，过滤算了
            if (OriginSource != SoundManager.Instance.MusicAudioSource)
                return;

            //todo
            //CasterSceneUnit.StateManager.EnterNewState(StateType.Idle);
            SoundManager.Instance.PlayMusic(OriginClip);
        }

        public override void OnAddDuplicateUnique(BaseEffect newEffect)
        {
            var newFightEffect = newEffect as ChangeStateFightingEffect;
            this.ExpireTime = newFightEffect.ExpireTime;
            this.AudioClip = newFightEffect.AudioClip;
        }
    }
}
