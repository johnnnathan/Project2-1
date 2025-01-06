using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Vision : MonoBehaviour
{
    public float visionRadius = 10f; // Max distance of rays
    public float visionAngle = 120f; // Field of view angle
    public int rayCount = 11; // Number of rays to shoot
    public LayerMask detectableLayers; // Layers to detect
    public int maxObjects = 5; // Maximum number of detected objects

    private List<Vector3> detectedObjects = new List<Vector3>();

    void Update()
    {
        ShootRays();
    }

    private void ShootRays()
    {
        detectedObjects.Clear();

        float angleStep = visionAngle / (rayCount - 1); // Angle between each ray
        float startAngle = -visionAngle / 2; // Start shooting rays from this angle

        for (int i = 0; i < rayCount; i++)
        {
            float currentAngle = startAngle + (i * angleStep);
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * transform.forward;

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, visionRadius, detectableLayers))
            {
                if (!detectedObjects.Contains(hit.point)) // Avoid duplicates
                {
                    detectedObjects.Add(hit.point);

                    if (detectedObjects.Count >= maxObjects)
                        break;
                }
            }

            Debug.DrawRay(transform.position, direction * visionRadius, Color.green); // Visualize the ray
        }
    }

    public List<Vector3> GetDetectedObjects()
    {
        return detectedObjects;
    }

    void OnDrawGizmos()
    {
        // Check if the object is selected in the editor
        if (!Selection.activeGameObject || Selection.activeGameObject != gameObject)
            return; // Exit if the object is not selected

        // Visualize the rays in the field of view
        float angleStep = visionAngle / (rayCount - 1);
        float startAngle = -visionAngle / 2;

        Gizmos.color = Color.green;
        for (int i = 0; i < rayCount; i++)
        {
            float currentAngle = startAngle + (i * angleStep);
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * transform.forward;
            Gizmos.DrawRay(transform.position, direction * visionRadius); // Visualize the ray
        }

        // Draw the field of view arc for visualization
        Gizmos.color = Color.yellow;
        Vector3 leftBoundary = Quaternion.Euler(0, -visionAngle / 2, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, visionAngle / 2, 0) * transform.forward;

        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * visionRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * visionRadius);

        // Draw spheres at detected object positions
        Gizmos.color = Color.red;
        foreach (Vector3 position in detectedObjects)
        {
            Gizmos.DrawSphere(position, 0.5f);
        }
    }
}
