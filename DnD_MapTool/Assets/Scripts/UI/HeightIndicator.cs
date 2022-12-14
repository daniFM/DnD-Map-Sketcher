// Copyright (c) 2022 Daniel Fernández Marqués
// Licensed under the GNU General Public License (GPL) version 3. See the LICENSE file for more details.

using UnityEngine;
using UnityEngine.UI;

public class HeightIndicator : MonoBehaviour
{
    private Text text;
    private const string label = "Height: ";

    private void OnEnable()
    {
        CameraMovement.HeightChanged += OnHeightChanged;
    }

    private void OnDisable()
    {
        CameraMovement.HeightChanged -= OnHeightChanged;
    }

    private void Start()
    {
        text = GetComponent<Text>();
    }

    private void OnHeightChanged(int level)
    {
        text.text = label + level;
    }
}
