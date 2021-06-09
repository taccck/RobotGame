using System;

public class SortedNodeSet
{
    public Node[] Nodes { get; private set; }

    public SortedNodeSet()
    {
        Nodes = new Node[0];
    }

    public int Count
    {
        get
        {
            return Nodes.Length;
        }
    }

    public void Add(Node node)
    {
        for (int i = 0; i < Count; i++) //don't add the node if it already is in the array
        {
            if (Nodes[i] == node)
            {
                if (i - 1 >= 0)
                {
                    if (node.GetfCost() < Nodes[i - 1].GetfCost()) //if it should move up with its new lower value
                    {
                        Remove(node);
                    }
                    else return;
                }
                else return;
            }
        }

        Node[] newArray = new Node[Count + 1];
        bool added = false;
        for (int i = 0; i < newArray.Length; i++)
        {
            if (!added)
            {
                if (i >= Count)
                {
                    newArray[i] = node;
                    added = true;
                    continue;
                }
                else if (node.GetfCost() < Nodes[i].GetfCost())
                {
                    newArray[i] = node;
                    added = true;
                    continue;
                }
            }

            if (!added)
                newArray[i] = Nodes[i];
            else
                newArray[i] = Nodes[i - 1];
        }

        Nodes = newArray;
    }

    public void Remove(int index)
    {
        if (index < 0 || index > Count - 1)
        {
            throw new IndexOutOfRangeException();
        }

        Node[] newArray = new Node[Count - 1];
        bool removed = false;

        for (int i = 0; i < Count; i++)
        {
            if (!removed)
            {
                if (i == index)
                {
                    removed = true;
                    continue;
                }
            }

            if (!removed)
                newArray[i] = Nodes[i];
            else
                newArray[i - 1] = Nodes[i];
        }

        Nodes = newArray;
    }

    public void Remove(Node node)
    {
        for (int i = 0; i < Count; i++)
        {
            if (Nodes[i] == node)
            {
                Remove(i);
                break;
            }
        }
    }
}
