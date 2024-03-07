using Assets.Scripts.ScriptableObj;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WildBoar.GUIModule;

[ExecuteAlways]
public class SkillDisplaySlot : MonoBehaviour
{

    public Skill_ScriptableObj SkillScriptObj;

    public GameObject SkillIconPrefab;

    [SerializeField]
    private UISprite iconSprite;
    // Start is called before the first frame update
    void Start()
    {
        InstantiateSkillIcon();
        InitSprite(iconSprite);

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void InstantiateSkillIcon()
    {
        if (iconSprite != null)
            return;
        Create1SkillIcon();
    }

    private void Create1SkillIcon()
    {
        var skillIconGO = MonoBehaviour.Instantiate(SkillIconPrefab, this.transform);
        skillIconGO.transform.localPosition = Vector3.zero;
        iconSprite = skillIconGO.GetComponent<UISprite>();
        InitSprite(iconSprite);
    }

    public void CreateNewSkillIcon()
    {
        Create1SkillIcon();
    }

    public void InitSprite(UISprite uiSprite)
    {
        uiSprite.SetAtlas(SkillScriptObj.SpriteAtlas);
        uiSprite.SetSprite(SkillScriptObj.SpriteName);
    }

}
