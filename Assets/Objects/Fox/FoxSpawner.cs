using System;
using HelperScripts.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FoxSpawner : MonoBehaviour
{
    [SerializeField] private GameObject foxPrefab;
    [SerializeField] private GameObject[] foxSpawnPoints;
    [SerializeField] private GameObject[] foxInitialTargets;

    [SerializeField]private float time = 0;
    [SerializeField]private float spawnTime = 10.0f;

    private float wave = 0;
    [SerializeField] private AnimationCurve waveDivficulty;
    [SerializeField] private float maxWave = 10;
    [SerializeField] private EventObjectScriptable lastWave;

    private SoundTransmitter st;

    private void Awake()
    {
        st = GetComponent<SoundTransmitter>();
    }

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
        st.Play("Spawn");
        if (foxSpawnPoints.Length == 0)
            return;
        if (wave >= maxWave)
        {
            lastWave.Call(null);
            return;
        }

        wave++;
        float nbFoxInWave = waveDivficulty.Evaluate(wave/maxWave);

        for (int i = 0; i < nbFoxInWave; i++)
        {
            GameObject spawnPoint = foxSpawnPoints[Random.Range(0, foxSpawnPoints.Length)];
            GameObject initialTarget = foxInitialTargets[Random.Range(0, foxInitialTargets.Length)];
            GameObject fox =  Instantiate(foxPrefab);
            fox.transform.position = spawnPoint.transform.position;
            fox.GetComponent<FoxMovement>().initialTarget = initialTarget.transform.position;
            FoxManager.Instance.foxes.Add(fox);
        }
       
    }

}