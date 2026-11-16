using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public Item itemToSpawn;
    public float noteSpeed = 6f; 
    public List<Item> items = new List<Item>();
    public Transform parent;

    [Header("Lane Settings (Left, Middle, Right)")]
    [SerializeField] private float[] laneXPositions = new float[] { -3.5f, 0f, 3.5f };
    [SerializeField] private float spawnY = 8f; 

    private void Start()
    {
        InvokeRepeating(nameof(BeginSpawn), 1f, 1f);
    }

    private void BeginSpawn()
    {
        var spawnedItem = Instantiate(itemToSpawn, parent);

        int randomLane = Random.Range(0, laneXPositions.Length);
        spawnedItem.assignedLane = randomLane;
        spawnedItem.speed = noteSpeed;

        spawnedItem.transform.position = new Vector3(laneXPositions[randomLane], spawnY, 0f);
        items.Add(spawnedItem);
    }

    private void Update()
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            if (items[i] == null)
            {
                items.RemoveAt(i);
            }
        }
    }
}