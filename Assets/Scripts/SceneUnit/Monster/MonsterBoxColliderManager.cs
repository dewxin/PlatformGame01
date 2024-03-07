using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MonsterBoxColliderManager : MonoBehaviour, IHasSceneUnitInfo
{
    public SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    public Monster monsterUnit;
    public Transform viewRoot;

    void Start()
    {
        boxCollider= GetComponent<BoxCollider2D>();
        

    }

    // Update is called once per frame
    void Update()
    {
        UpdateCollider();

    }

    private void UpdateCollider()
    {
        if (spriteRenderer == null)
            return;

        var bounds = spriteRenderer.sprite.bounds;

        boxCollider.offset = bounds.center;
        boxCollider.size = bounds.size;
    }

    public SceneUnitInfo GetSceneUnit()
    {
        monsterUnit.sceneUnitInfo.SceneUnitInfoOwner = this;
        return monsterUnit.sceneUnitInfo;
    }

    public SpriteRenderer GetSpriteRenderer()
    {
        return spriteRenderer;
    }

    public Transform GetViewRoot()
    {
        return viewRoot;
    }
}
