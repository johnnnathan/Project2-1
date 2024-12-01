using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Sensors;

public class MemorySensor : MonoBehaviour
{
    public int memorySize = 10; // Number of memories stored
    public LayerMask detectableLayers; // Layers to detect (e.g., ball, players, etc.)
    private List<List<Vector3>> memories = new List<List<Vector3>>();
    
    void Awake()
    {
        for (int i = 0; i < memorySize; i++)
        {
            memories.Add(new List<Vector3> { Vector3.zero }); // Fill each memory with a zeroed Vector3
        }
    }
    
    public void AddMemory(List<Vector3> obsVector)
    {
        if (memories.Count >= memorySize)
        {
            memories.RemoveAt(0);
        }

        memories.Add(obsVector);
    }

    public List<List<Vector3>> GetMemories()
    {
        return memories;
    }
}