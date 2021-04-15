using UnityEngine;

public class PathRequester : MonoBehaviour //keeps track of the path the gameobject is following
{
    public AStar aStar;
    public float minMoveDistance;
    public Vector3[] path { get; private set; }
    int pathIndex = 1;

    public Vector3 GetWalkPoint(Vector3 currPos)
    {
        if (path != null)
        {
            if (!CanMoveTo(Vector3.Distance(currPos, path[path.Length - 1]))) //stooooops because the requesting object is close enough to the end position
            {
                path = null;
                return currPos;
            }

            if (Vector3.Distance(currPos, path[pathIndex - 1]) > Vector3.Distance(path[pathIndex], path[pathIndex - 1])) //if your past current walk point
            {
                pathIndex++;
            }

            return path[pathIndex];
        }
        return currPos;
    }

    public void SetPath(Vector3 startPos, Vector3 endPos, float r)
    {
        pathIndex = 1;
        path = aStar.FindPath(startPos, endPos, r);
    }

    public bool CanMoveTo(float distance)
    {
        return distance > minMoveDistance;
    }
}
