using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public float GCost { get; set; } = Mathf.Infinity; //the cost of moving to this node form the start
    public float HCost { get; set; } = Mathf.Infinity; //the cost of moving to the goal from this node
    public float GetfCost() //the number that is compared against when checking what node to move to
    {
        return HCost + GCost;
    }

    public Node Parent { get; set; }  //the node that comes before it in a path, needed for backtracking 

    public List<Node> Neighbours { get; private set; } = new List<Node>(); //nodes that can be moved to from this node
    public void SetNeighbours(Node[] grid, LayerMask neighbourDetectionMaks)
    {
        //find nodes that you can move to from this node
        //could work large scale by limiting raycast by distance between nodes
        //would work best with auto generated terrain, grids are beeter for manual since you can serialize the nodes
        foreach (Node n in grid)
        {
            if (n == this) //skip self
            {
                continue;
            }

            if (!Physics.Raycast(transform.position, n.transform.position - transform.position, Vector3.Distance(transform.position, n.transform.position), neighbourDetectionMaks)) //if not blocked by obstacles or ground, it's a neighbour
            {
                Neighbours.Add(n);
                n.Neighbours.Add(this);
            }
        }
    }

    public Ray Ray { get; private set; } //ray from parent to position
    public Vector3 RelativPos { get; private set; }
    private void Start()
    {
        Vector3 originPos = transform.parent.position;
        RelativPos = transform.position - originPos;
        Ray = new Ray(originPos, RelativPos);
    }

    public Vector3 GetPostionWithOffset(float offset)
    {
        //lets gameobjects to move to this node with an offset to not get stuck on obstacles

        float distanceFromOrigin = Vector3.Distance(Vector3.zero, RelativPos);
        return Ray.GetPoint(distanceFromOrigin + offset);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(transform.position, .2f);

    //    foreach (Node n in Neighbours)
    //        Gizmos.DrawLine(transform.position, n.transform.position);
    //}
}