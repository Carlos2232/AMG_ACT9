using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public Item itemToSpawn;
    public float noteSpeed = 6f; // Scrolling speed downward
    public List<Item> items = new List<Item>();
    public Transform parent;

    [Header("Lane Settings (Left, Middle, Right)")]
    [SerializeField] private float[] laneXPositions = new float[] { -2.5f, 0f, 2.5f };
    [SerializeField] private float spawnY = 8f; // Top of the screen

    private void Start()
    {
        // Spawns a new note/obstacle every 1 second (adjust timing to match song/difficulty)
        InvokeRepeating(nameof(BeginSpawn), 1f, 1f);
    }

    private void BeginSpawn()
    {
        var spawnedItem = Instantiate(itemToSpawn, parent);

        // Pick a random lane (0 = Left, 1 = Middle, 2 = Right)
        int randomLane = Random.Range(0, laneXPositions.Length);
        spawnedItem.assignedLane = randomLane;
        spawnedItem.speed = noteSpeed;

        // Position at the top of the screen in the chosen lane
        spawnedItem.transform.position = new Vector3(laneXPositions[randomLane], spawnY, 0f);
        items.Add(spawnedItem);
    }

    private void Update()
    {
        // Clean up destroyed items from the list
        for (int i = items.Count - 1; i >= 0; i--)
        {
            if (items[i] == null)
            {
                items.RemoveAt(i);
            }
        }
    }
}