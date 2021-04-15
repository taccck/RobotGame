using UnityEngine;
using TMPro;

public class ItemDescription : MonoBehaviour
{
    //logic for description window in inventory ui

    public ButtonGroup itemIcons;
    public Inventory inventory; //set when opening inventory
    public EquipButton equipButton;
    public TextMeshProUGUI descName;
    public TextMeshProUGUI descDesc;

    public void UpdateDescription()
    {
        //sets the apperance of the window to match the selected item

        if (itemIcons.selectedButton) 
        {
            equipButton.active = true;
            IconScript itemScript = itemIcons.selectedButton.GetComponent<IconScript>();

            if (itemScript.individualItem == inventory.Hand) //if selected item is held the equip button is already pressed
            {
                equipButton.currState = EquipButton.ButtonState.Selected;
            }
            else
            {
                equipButton.currState = EquipButton.ButtonState.Idle;
            }

            descName.text = itemScript.itemName;
            descDesc.text = itemScript.description.Replace("<br>","\n");

            equipButton.ButtonUpdate();
        }
        else //if nothing is selected make the window empty and the equip button unclickable
        {
            equipButton.active = false;
            equipButton.currState = EquipButton.ButtonState.Hover;

            descName.text = "";
            descDesc.text = "";

            equipButton.ButtonUpdate();
        }
    }

    public void Equip()
    {
        //equip the button groups selected item, called from equip button
        inventory.SetHand(itemIcons.selectedButton.GetComponent<ItemScript>().individualItem);
    }

    public void Unequip()
    {
        //unequip the button groups selected item, called from equip button
        inventory.SetHand(null);
    }
}
