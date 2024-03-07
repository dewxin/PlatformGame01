using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ScriptableObj
{
    [CreateAssetMenu(fileName = "SpawnPrefabEffect", menuName = "PlatformGameObj/Skill/Effect/SpawnPrefabEffect")]
    public class SpawnPrefabEffect : BaseEffect
    {
        public GameObject prefab;
        public Vector2 localPos;
        //todo 后面可以扩展选择挂载到 target上

        private GameObject instance;
        public override void Start()
        {
            Debug.Log("dddd");
            instance = Instantiate(prefab, CasterSceneUnit.SelfGameObj.transform);
            instance.transform.localPosition = localPos;

        }

        public override void OnDestroy()
        {
            Destroy(instance);
        }

    }
}
