using System;

[Serializable]
public class SaveFile
{
    //contains all variables that will be serialized and stored between playtimes 
    public IndividualItem[] items;
    public int handIndex;

    public SaveFile(IndividualItem[] _items, int _handIndex)
    {
        items = _items;
        handIndex = _handIndex;
    }
}
