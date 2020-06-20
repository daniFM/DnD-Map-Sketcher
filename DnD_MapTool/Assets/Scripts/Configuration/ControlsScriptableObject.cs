using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ControlsConfig", menuName = "ScriptableObjects/ControlsConfig", order = 2)]
public class ControlsScriptableObject: ScriptableObject
{
    public KeyCode keyCTRL = KeyCode.LeftControl;
    public KeyCode keyZ = KeyCode.Z;
}
