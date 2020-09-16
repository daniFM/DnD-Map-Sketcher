using System.Collections.Generic;
using UnityEngine;

public enum ControlAction
{
    Controls = 0,
    Pan = 1,
    MoveUpDown = 2,
    MoveUp = 3,
    MoveDown = 4,
    RotateRight = 5,
    RotateLeft = 6,
    Zoom = 7,
    Paint = 8,
    Interact = 9,
    CycleTool = 10,
    RotateTile = 11,
    RotateTileRight = 12,
    RotateTileLeft = 13,
    HeightAid = 14,
    Ping = 15,
    PlaceToken = 16,
    HideUI = 17,
    Undo = 18
}

[System.Serializable]
public class Control
{
    [SerializeField]
    private ControlAction action;
    [SerializeField]
    private KeyCode mainKey;
    [SerializeField]
    private KeyCode shiftKey;

    //public Control(ControlAction code)
    //{
    //    this.code = code;
    //}

    public ControlAction GetAction()
    {
        return action;
    }

    public KeyCode GetMainKey()
    {
        return mainKey;
    }

    public string KeyName()
    {
        return mainKey.ToString();
    }

    public void ReassignKey(KeyCode k)
    {
        mainKey = k;
    }

    public bool GetKeyDown()
    {
        return shiftKey == KeyCode.None ? Input.GetKeyDown(mainKey) : Input.GetKeyDown(mainKey) && Input.GetKeyDown(shiftKey);
    }

    public bool GetKey()
    {
        return shiftKey == KeyCode.None ? Input.GetKey(mainKey) : Input.GetKey(mainKey) && Input.GetKey(shiftKey);
    }

    public bool GetKeyUp()
    {
        return shiftKey == KeyCode.None ? Input.GetKeyUp(mainKey) : Input.GetKeyUp(mainKey) && Input.GetKeyUp(shiftKey);
    }
}

[CreateAssetMenu(fileName = "ControlsConfig", menuName = "ScriptableObjects/ControlsConfig", order = 2)]
public class ControlsScriptableObject : ScriptableObject
{
    [SerializeField] private Control[] controlsConfig;
    public bool keysDisabled = false;

    private Dictionary<ControlAction, Control> controlsLookup;

    void Start()
    {
        UpdateLookup();
    }

    public void ChangeToolKey(string keyName, KeyCode newKey)
    {
        foreach(Control k in controlsConfig)
        {
            if(k.KeyName() == keyName)
            {
                k.ReassignKey(newKey);
            }
        }

        UpdateLookup();
    }

    public bool GetKeyDown(ControlAction arrIndex)
    {
        return keysDisabled ? false : controlsLookup[arrIndex].GetKeyDown();
    }

    public bool GetKey(ControlAction arrIndex)
    {
        return keysDisabled ? false : controlsLookup[arrIndex].GetKey();
    }

    public bool GetKeyUp(ControlAction arrIndex)
    {
        return keysDisabled ? false : controlsLookup[arrIndex].GetKeyUp();
    }

    private void UpdateLookup()
    {
        controlsLookup = new Dictionary<ControlAction, Control>();

        for(int i = 0; i < controlsConfig.Length; ++i)
        {
            Control control = controlsConfig[i];
            controlsLookup[control.GetAction()] = control;
        }
    }
}
