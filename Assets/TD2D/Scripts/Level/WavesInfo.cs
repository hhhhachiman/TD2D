using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WavesInfo : MonoBehaviour
{   //to between waves by default
    public float defaultWaveTimeout = 10f;
    //to between waves
    public List<float> wavesTimeouts = new List<float>();
#if UNITY_EDITOR
    //list with active spawners in level
    private SpawnPoint[] spawners;
    void Start()
    {
        spawners = FindObjectsOfType<SpawnPoint>();

    }

    // Update is called once per frame
    void Update()
    {
        int wavesCount = 0;
        foreach (SpawnPoint spawner in spawners)
        {
            if (spawner.waves.Count > wavesCount)
            {
                wavesCount = spawner.waves.Count;
            }
        }

        //display actual list with waves timeouts
        if (wavesTimeouts.Count < wavesCount)
        {
            int i;
            for (i = wavesTimeouts.Count; i < wavesCount; ++i)
            {
                wavesTimeouts.Add(defaultWaveTimeout);
            }

        }
        else if (wavesTimeouts.Count > wavesCount)
        {
            wavesTimeouts.RemoveRange(wavesCount, wavesTimeouts.Count - wavesCount);
        }

    }
#endif
}
