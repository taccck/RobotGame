using UnityEngine;

public class EnemyBounds : MonoBehaviour
{
    public Vector3 Position { get; private set; }
    public Vector3 bounds; //size of the areaa enemies can be in

    void Awake()
    {
        Position = transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, bounds);
    }
}
