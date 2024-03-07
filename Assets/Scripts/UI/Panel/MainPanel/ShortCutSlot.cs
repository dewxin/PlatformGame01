using Assets.Scripts.ScriptableObj;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ShortCutSlot : MonoBehaviour
{
    public InputEnum InputEnum;
    public KeyCode KeyCode { get; private set; }
    private TextMeshPro textMeshPro;

    private ShortCutSlotManager manager;


    private SkillIconDrag skillIconDrag;
    private ShortCutSlotItem item;

    private ShortCutSlotCoolDown coolDownMask;

    void Start()
    {
        manager= GetComponentInParent<ShortCutSlotManager>();
        textMeshPro= GetComponentInChildren<TextMeshPro>();

        coolDownMask = Instantiate(manager.ShortCutSlotCoolDown);
        coolDownMask.transform.SetParent(transform, false);
        coolDownMask.transform.transform.localPosition = Vector3.zero;



        HandleInputAndKeyCode();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HandleInputAndKeyCode()
    {
        KeyCode = InputManager.Instance.GetKeyCode(InputEnum);

        textMeshPro.SetText(KeyCode.ToString().Replace("Alpha",""));
        manager.RegisterShortCutSlot(this);
    }

    public void OnSkillDrop(SkillIconDrag skillIconDrag)
    {
        if(this.skillIconDrag!=null)
        {
            Destroy(this.skillIconDrag.gameObject);
        }

        this.skillIconDrag = skillIconDrag;

        skillIconDrag.transform.SetParent(transform, false);
        skillIconDrag.transform.transform.localPosition = Vector3.zero;

        item = new ShortCutSlotItem() { Skill = skillIconDrag.Skill };
    }

    public void OnSkillRemove()
    {
        skillIconDrag = null;
        item = null;
    }

    public void Use()
    {
        if(item!=null)
        {
            if(item.TryUseAndRetCoolDown(out var coolDown))
            {
                this.coolDownMask.SetCoolDown(coolDown);
            }
        }

    }


}
