using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointTracker : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;

    public IReadOnlyList<Transform> Waypoints => waypoints;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        foreach (Transform point in waypoints)
        {
            Gizmos.DrawWireSphere(point.position, 0.5f);
        }
    }
}
