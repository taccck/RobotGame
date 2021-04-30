using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    Node[] grid; //all nodes 
    public LayerMask neighbourDetectionMaks;
    public LayerMask obstacleMask;

    void Start()
    {
        grid = GetComponentsInChildren<Node>(); //add all nodes

        Stack<Node> neighbourGrid = new Stack<Node>(grid); //shouldn't consider a node as a nighbour twice
        neighbourGrid = ReverseStack(neighbourGrid); //pop start from top, foreach start from bottom, needs to be reversed so they'll match
        foreach (Node n in grid)
        {
            neighbourGrid.Pop(); //don't consider this one as a neightbour
            n.SetNeighbours(neighbourGrid.ToArray(), neighbourDetectionMaks); //set all neighbours
        }
    }

    public Vector3[] FindPath(Vector3 startPos, Vector3 endPos, float r) //returns a walkable path between start and end
    {
        if (!ObstacleRaycast(startPos, endPos)) //no path needs to be found if there is no obstacle between start and end
        {
            return new Vector3[] { startPos, endPos };
        }

        Node currNode = null;
        float closestNode = Mathf.Infinity;
        foreach (Node n in hit.transform.parent.GetComponentsInChildren<Node>()) //set current node to the closest node from the object blocking the raycast
        {
            float currDistance = Vector3.Distance(startPos, n.transform.position);
            if (currDistance < closestNode)
            {
                currNode = n;
                closestNode = currDistance;
            }
        }

        if (!currNode) //the object blocking the raycast did not have nodes
        {
            Debug.LogError("Object did not have nodes, add them or remove from obstacle layer");
            return null; 
        }

        Node startNode = currNode;
        Vector3 currPos = currNode.transform.position;
        HashSet<Node> openSet = new HashSet<Node>(); //all nodes to consider as current node
        HashSet<Node> closedSet = new HashSet<Node>(); //all nodes that have been the current node
        while (true)
        {
            if (!ObstacleRaycast(currNode.GetPostionWithOffset(r), endPos)) //if goal is reachable from this node
            {
                break;
            }

            foreach (Node n in currNode.Neighbours) //add neighbours to open set and set their cost and parent
            {
                if (!closedSet.Contains(n))
                {
                    openSet.Add(n);
                    n.HCost = Vector3.Distance(n.transform.position, endPos);
                    n.GCost = currNode.GCost + Vector3.Distance(currPos, n.transform.position);
                    n.Parent = currNode;
                }
            }

            Node candidateForCurr = null;
            foreach (Node n in openSet) //find next current node
            {
                if (!candidateForCurr)
                {
                    candidateForCurr = n;
                    continue;
                }

                if (n.GetfCost() < candidateForCurr.GetfCost())
                {
                    candidateForCurr = n;
                }
            }
            currNode = candidateForCurr;

            if (!currNode)
            {
                Debug.LogWarning("Path can't be found: dead end");
                return null;
            }

            currPos = currNode.transform.position;
            closedSet.Add(currNode);
            openSet.Remove(currNode);
        }

        List<Vector3> path = new List<Vector3>();
        path.Add(endPos);
        while (currNode != startNode) //retrace steps to get path
        {
            currPos = currNode.GetPostionWithOffset(r);
            path.Add(currPos);
            currNode = currNode.Parent;
        }
        path.Add(startPos);

        //reverse
        path.Reverse();
        return path.ToArray();
    }

    RaycastHit hit;
    bool ObstacleRaycast(Vector3 start, Vector3 end)
    {
        return Physics.Raycast(start, end - start, out hit, Vector3.Distance(start, end), obstacleMask, QueryTriggerInteraction.Collide);
    }

    // https://www.csharp-console-examples.com/collection/reverse-a-stack-array-in-c/
    public Stack<Node> ReverseStack(Stack<Node> input)
    {
        Stack<Node> temp = new Stack<Node>();

        while (input.Count != 0)
            temp.Push(input.Pop());

        return temp;
    }
}
