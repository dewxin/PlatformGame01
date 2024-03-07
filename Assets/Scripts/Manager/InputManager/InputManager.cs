using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InputManager : Singleton<InputManager> 
{
    private Dictionary<InputEnum, KeyCode> input2KeyCodeDict = new Dictionary<InputEnum, KeyCode>();

    protected override void Init()
    {
        input2KeyCodeDict.Add(InputEnum.MoveLeft, KeyCode.A);
        input2KeyCodeDict.Add(InputEnum.MoveRight, KeyCode.D);
        input2KeyCodeDict.Add(InputEnum.MoveUp, KeyCode.W);
        input2KeyCodeDict.Add(InputEnum.MoveDown, KeyCode.S);
        input2KeyCodeDict.Add(InputEnum.Jump, KeyCode.Space);

        input2KeyCodeDict.Add(InputEnum.ShortCutSlot1, KeyCode.Alpha1);
        input2KeyCodeDict.Add(InputEnum.ShortCutSlot2, KeyCode.Alpha2);
        input2KeyCodeDict.Add(InputEnum.ShortCutSlot3, KeyCode.Alpha3);
        input2KeyCodeDict.Add(InputEnum.ShortCutSlot4, KeyCode.Alpha4);
        input2KeyCodeDict.Add(InputEnum.ShortCutSlot5, KeyCode.Alpha5);
        input2KeyCodeDict.Add(InputEnum.ShortCutSlot6, KeyCode.Alpha6);
        input2KeyCodeDict.Add(InputEnum.ShortCutSlot7, KeyCode.Alpha7);
        input2KeyCodeDict.Add(InputEnum.ShortCutSlot8, KeyCode.Alpha8);
        input2KeyCodeDict.Add(InputEnum.ShortCutSlot9, KeyCode.Alpha9);

    }

    public KeyCode GetKeyCode(InputEnum inputEnum)
    {
        return input2KeyCodeDict[inputEnum];
    }

    public bool GetInputDown(InputEnum inputEnum)
    {
        if (!input2KeyCodeDict.ContainsKey(inputEnum))
        {
            throw new System.Exception("input not registered");
        }

        var keyCode = input2KeyCodeDict[inputEnum];

        return Input.GetKeyDown(keyCode);
    }

    public bool GetInput(InputEnum inputEnum)
    {
        if (!input2KeyCodeDict.ContainsKey(inputEnum))
        {
            throw new System.Exception("input not registered");
        }

        var keyCode = input2KeyCodeDict[inputEnum];

        return Input.GetKey(keyCode);
    }

}
