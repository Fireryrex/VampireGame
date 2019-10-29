using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Enemy_Or_Object : MonoBehaviour
{
    [SerializeField] GameObject spawnedItem;
    [SerializeField] int numItems;
    [SerializeField] int numSpawnedAtATime = 1;
    private Vector3 Position;
    // Start is called before the first frame update
    private void Start()
    {
        Position = transform.position;
    }

    public void SpawnItem()
    {
        if (gameObject.transform.childCount <= numItems + 1)        //caps number of items that can spawn
        {
            for (int i = 0; i < numSpawnedAtATime; ++i)             //spawns numSpawnedAtATime number of objects at once
            {
                Instantiate(spawnedItem, Position, Quaternion.identity, gameObject.transform);  //spawns the thing
            }
        }
    }
}