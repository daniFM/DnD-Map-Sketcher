using UnityEngine;

[System.Serializable]
public class Key
{
    [SerializeField]
    private KeyCode code;
    [SerializeField]
    private string keyName;

    public Key(KeyCode code)
    {
        this.code = code;
    }

    public KeyCode GetKeyCode()
    {
        return code;
    }

    public string KeyName()
    {
        return keyName;
    }

    public void ReassignKey(KeyCode k)
    {
        code = k;
    }

    public bool GetKeyDown()
    {
        return Input.GetKeyDown(code);
    }

    public bool GetKey()
    {
        return Input.GetKey(code);
    }

    public bool GetKeyUp()
    {
        return Input.GetKeyUp(code);
    }
}

[CreateAssetMenu(fileName = "ControlsConfig", menuName = "ScriptableObjects/ControlsConfig", order = 2)]
public class ControlsScriptableObject : ScriptableObject
{
    public Key[] keys;
    public bool keysDisabled = false;


    public void ChangeToolKey(string keyName, KeyCode newKey)
    {
        foreach(Key k in keys)
        {
            if(k.KeyName() == keyName)
            {
                k.ReassignKey(newKey);
            }
        }
    }

    public string GetKeyName(int arrIndex)
    {
        return keys[arrIndex].KeyName();
    }

    public bool DetectKeyPress(int arrIndex)
    {
        return keysDisabled ? false : keys[arrIndex].GetKeyDown();
    }

    public bool DetectKeyHeld(int arrIndex)
    {
        return keysDisabled ? false : keys[arrIndex].GetKey();
    }

    public bool DetectKeyUp(int arrIndex)
    {
        return keysDisabled ? false : keys[arrIndex].GetKeyUp();
    }
}
