using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GenPlatformColliderTool : MonoBehaviour
{
    [SerializeField]
    private float horizontalColliderWidth = 0.1f;

    [SerializeField]
    private float verticalColliderWidth = 0.3f;

    //
    private Dictionary<(Transform,Transform), Platform> twoPoint2PlatformDict = new Dictionary<(Transform,Transform), Platform>();
    void Start()
    {
        
    }


    public GameObject GenerateColliders()
    {
        twoPoint2PlatformDict.Clear();

        var colliderRootName = this.name + "Collider";

        var colliderTransform = transform.parent.Find(colliderRootName);;
        if(colliderTransform != null)
        {
            DestroyImmediate(colliderTransform.gameObject);
        }

        var colliderParentGO = new GameObject();
        colliderParentGO.name = colliderRootName;
        colliderParentGO.transform.SetParent(transform.parent);

        //GenerateHorizontalPlatform
        foreach (Transform platform in transform)
        {
            if (platform.tag != "HorizontalPlatform")
                continue;

            System.Collections.Generic.List<Transform> childTransformList = new System.Collections.Generic.List<Transform>();
            foreach (Transform child in platform)
                childTransformList.Add(child);
            childTransformList.Sort((v1, v2) => (int)(v1.position.x - v2.position.x));
            AttachHorizontalColliderAsChild(childTransformList, colliderParentGO);
        }

        foreach (Transform platform in transform)
        {
            if (platform.tag != "VerticalPlatform")
                continue;

            System.Collections.Generic.List<Transform> childTransformList = new System.Collections.Generic.List<Transform>();
            foreach (Transform child in platform)
                childTransformList.Add(child);
            childTransformList.Sort((v1, v2) => (int)(v1.position.y - v2.position.y));
            AttachVerticalColliderAsChild(childTransformList, colliderParentGO);
        }


        return colliderParentGO;
    }


    private void AttachHorizontalColliderAsChild(System.Collections.Generic.List<Transform> pointList, GameObject colliderParentGO)
    {

        var platformList = new System.Collections.Generic.List<Platform>();
        for (int i = 0; i < pointList.Count - 1; i++)
        {
            var fromPos = pointList[i];
            var toPos = pointList[i + 1];
            var colliderGO = GenerateCollider(fromPos, toPos, colliderParentGO, false);

            var platform = AttachSubPlatform(colliderGO, fromPos, toPos);
            platformList.Add(platform);

            twoPoint2PlatformDict.Add((fromPos, toPos), platform);
        }


        LinkPlatform(platformList);

    }

    private GameObject GenerateCollider(Transform fromPos, Transform toPos, GameObject colliderParentGO, bool vertical)
    {
        var direction = toPos.position - fromPos.position;

        var colliderGO = new GameObject();
        colliderGO.layer = LayerMask.NameToLayer("Platform");
        if(vertical)
            colliderGO.layer = LayerMask.NameToLayer("ClimbPlatform");
        colliderGO.transform.position = (toPos.position + fromPos.position) / 2;
        colliderGO.transform.SetParent(colliderParentGO.transform);


        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        colliderGO.transform.rotation = Quaternion.Euler(0, 0, angle);

        var collider = colliderGO.AddComponent<CapsuleCollider2D>();
        collider.direction = CapsuleDirection2D.Horizontal;
        colliderGO.name = "colliderPlatform" + colliderParentGO.transform.childCount;

        //这里缩小一点长度，避免玩家卡住
        collider.size = new Vector2(direction.magnitude-0.01f /*+ horizontalColliderWidth*/, horizontalColliderWidth);
        if(vertical)
            collider.size = new Vector2(direction.magnitude - 0.01f , verticalColliderWidth);


        return colliderGO;
    }

    private Platform AttachSubPlatform(GameObject colliderGO, Transform from, Transform to)
    {
        var platform = colliderGO.AddComponent<Platform>();
        platform.StartPoint = from.position;
        platform.EndPoint = to.position;

        return platform;
    }

    private void LinkPlatform(System.Collections.Generic.List<Platform> platformList)
    {

        Platform prevPlatform = null;
        foreach (Platform platform in platformList)
        {
            if (prevPlatform != null)
            {
                prevPlatform.NextPlatform = platform;
                platform.PrevPlatform = prevPlatform;
            }

            prevPlatform = platform;
        }
    }



    private void AttachVerticalColliderAsChild(System.Collections.Generic.List<Transform> pointList, GameObject colliderParentGO)
    {
        for (int i = 0; i < pointList.Count - 1; i++)
        {
            var fromPos = pointList[i];
            var toPos = pointList[i + 1];
            var colliderGO = GenerateCollider(fromPos, toPos, colliderParentGO, true);


            var platform = AttachSubPlatform(colliderGO, fromPos, toPos);
            platform.Direct = Platform.Direction.Vertical;

            var fromPosInfo = fromPos.GetComponent<VerticalPlatformPoint>();
            platform.PrevPlatform = twoPoint2PlatformDict[(fromPosInfo.horizontalPlatformStart, fromPosInfo.horizontalPlatformEnd)];
            var toPosInfo = toPos.GetComponent<VerticalPlatformPoint>();
            platform.NextPlatform = twoPoint2PlatformDict[(toPosInfo.horizontalPlatformStart, toPosInfo.horizontalPlatformEnd)];
        }


    }
}
