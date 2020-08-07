using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ToolType type;
    public bool toggle;
    //public UnityEvent eventActivate;
    //public UnityEvent eventDeactivate;
    public KeyCode shortcutKey;

    [HideInInspector] public Button button;
    [HideInInspector] public Text text;
    [SerializeField] private ToolButton[] incompatibleTools;

    protected virtual void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Execute);

        text = GetComponentInChildren<Text>();
        text.gameObject.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(shortcutKey))
        {
            //eventActivate.Invoke();
            Execute();
        }
    }

    public virtual void Activate()
    {
        button.interactable = true;
    }

    public void Execute()
    {
        GameController.instance.SetTool(type);

        if(!toggle)
        {
            button.interactable = false;
        }

        foreach(ToolButton incompatibleTool in incompatibleTools)
        {
            //eventDeactivate.Invoke();
            incompatibleTool.Activate();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.gameObject.SetActive(false);
    }
}
