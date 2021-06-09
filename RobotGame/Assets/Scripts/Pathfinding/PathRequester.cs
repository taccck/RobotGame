using UnityEngine;

public class PathRequester : MonoBehaviour //keeps track of the path the gameobject is following
{
    public AStar aStar;
    public float minMoveDistance;
    public Vector3[] path { get; private set; }
    int pathIndex = 1;

    public Vector3? GetWalkPoint(Vector3 currPos)
    {
        if (path != null)
        {
            if (!CanMoveTo(Vector3.Distance(currPos, path[path.Length - 1]))) //stooooops because the requesting object is close enough to the end position
            {
                path = null;
                return currPos;
            }

            if (Vector3.Distance(currPos, path[pathIndex - 1]) > Vector3.Distance(path[pathIndex], path[pathIndex - 1])) //if you have moved past the current walk point
            {
                pathIndex++;
            }

            if (pathIndex < path.Length)
            {
                return path[pathIndex];
            }
            else //if path index gets too high
            {
                path = null;
            }
        }
        return null;
    }

    public void SetPath(Vector3 startPos, Vector3 endPos, float r)
    {
        if (CanMoveTo(Vector3.Distance(startPos, endPos)))
        {
            pathIndex = 1;
            path = aStar.FindPath(startPos, endPos, r);
        }
    }

    public bool CanMoveTo(float distance)
    {
        return distance > minMoveDistance;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;

    //    if (path != null)
    //        for (int i = 0; i < path.Length; i++)
    //        {
    //            Gizmos.DrawSphere(path[i], .2f);

    //            if (i - 1 >= 0)
    //            {
    //                Gizmos.DrawLine(path[i], path[i - 1]);
    //            }
    //        }

    //    if (path != null)
    //    {
    //        Gizmos.DrawSphere(path[0], .2f);
    //        Gizmos.DrawSphere(path[path.Length - 2], .2f);
    //    }
    //}
}
