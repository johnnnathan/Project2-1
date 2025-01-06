using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Sensors;

public class HearingSensor : MonoBehaviour
{
    public float hearingRadius = 50f; // Radius within which the agent "hears"
    public LayerMask detectableLayers; // Layers to detect
    public int maxObjects = 5; // Maximum number of objects to track
    public float minMovementThreshold = 0.1f; // Speed threshold to be registered

    private List<Vector3> objectPositions = new List<Vector3>();

    public void CollectNearbyObjects()
    {
        objectPositions.Clear();

        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, hearingRadius, detectableLayers);

        foreach (Collider obj in nearbyObjects)
        {
            if (objectPositions.Count >= maxObjects)
                break;

            Rigidbody rb = obj.GetComponent<Rigidbody>();

            // Check if the object has a Rigidbody and is moving
            if (rb != null && rb.velocity.magnitude > minMovementThreshold)
            {
                objectPositions.Add(obj.transform.position);
            }
        }
    }

    public List<Vector3> GetObjectPositions()
    {
        return objectPositions;
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the hearing radius
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, hearingRadius);

        // Visualize observed objects
        Gizmos.color = Color.red;
        foreach (Vector3 position in objectPositions)
        {
            Gizmos.DrawSphere(position, 1.0f); // Draw small spheres at each observed object's position
        }
    }
}