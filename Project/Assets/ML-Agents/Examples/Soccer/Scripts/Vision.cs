using UnityEngine;

public class Vision : MonoBehaviour
{


    // Rotation speed for the vision sphere
    public float rotationSpeed = 30f;

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
