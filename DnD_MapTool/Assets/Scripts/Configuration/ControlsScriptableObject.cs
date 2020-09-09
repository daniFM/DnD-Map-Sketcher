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
public class ControlsScriptableObject : ScriptableObject
{
    public KeyCode keyShortcut = KeyCode.C;
    public KeyCode keyPan = KeyCode.Mouse1;
    public KeyCode keyRotateLeft = KeyCode.Q;
    public KeyCode keyRotateRight = KeyCode.E;
    public KeyCode keyPaint = KeyCode.Mouse0;
    public KeyCode keyInteract = KeyCode.Mouse0;
    public KeyCode keySwitchTool = KeyCode.Tab;
    public KeyCode keyHeight = KeyCode.Space;
    public KeyCode keyPing = KeyCode.P;
    public KeyCode keyHide = KeyCode.H;
    public KeyCode keyCTRL = KeyCode.LeftControl;
    public KeyCode keyUndo = KeyCode.Z;
    public bool keysDisabled = false;


    public void ChangeToolKey(string toolToChange, KeyCode newKey)
    {

        if (toolToChange == "keyshOrtcut")
        {
            keyShortcut = newKey;
        }
        if (toolToChange == "keyPan")
        {
            keyPan = newKey;
        }
        if (toolToChange == "keyRotateLeft")
        {
            keyRotateLeft = newKey;
        }
        if (toolToChange == "keyRotateRight")
        {
            keyRotateRight = newKey;
        }
        if (toolToChange == "keyPaint")
        {
            keyPaint = newKey;
        }
        if (toolToChange == "keyInteract")
        {
            keyInteract = newKey;
        }
        if (toolToChange == "keySwitchTool")
        {
            keySwitchTool = newKey;
        }
        if (toolToChange == "keyHeight")
        {
            keyHeight = newKey;
        }
        if (toolToChange == "keyPing")
        {
            keyPing = newKey;
        }
        if (toolToChange == "keyHide")
        {
            keyHide = newKey;
        }
        if (toolToChange == "keyUndo")
        {
            keyUndo = newKey;
        }
    }

    public string GetKeyCodeToString(string keyName)
    {
        if (keyName == "keyShortcut")
        {
            return keyShortcut.ToString();
        }
        if (keyName == "keyPan")
        {
            return keyPan.ToString();
        }
        if (keyName == "keyRotateLeft")
        {
            return keyRotateLeft.ToString();
        }
        if (keyName == "keyRotateRight")
        {
            return keyRotateRight.ToString();
        }
        if (keyName == "keyPaint")
        {
            return keyPaint.ToString();
        }
        if (keyName == "keyInteract")
        {
            return keyInteract.ToString();
        }
        if (keyName == "keySwitchTool")
        {
            return keySwitchTool.ToString();
        }
        if (keyName == "keyHeight")
        {
            return keyHeight.ToString();
        }
        if (keyName == "keyPing")
        {
            return keyPing.ToString();
        }
        if (keyName == "keyHide")
        {
            return keyHide.ToString();
        }
        if (keyName == "keyUndo")
        {
            return "CTRL + " + keyUndo.ToString();
        }
        return "key not available";
    }

    public KeyCode GetKeyCodeValue(string keyName)
    {
        if(keysDisabled == true)
        {
            return KeyCode.None;
        }
        if (keyName == "keyShortcut")
        {
            return keyShortcut;
        }
        if (keyName == "keyPan")
        {
            return keyPan;
        }
        if (keyName == "keyRotateLeft")
        {
            return keyRotateLeft;
        }
        if (keyName == "keyRotateRight")
        {
            return keyRotateRight;
        }
        if (keyName == "keyPaint")
        {
            return keyPaint;
        }
        if (keyName == "keyInteract")
        {
            return keyInteract;
        }
        if (keyName == "keySwitchTool")
        {
            return keySwitchTool;
        }
        if (keyName == "keyHeight")
        {
            return keyHeight;
        }
        if (keyName == "keyPing")
        {
            return keyPing;
        }
        if (keyName == "keyHide")
        {
            return keyHide;
        }
        if (keyName == "keyUndo")
        {
            return keyUndo;
        }
        return KeyCode.None;
    }

}
