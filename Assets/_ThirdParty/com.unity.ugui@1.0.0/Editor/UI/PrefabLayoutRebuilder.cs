using UnityEditor;
using UnityEngine;
using WildBoar.UI;

namespace WildBoarEditor.UI
{
    [InitializeOnLoad]
    internal class PrefabLayoutRebuilder
    {
        static PrefabLayoutRebuilder()
        {
            PrefabUtility.prefabInstanceUpdated += OnPrefabInstanceUpdates;
        }

        static void OnPrefabInstanceUpdates(GameObject instance)
        {
            if (instance)
            {
                RectTransform rect = instance.transform as RectTransform;
                if (rect != null)
                    LayoutRebuilder.MarkLayoutForRebuild(rect);
            }
        }
    }
}
