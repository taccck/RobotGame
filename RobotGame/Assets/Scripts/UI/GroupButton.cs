using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class GroupButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    //https://www.youtube.com/watch?v=211t6r12XPQ
    //a button within a button group

    public ButtonGroup buttonGroup;
    public Image background;
    public UnityEvent onSelected; //what happens when the button gets selected
    public UnityEvent onDeselected; //what happens when the button gets deselected

    public void OnPointerClick(PointerEventData eventData)
    {
        buttonGroup.OnSelected(this); //eneter active state when clicked
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonGroup.OnEnter(this); //eneter hover state when entered
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonGroup.OnExit(this); //eneter idle state when exited
    }

    void Awake()
    {
        background = GetComponent<Image>();
    }

    public void Select() 
    {
        //invoke selected eventsystem when button gets selected

        if (onSelected != null)
        {
            onSelected.Invoke();
        }
    }

    public void Deselect()
    {
        //invoke deselected eventsystem when button gets deselected

        if (onDeselected != null)
        {
            onDeselected.Invoke();
        }
    }
}