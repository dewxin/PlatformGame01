using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class SpawnMonster : MonoBehaviour
{
    public MonsterNameEnum monsterTemplateId;
    [Range(1f,10f)]
    public int count;

    private const string CONST_PREVIEW = "Preview";
    private const string CONST_NAMPLATE_PATH = @"Prefabs/Monsters/Nameplate";
    private const string CONST_COUNT_PATH = @"Prefabs/Monsters/Count";

    private bool dirty;


    // Start is called before the first frame update
    void Start()
    {
        if(Application.isPlaying)
        {
            DisablePreview();
            SpawnMonsters();
        }

    }



    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        UpdatePreviewIfDirty();
#endif


    }




    private void OnValidate()
    {
        dirty = true;
    }

    #region Spawn Game Object

    private void SpawnMonsters()
    {
        var monsterGroup = new GameObject("MonsterGroup");
        monsterGroup.transform.SetParent(transform, false);

        for (int i = 0; i < count; i++)
        {
            var monsterGO = Spawn1Monster(i);
            monsterGO.transform.SetParent(monsterGroup.transform, false);
        }
    }

    /// <summary>
    /// -Monster 
    ///  -Visual (transform z is diffrent for better visual effect)
    ///   -3707(monsterView)
    ///   -Nameplate
    ///   -Collider(for mouse click and outline)
    /// 
    /// </summary>
    private GameObject Spawn1Monster(int index)
    {
        var monsterConfig = MonsterConfig.Get(monsterTemplateId);

        var monsterGO = new GameObject(monsterConfig.Name+index);
        monsterGO.AddComponent<SimpleStateManager>();
        var monster = monsterGO.AddComponent<Monster>();
        monster.config = monsterConfig;


        var visualGO = SetVisual(monsterGO, index);
        SetNameplate(visualGO, monsterConfig.Name);
        var viewGO = SetMonsterAvatar(visualGO, monsterConfig.ViewId);

        var spriteRenderer = viewGO.GetComponentInChildren<SpriteRenderer>();
        SetCollider(visualGO, spriteRenderer, monster);



        return monsterGO;
    }

    private GameObject SetVisual(GameObject parentGO,int index)
    {
        var visualGO = new GameObject("Visual");
        visualGO.transform.position = new Vector3(0, 0, -0.1f * index);
        visualGO.transform.SetParent(parentGO.transform, false);

        return visualGO;

    }

    private GameObject SetCollider(GameObject parentGO, SpriteRenderer monsterViewRenderer,Monster monster)
    {
        var colliderGO = new GameObject("Collider");
        colliderGO.layer = LayerMask.NameToLayer("Enemy");
        colliderGO.transform.SetParent(parentGO.transform, false);
        colliderGO.AddComponent<BoxCollider2D>();
        var colliderManager = colliderGO.AddComponent<MonsterBoxColliderManager>();
        colliderManager.spriteRenderer = monsterViewRenderer;
        colliderManager.monsterUnit = monster;
        colliderManager.viewRoot = parentGO.transform;

        return colliderGO;

    }

    #endregion


    #region Preview Game Object

    private void DisablePreview()
    {
        if (Application.isPlaying)
        {
            var previewTransform = transform.Find(CONST_PREVIEW);
            if (previewTransform != null)
            {
                previewTransform.gameObject.SetActive(false);
                Destroy(previewTransform.gameObject);
            }
        }

    }

    private void UpdatePreviewIfDirty()
    {
        if (Application.isPlaying)
            return;

        if (dirty)
        {
            SetPreviewByTemplateId((int)monsterTemplateId);

            dirty = false;

        }
    }

    private void SetPreviewByTemplateId(int templateId)
    {
        var previewObj = GetPreviewGameObj();

        var config = MonsterConfig.Get(templateId);

        SetNameplate(previewObj, config.Name);
        SetCount(previewObj, count);
        SetMonsterAvatar(previewObj, config.ViewId);

    }

    private GameObject GetPreviewGameObj()
    {
        var previewTransform = transform.Find(CONST_PREVIEW);
        if (previewTransform != null)
        {
            DestroyImmediate(previewTransform.gameObject);
        }
        GameObject preview = new GameObject(CONST_PREVIEW);
        preview.transform.SetParent(transform, false);
        previewTransform = preview.transform;
        return previewTransform.gameObject;
    }

    private void SetNameplate(GameObject parentGO,string name)
    {
        var prefab = Resources.Load<GameObject>(CONST_NAMPLATE_PATH);
        var nameplateGO = MonoBehaviour.Instantiate(prefab);
        nameplateGO.transform.SetParent(parentGO.transform, false);
        var nameplate = nameplateGO.GetComponent<Nameplate>();
        nameplate.ChangeName(name);
    }
    private void SetCount(GameObject parentGO, int count)
    {
        var prefab = Resources.Load<GameObject>(CONST_COUNT_PATH);
        var nameplateGO = MonoBehaviour.Instantiate(prefab);
        nameplateGO.transform.SetParent(parentGO.transform, false);
        var textMeshPro = nameplateGO.GetComponent<TextMeshPro>();
        textMeshPro.text = count.ToString();
    }

    private GameObject SetMonsterAvatar(GameObject parentGO, uint viewId)
    {
        var viewGO = new GameObject("AvatarHolder");
        viewGO.transform.SetParent(parentGO.transform, false);
        
        var avatarHolder = viewGO.AddComponent<SceneUnitAvatarHolder>();
        avatarHolder.SortingLayer = "Back";
        avatarHolder.SortingOrder = 100;
        avatarHolder.GenerateSlotAvatar(new List<AvatarSlotTypeEnum>() { AvatarSlotTypeEnum.body });
        avatarHolder.ChangeGender(AvatarGenderEnum.O);
        avatarHolder.ChangeSlotAvatarId(AvatarSlotTypeEnum.body, viewId);

        //spriteRenderer.sortingLayerName = "Back";
        //spriteRenderer.sortingOrder = 100;

        return viewGO;
    }


    #endregion
}
