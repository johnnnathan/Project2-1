using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Sensors;

public class MemorySensor : MonoBehaviour
{
    public int memorySize = 10; // Number of observation states stored
    public LayerMask detectableLayers; // Layers to detect (e.g., ball, players, etc.)
    private List<List<Vector3>> memories = new List<List<Vector3>>(); // Stored observations
    
    void Awake()
    {
        for (int i = 0; i < memorySize; i++)
        {
            memories.Add(new List<Vector3> { Vector3.zero }); // Fill each memory with a zeroed Vector3
        }
    }
    
    // Remove the last observation and add the current one to the observation list
    public void AddMemory(List<Vector3> obsVector)
    {
        
        memories.RemoveAt(0);

        memories.Add(obsVector);
    }


    public List<List<Vector3>> GetMemories()
    {
        return memories;
    }
}