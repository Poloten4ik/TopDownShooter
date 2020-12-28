using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [HideInInspector]
    public int wayPointIndex = 0;
    public float spawnDuration;
    public int numberOfZombies;
    public GameObject zombiePrefab;
    public Transform[] wayPoints;

    void Start()
    {
        StartCoroutine(ZombieSpawn());
    }

    private IEnumerator ZombieSpawn()
    {
        int countOfZombies = 0;
        while (numberOfZombies > countOfZombies)
        {
            yield return new WaitForSeconds(spawnDuration);
            Instantiate(zombiePrefab, transform.position, Quaternion.identity);
            countOfZombies += 1;
        }
       
    }
}
