using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenMenu : MonoBehaviour
{
    public TokenController tokenController;

    private Color color;

    private void Start()
    {
        color = GameController.instance.GetPlayerColor();
    }

    public void CreateToken()
    {
        tokenController.CreateToken(color);
        GameController.instance.Tooltip("A token has been created.");
    }

    public void OnColorChanged(Color color)
    {
        this.color = color;
    }
}
