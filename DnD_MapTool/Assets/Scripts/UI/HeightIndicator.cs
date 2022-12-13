// Copyright (c) Daniel Fernández 2022


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
