#if UNITY_EDITOR
using Assets.Scripts.ScriptableObj;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using WildBoar.EventSystems;
using WildBoar.UI;

public class SkillEditorScrollItem : MonoBehaviour,IPointerClickHandler
{

    public ScriptableObject ScriptableObject { get; private set; }

    public TextMeshProUGUI TextMeshPro_Text;
    public Image skillIcon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScriptableObj(ScriptableObject obj)
    {
        this.ScriptableObject= obj;
        TextMeshPro_Text.SetText(obj.name);
        var skillObj = obj as Skill_ScriptableObj;
        if(skillObj != null)
        {
            //todo
            //skillIcon.sprite = skillObj.Icon;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EditorGUIUtility.PingObject(ScriptableObject);


        var skillObj = ScriptableObject as Skill_ScriptableObj;
        if (skillObj != null)
        {
        }

        Debug.Log(ScriptableObject.name);
    }
}
#endif
