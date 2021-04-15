using TMPro;

public class IconScript : ItemScript
{
    TextMeshProUGUI tmp;
    public string description;

    public override void Start()
    {
        base.Start();
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = itemName + " " + amount + "x";
    }
}
