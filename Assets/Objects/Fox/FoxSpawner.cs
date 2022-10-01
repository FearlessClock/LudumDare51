
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxSpawner : MonoBehaviour
{
    [SerializeField] private GameObject foxPrefab;
    [SerializeField] private GameObject[] foxSpawnPoints;
    [SerializeField] private GameObject[] foxInitialTargets;

    [SerializeField]private float time = 0;
    private float spawnTime = 10.0f;

    private void FixedUpdate()
    {
        time += TimeManager.fixedDeltaTime;

        if (time > spawnTime)
        {
            time -= spawnTime;
            SpawnFox();
        }
    }

    private void SpawnFox()
    {
        if (foxSpawnPoints.Length == 0)
            return;

        GameObject spawnPoint = foxSpawnPoints[Random.Range(0, foxSpawnPoints.Length)];
        GameObject initialTarget = foxInitialTargets[Random.Range(0, foxInitialTargets.Length)];
        GameObject fox =  Instantiate(foxPrefab);
        fox.transform.position = spawnPoint.transform.position;
        fox.GetComponent<FoxMovement>().initialTarget = initialTarget.transform.position;
        FoxManager.Instance.foxes.Add(fox);
    }

}