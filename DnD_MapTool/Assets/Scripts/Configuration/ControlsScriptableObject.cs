// Copyright (c) Daniel Fernández 2022


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    [SerializeField] private ControlAction action;
    [SerializeField] private KeyCode mainKey;
    [SerializeField] private KeyCode shiftKey;  // ATM this is always pre-configured

    public Control(ControlAction action)
    {
        this.action = action;
    }

    public ControlAction GetAction()
    {
        return action;
    }

    public KeyCode GetMainKey()
    {
        return mainKey;
    }

    public KeyCode GetShiftKey()
    {
        return shiftKey;
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
        return shiftKey == KeyCode.None ? Input.GetKeyDown(mainKey) : Input.GetKeyDown(mainKey) && Input.GetKey(shiftKey);
    }

    public bool GetKey()
    {
        return shiftKey == KeyCode.None ? Input.GetKey(mainKey) : Input.GetKey(mainKey) && Input.GetKey(shiftKey);
    }

    public bool GetKeyUp()
    {
        return shiftKey == KeyCode.None ? Input.GetKeyUp(mainKey) : Input.GetKeyUp(mainKey) && Input.GetKey(shiftKey);
    }
}

[CreateAssetMenu(fileName = "ControlsConfig", menuName = "ScriptableObjects/ControlsConfig", order = 2)]
public class ControlsScriptableObject : ScriptableObject
{
    [HideInInspector] public bool keysDisabled;

    [SerializeField] private Control[] controlsConfig;
    private Dictionary<ControlAction, Control> controlsLookup;

    public void Instantiate()
    {
        UpdateLookup();
        keysDisabled = false;
    }

    public void RemapControl(ControlAction action, KeyCode newKey)
    {
        controlsLookup[action].ReassignKey(newKey);
        UpdateConfig();
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

    private void UpdateConfig()
    {
        controlsConfig = new Control[controlsLookup.Count];

        for(int i = 0; i < controlsConfig.Length; ++i)
        {
            controlsConfig[i] = controlsLookup.Values.ElementAt(i);
        }
    }

    public ControlAction[] GetActions()
    {
        return controlsLookup.Keys.ToArray();
    }

    public KeyCode[] GetActionKeys()
    {
        KeyCode[] codes = new KeyCode[controlsConfig.Length];

        for(int i = 0; i < codes.Length; ++i)
        {
            codes[i] = controlsConfig[i].GetMainKey();
        }

        return codes;
    }

    public string[] GetKeyNames()
    {
        string[] names = new string[controlsConfig.Length];
        StringBuilder sb = new StringBuilder();

        int i = 0;
        foreach(Control control in controlsConfig)
        {
            names[i] = GetKeyName(control.GetAction(), sb);
            i++;
        }

        return names;
    }

    public string GetKeyName(ControlAction action, StringBuilder sb = null)
    {
        if(sb == null)
            sb = new StringBuilder();

        Control control = controlsLookup[action];
        sb.Clear();
        KeyCode shiftKey = control.GetShiftKey();
        if(shiftKey != KeyCode.None)
        {
            sb.Append(shiftKey).Append(" + ");
        }
        sb.Append(control.GetMainKey());

        return sb.ToString();
    }

    public Control GetControl(ControlAction action)
    {
        return controlsLookup[action];
    }

    #if UNITY_EDITOR

    public void GenerateEmptyControls()
    {
        ControlAction[] controls = Enum.GetValues(typeof(ControlAction)).Cast<ControlAction>().ToArray();
        controlsConfig = new Control[controls.Length];

        for(int i = 0; i < controls.Length; ++i)
        {
            controlsConfig[i] = new Control(controls[i]);
        }
    }

    public void UpdateConfigEditor()
    {
        ControlAction[] controls = Enum.GetValues(typeof(ControlAction)).Cast<ControlAction>().ToArray();
        
        Control[] newConfig = new Control[controls.Length];

        for(int i = 0; i < controls.Length; ++i)
        {
            Control control = Array.Find(controlsConfig, c => c.GetAction() == controls[i]);
            if(control != null)
            {
                newConfig[i] = control;
            }
            else
            {
                newConfig[i] = new Control(controls[i]);
            }
        }

        controlsConfig = newConfig;
    }

    #endif
}
