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

        Stack<Node> neighbourGrid = new Stack<Node>(grid); //use a stack to not consider a node as a nighbour twice
        neighbourGrid = ReverseStack(neighbourGrid); //pop starts from top, foreach starts from bottom, needs to be reversed so they'll match
        foreach (Node n in grid)
        {
            neighbourGrid.Pop(); //don't consider this one as a neightbour
            n.SetNeighbours(neighbourGrid.ToArray(), neighbourDetectionMaks); //set all neighbours
        }
    }

    public Vector3[] FindPath(Vector3 startPos, Vector3 endPos, float r) //returns a walkable path between start and end
    {
        Node start = FindNodeBetween(startPos, endPos, r);
        Node end = FindNodeBetween(endPos, startPos, r);

        if (start == null || end == null) //no path needs to be found if there is no obstacle between start and end
        {
            return new Vector3[] { startPos, endPos };
        }

        start.GCost = 0;
        Node currNode = start;
        Vector3 currPos = currNode.transform.position;
        SortedNodeSet openSet = new SortedNodeSet(); //all nodes to consider as current node
        List<Node> closedSet = new List<Node>(); //all nodes that have been the current node
        while (currNode != end)
        {
            foreach (Node n in currNode.Neighbours) //add neighbours to open set and set their cost and parent
            {
                if (!closedSet.Contains(n))
                {
                    float hCost = Vector3.Distance(n.transform.position, endPos);
                    float gCost = currNode.GCost + Vector3.Distance(currPos, n.transform.position);

                    if (hCost + gCost < n.GetfCost())
                    {
                        n.HCost = hCost;
                        n.GCost = gCost;
                        n.Parent = currNode;
                        openSet.Add(n);
                    }
                }
            }

            if (openSet.Count == 0)
            {
                Debug.LogWarning("Path can't be found: dead end");
                return null;
            }

            currNode = openSet.Nodes[0];

            currPos = currNode.transform.position;
            closedSet.Add(currNode);
            openSet.Remove(currNode);
        }

        foreach (Node n in grid) //reset g and h cost
        {
            n.GCost = Mathf.Infinity;
            n.HCost = Mathf.Infinity;
        }

        return RetraceSteps(currNode, start, startPos, endPos, r);
    }

    Node FindNodeBetween(Vector3 startPos, Vector3 endPos, float r)
    {
        Node node;

        (RaycastHit raycastHit, bool hit) hit = ObstacleRaycast(new Vector3(startPos.x, startPos.y + 100, startPos.z), startPos, r);
        if (hit.hit)
        {
            node = FindClosestNode(hit.raycastHit.transform.parent.GetComponentsInChildren<Node>(), startPos, r);
        }
        else
        {
            hit = ObstacleRaycast(startPos, endPos, r);

            if (!hit.hit)
                return null; //no obstacle between start and end pos
            else
                node = FindClosestNode(hit.raycastHit.transform.parent.GetComponentsInChildren<Node>(), startPos, r);
        }
        //print(hit.raycastHit.transform.parent.name);

        return node;
    }

    Node FindClosestNode(Node[] nodes, Vector3 position, float r)
    {
        Node node = null;
        float closestNode = Mathf.Infinity;
        foreach (Node n in nodes) //set current node to the closest node from the object blocking the raycast
        {
            float currDistance = Vector3.Distance(position, n.transform.position);
            if (currDistance < closestNode)
            {
                node = n;
                closestNode = currDistance;
            }
        }

        if (node == null) //the object blocking the raycast did not have nodes
        {
            Debug.LogError("Object did not have nodes, add them or remove from obstacle layer");
            return null;
        }

        return node;
    }

    (RaycastHit raycastHit, bool hit) ObstacleRaycast(Vector3 start, Vector3 end, float r)
    {
        bool hit = Physics.SphereCast(start, r, end - start, out RaycastHit raycastHit, Vector3.Distance(start, end) + .2f, obstacleMask, QueryTriggerInteraction.Collide);
        return (raycastHit, hit);
    }

    Vector3[] RetraceSteps(Node currNode, Node lastNode, Vector3 startPos, Vector3 endPos, float r)
    {
        List<Vector3> path = new List<Vector3>();
        Vector3 currPos;
        path.Add(endPos);
        while (currNode != lastNode)
        {
            currPos = currNode.GetPostionWithOffset(r);
            path.Add(currPos);
            currNode = currNode.Parent;
        }
        path.Add(lastNode.GetPostionWithOffset(r));
        path.Add(startPos);

        path.Reverse();
        return path.ToArray();
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