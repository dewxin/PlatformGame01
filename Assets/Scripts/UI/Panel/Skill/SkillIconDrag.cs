using Assets.Scripts.ScriptableObj;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;
using WildBoar.GUIModule;

public class SkillIconDrag : MonoBehaviour, IBeginDragHandler, IOnDragHandler, IEndDragHandler
{

    private NUICanvas canvas;
    private Camera uiCamera;

    private Vector3 positionBeforeDrag;

    private SkillDisplaySlot skillDisplaySlot;
    private ShortCutSlot shortCutSlot;

    private Skill_ScriptableObj skill;
    public Skill_ScriptableObj Skill => skill;
    private UISprite sprite;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponentInParent<NUICanvas>();
        skillDisplaySlot = GetComponentInParent<SkillDisplaySlot>();
        skill = skillDisplaySlot.SkillScriptObj;
        sprite = GetComponent<UISprite>();
        uiCamera = canvas.Camera;

    }
    // Update is called once per frame
    void Update()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        sprite.SetRendererSortingOrder(sprite.SortingOrder + 1);
        transform.SetParent(canvas.transform, true);
        if(skillDisplaySlot!= null) 
        {
            skillDisplaySlot.CreateNewSkillIcon();
            skillDisplaySlot = null;
        }

        //positionBeforeDrag = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = uiCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var ray = uiCamera.ScreenPointToRay(Input.mousePosition);

        bool findSlot = false;
        LayerMask mask = LayerMask.GetMask("UI");
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 100, mask, QueryTriggerInteraction.Collide))
        {
            var shortcut = raycastHit.transform.GetComponent<ShortCutSlot>();
            if(shortcut!= null)
            {
                if(shortCutSlot!=null)
                {
                    shortCutSlot.OnSkillRemove();
                }

                //reset sorting order
                sprite.SetRendererSortingOrder(sprite.SortingOrder);
                shortcut.OnSkillDrop(this);
                shortCutSlot = shortcut;
                findSlot = true;
            }

        }

        if(!findSlot)
        {
            Destroy(gameObject);
        }



    }
}
