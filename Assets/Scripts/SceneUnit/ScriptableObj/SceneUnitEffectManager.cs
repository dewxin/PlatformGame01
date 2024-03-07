using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ScriptableObj
{
    public class SceneUnitEffectManager:MonoBehaviour
    {
        public List<BaseEffect> effectList = new List<BaseEffect>();

        private SceneUnitInfo sceneUnit;

        public void Start()
        {
            var sceneUnitGetter = GetComponent<IHasSceneUnitInfo>();
            sceneUnit = sceneUnitGetter.GetSceneUnit();
        }

        public void Update() 
        {
            foreach (var state in effectList)
            {
                state.Update();
            }
            RemoveExpiredEffects();
        }

        private void RemoveExpiredEffects()
        {
            var expireList = new List<BaseEffect>();
            foreach (var effect in effectList)
            {
                if (effect.ExpireTime < Time.time)
                    expireList.Add(effect);
            }

            foreach (var effect in expireList)
            {
                effectList.Remove(effect);
                Destroy(effect);
            }
        }

        //public void AddEffect<T>() where T : BaseEffect
        //{
        //    var state = ScriptableObject.CreateInstance<T>();

        //    AddEffect(state);
        //}

        public void AddEffect(BaseEffect effect) 
        {

            effect.ExpireTime = Time.time + effect.Duration;

            BaseEffect oldUnique = null;
            if(effect.IsUnique && (oldUnique = FindFirstSameTypeEffect(effect))!=null)
            {
                oldUnique.OnAddDuplicateUnique(effect);
                Destroy(effect);
            }
            else
            {
                effect.Start();
                effectList.Add(effect);
            }

        }

        public bool HasEffect<T>() where T : BaseEffect
        {
            return effectList.Any(state => state is T);
        }

        public BaseEffect FindFirstSameTypeEffect(BaseEffect effect)
        {
            return effectList.Find(state => state.GetType() == effect.GetType());
        }

    }
}
