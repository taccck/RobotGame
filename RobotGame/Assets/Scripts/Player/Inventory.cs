using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public IndividualItem Hand { get; private set; } //held item
    public GameObject SceneHand { get; private set; } //gameobject representation of the hand

    public List<IndividualItem> Items { get; set; } = new List<IndividualItem>(); //all items in inventory
    public ItemFunctionality itemFunc;

    private void Start()
    {
        genericInputs = GetComponent<PlayerMovement>().tripod.GetComponent<GenericInputs>();
        genericInputs.spawnItemIcons.inventory = this;
        genericInputs.itemDescription.inventory = this;
    }

    private void Update()
    {
        PickUpInput();

        if (Input.GetButtonDown("OpenInventory"))
        {
            OpenInventory();
        }
    }

    private void FixedUpdate()
    {
        PickUpCheck();
    }

    void AddItem(IndividualItem itemToAdd)
    {
        //adds an item to inventory

        bool added = false;
        foreach (IndividualItem item in Items) //check if an item with the same id exists in items
        {
            if (itemToAdd.Id == item.Id) //if it does, add its amount to the already existing one instead of adding it to the list
            {
                item.AddAmount(itemToAdd.Amount);
                added = true;
                break;
            }
        }
        if (!added)
        {
            Items.Add(itemToAdd);
        }
    }

    public Vector3 handSpawnPos;
    public void InstantiateHand()
    {
        //instantates a scene representation of the hand

        if (SceneHand) //if a scene hand already exists destroy it
        {
            Destroy(SceneHand);
        }

        if (Hand != null)
        {
            GameObject handPrefab = itemFunc.GetItemPrefab((ItemFunctionality.ItemIds)Hand.Id);
            SceneHand = itemFunc.SpawnItem(handPrefab, handSpawnPos, Quaternion.identity, transform, Hand);
        }
    }

    public string pickUpTxt { get; private set; }
    public float pickUpRange;
    public LayerMask dropMask;
    Collider[] pickUps;
    void PickUpCheck()
    {
        pickUpTxt = ""; //clear pickup text
        pickUps = Physics.OverlapSphere(transform.position, pickUpRange, dropMask); //check for items in sourrounding area
        if (pickUps.Length > 0)
        {
            foreach (Collider c in pickUps) //display a list of all pickupable items
            {
                ItemScript itemScript = c.GetComponent<ItemScript>();
                pickUpTxt += itemScript.itemName + " " + itemScript.individualItem.Amount + "x" + "\n";
            }
        }
    }

    public void PickUpInput()
    {
        if (pickUps.Length > 0)
        {
            if (Input.GetButtonDown("Action")) //pick up the first item in the list
            {
                AddItem(pickUps[0].gameObject.GetComponent<ItemScript>().individualItem);
                Destroy(pickUps[0].gameObject);
            }
        }
    }

    public void SetHand(IndividualItem i)
    {
        Hand = i;
        InstantiateHand();
    }

    public int HandIndex()
    {
        //return hands index in the items list

        if (Hand != null)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i] == Hand)
                {
                    return i;
                }
            }
        }

        return -1; //-1 means empty
    }

    bool open;
    GenericInputs genericInputs;
    public void OpenInventory()
    {
        //shows and hides the inventory 

        if (Input.GetButtonDown("OpenInventory"))
        {
            open = !open;

            if (open)
            {
                genericInputs.inventoryUI.SetActive(true);
                genericInputs.buttonGroup.OnActivate(); //makes sure the all tab is selected

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                genericInputs.cameraMomement.canLook = false;
                genericInputs.PlayerMovementVar.canMove = false;
            }
            else
            {
                genericInputs.spawnItemIcons.RemoveAll();
                genericInputs.inventoryUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                genericInputs.cameraMomement.canLook = true;
                genericInputs.PlayerMovementVar.canMove = true;
            }
        }
    }
}
