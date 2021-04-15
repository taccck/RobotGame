using System;

[Serializable]
public class IndividualItem
{
    public int Id;
    public int Amount;
    public ItemScript ItemScript { get; private set; } //reference to items scene instance

    public IndividualItem(int _id, int _amount)
    {
        Id = _id;
        Amount = _amount;
    }

    public void AddAmount(int toAdd)
    {
        Amount += toAdd;
    }

    public void SetItemScript(ItemScript _itemScript)
    {
        ItemScript = _itemScript;
    }
}
