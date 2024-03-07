using Assets.Scripts.ScriptableObj;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortCutSlotManager : MonoBehaviour
{
    // Start is called before the first frame update
    private Dictionary<KeyCode, ShortCutSlot> keyCode2SlotDict = new Dictionary<KeyCode, ShortCutSlot>();

    public ShortCutSlotCoolDown ShortCutSlotCoolDown;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var slotItem in keyCode2SlotDict.Values)
        {
            if(Input.GetKey(slotItem.KeyCode))
            {
                slotItem.Use();
            }
        }
    }


    public void RegisterShortCutSlot(ShortCutSlot slot)
    {
        keyCode2SlotDict[slot.KeyCode] = slot;
    }

}
