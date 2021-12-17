using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    //controls how often enemies spawn
    public float spawnRate = 5;
    //controls how many enemies are spawned at once
    public float enemiesSpawned = 1;
    //controls the type of enemy spawned
    public GameObject EnemyType;
    //controls if the spawner is actively spawning
    public bool spawning = true;
    //controls how mnay enemies the spawner can spawn before stopping. -1 means infinite
    public int maxSpawned = -1;
    
    private float timer = 0;

    // Update is called once per frame
    void Update()
    {
        if(spawning)
        {
            timer += Time.deltaTime;
            if(timer >= spawnRate)
            {
                for(int i = 0; i < enemiesSpawned; i++)
                {
                    Instantiate(EnemyType, transform.position, Quaternion.identity);
                }
                if(maxSpawned != -1)
                {
                    maxSpawned--;
                    if(maxSpawned == 0) 
                    {
                        Destroy(this);
                    }
                }
                timer = 0;
            }
        }   
    }
}
