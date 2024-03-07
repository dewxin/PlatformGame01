using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boundry : MonoBehaviour
{
    //TODO 改成和scene相关的

    public static RectBoundry MapBoundry;

    public bool DrawGizmos = false;

    public static Boundry FindBoundryInAncestorChild(Transform transform)
    {
        Transform parentTransform = transform.parent;
        while (parentTransform != null)
        {
            var boundry = parentTransform.GetComponentInChildren<Boundry>();
            if (boundry != null)
                return boundry;

            parentTransform = parentTransform.parent;
        }

        return null;
    }

}
