using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
//public class Key
//{
//    private KeyCode code;

//    public Key(KeyCode code)
//    {
//        this.code = code;
//    }

//    public static Key operator = (Key a, Key b)
//    {

//    }


//    public bool GetKeyDown()
//    {

//    }

//    public bool GetKey()
//    {

//    }

//}

[CreateAssetMenu(fileName = "ControlsConfig", menuName = "ScriptableObjects/ControlsConfig", order = 2)]
public class ControlsScriptableObject: ScriptableObject
{
    public KeyCode keyCTRL = KeyCode.LeftControl;
    public KeyCode keyUndo = KeyCode.Z;
    public KeyCode keyPing = KeyCode.P;
    public KeyCode keySwitchTool = KeyCode.Tab;
    public KeyCode keyHeight = KeyCode.Space;
    public bool keysDisabled = false;


    public void ChangeToolKey(string toolToChange, KeyCode newKey)
    {

    }

    //public bool GetShortcut(Key[] keys)
    //{
    //    return false;
    //}
}
