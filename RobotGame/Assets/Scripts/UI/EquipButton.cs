using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(Image))]
public class EquipButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    //button to equip an item as the held item

    public enum ButtonState { Idle, Hover, Selected }
    public ButtonState currState = ButtonState.Idle;

    public ItemDescription itemDescription; //has the un/equip method since it has a reference to inventory
    public Image image;
    public TextMeshProUGUI text;
    public Color clear;
    public Color hover;
    public Color selected;
    public Color textColor;
    public bool active = false; //if no item is selected, button is uninteractable

    public void ButtonUpdate()
    {
        //updates apperance of button depending on its state

        switch (currState)
        {
            case ButtonState.Idle:
                image.color = clear;
                text.color = textColor;
                text.text = "Equip";
                break;

            case ButtonState.Hover:
                image.color = hover;
                text.color = textColor;
                text.text = "Equip";
                break;

            case ButtonState.Selected:
                image.color = selected;
                text.color = clear;
                text.text = "Unequip";
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //button stays in the selected state when clicked

        if (active)
        {
            if (currState != ButtonState.Selected) //it if isn't selected equip it
            {
                currState = ButtonState.Selected;
                itemDescription.Equip();
            }
            else //unequip it
            {
                currState = ButtonState.Hover;
                itemDescription.Unequip();
            }

            ButtonUpdate();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //set state to hover when entered

        if (active)
        {
            if (currState != ButtonState.Selected)
            {
                currState = ButtonState.Hover;
            }

            ButtonUpdate();
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //set state to idle when exited

        if (active)
        {
            if (currState != ButtonState.Selected)
            {
                currState = ButtonState.Idle;
            }

            ButtonUpdate();
        }
        
    }
}
