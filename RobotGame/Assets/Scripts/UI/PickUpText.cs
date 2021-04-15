using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]

public class PickUpText : MonoBehaviour 
{
    //sets the tmps text to match the tripods players intventorys pickuptext string

    Inventory inventory;
    TextMeshProUGUI text;
    public CameraMomement cameraMomement;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void FixedUpdate()
    {
        if (inventory == null) //get inventory from tripod
        {
            inventory = cameraMomement.player.GetComponent<Inventory>();
        }

        text.text = inventory.pickUpTxt;
    }
}
