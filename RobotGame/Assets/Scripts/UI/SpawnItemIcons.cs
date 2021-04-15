using UnityEngine;

public class SpawnItemIcons : MonoBehaviour
{
    //instantiates ui item gameobjects, icons, that represents the content of inventory

    public ItemFunctionality itemFunctionality;
    public Inventory inventory; //set when opening inventory
    public ButtonGroup buttonGroup; //keeps track of all icons
    public ItemDescription itemDescription;

    public void Spawn(int filter)
    {
        //instantiates all the icons that represent an inventory slot

        bool foundHand = false; //if any item in inventory is being held
        foreach (IndividualItem i in inventory.Items)
        {
            GameObject prefabIcon = itemFunctionality.GetIconItemPrefab((ItemFunctionality.ItemIds)i.Id);
            if ((int)prefabIcon.GetComponent<ItemScript>().type == filter || (int)ItemFunctionality.ItemType.all == filter) //filters for items of specified type
            {
                GameObject newIcon = Instantiate(prefabIcon, gameObject.transform);
                newIcon.GetComponent<ItemScript>().SetIndividualItem(i); //set icons individual item reference, individual items do not have references to icons

                GroupButton button = newIcon.GetComponent<GroupButton>(); //make the icon apart of the button group
                buttonGroup.buttons.Add(button);
                button.buttonGroup = buttonGroup;
                button.onSelected.AddListener(itemDescription.UpdateDescription);

                //find hand and set it to the selected button in button group
                if (i == inventory.Hand)
                {
                    buttonGroup.OnSelected(button);
                    foundHand = true;
                }
            }
        }

        //if nothing is held, nothing will be selected when icons instantiate
        if (!foundHand)
        {
            buttonGroup.selectedButton = null;
            itemDescription.UpdateDescription();
        }
    }

    public void RemoveAll()
    {
        //remove all icons
        foreach (Transform t in gameObject.GetComponentInChildren<Transform>())
        {
            buttonGroup.buttons.Clear();
            Destroy(t.gameObject);
        }
    }
}
