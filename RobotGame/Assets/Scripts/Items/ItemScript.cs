using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public string itemName;
    public ItemFunctionality.ItemIds id;
    public ItemFunctionality.ItemType type;
    public IndividualItem individualItem { get; private set; } = null;
    public int amount; 

    public virtual void Start()
    {
        if (individualItem == null)
        {
            SetIndividualItem(new IndividualItem((int)id, amount));
        }
        else
        {
            amount = individualItem.Amount;
        }
    }

    public void SetIndividualItem(IndividualItem _individualItem)
    {
        individualItem = _individualItem;
    }
}
