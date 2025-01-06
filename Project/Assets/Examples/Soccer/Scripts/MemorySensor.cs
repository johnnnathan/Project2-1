using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Sensors;

public class MemorySensor : MonoBehaviour
{
    public int memorySize = 10; // Number of observation states stored
    public int obsSize = 15; // Size of memory unit
    public LayerMask detectableLayers; // Layers to detect (e.g., ball, players, etc.)
    private List<List<Vector3>> memories = new List<List<Vector3>>(); // Stored observations
    
    // Initialize each memory with obsSize zeroed Vector3 elements
    void Awake()
    {
        for (int i = 0; i < memorySize; i++)
        {
            List<Vector3> memoryUnit = new List<Vector3>();
            for (int j = 0; j < obsSize; j++)
            {
                memoryUnit.Add(Vector3.zero);
            }
            memories.Add(memoryUnit);
        }
    }
    
    // Remove the last observation and add the current one to the observation list
    public void AddMemory(List<Vector3> obsVector)
{
    // Ensure the obsVector has a consistent size
    List<Vector3> fixedSizeObs = new List<Vector3>(obsSize);

    for (int i = 0; i < obsSize; i++)
    {
        if (i < obsVector.Count)
        {
            fixedSizeObs.Add(obsVector[i]);
        }
        else
        {
            fixedSizeObs.Add(Vector3.zero);
        }
    }

    // Remove the oldest memory and add the new fixed-size memory
    memories.RemoveAt(0);
    memories.Add(fixedSizeObs);
}


    public List<List<Vector3>> GetMemories()
    {
        return memories;
    }
}