using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class WavesTimer : MonoBehaviour
{
    public Image timeBar;
    // Current wave text field
    // Effect of highlighted timer
    //public GameObject highlightedFX;
    // Current wave text field
    public Text currentWaveText;
    // Max wave text field
    public Text maxWaveNumberText;
    // Duration for highlighted effect
    //public float highlightedTO = 0.2f;

    //waves description for this game level
    private WavesInfo wavesInfo;
    // Waves list
    private List<float> waves = new List<float>();
    // Timer stopped
    private bool finished;
    private  int currentWave;
    private float counter;
    // TO before next wave
    private float currentTimeout;
    void OnDisable()
    {
        StopAllCoroutines();    
    }

     void Awake()
    {
        wavesInfo = FindObjectOfType<WavesInfo>();
        Debug.Assert(timeBar && wavesInfo && timeBar && currentWaveText && maxWaveNumberText, "Wrong initial settings");
    }

     void Start()
    {
        //highlightedFX.SetActive(false);
        waves = wavesInfo.wavesTimeouts;
        currentWave = 0;
        counter = 0f;
        finished = false;
        GetCurrentWaveCounter();
        maxWaveNumberText.text = waves.Count.ToString();
        currentWaveText.text = "0";
    }

     void FixedUpdate()
    {
        if (finished == false)
        {
            //timeout expired
            if (counter<=0f)
            {
                //send event about next wave start
                EventManager.TriggerEvent("WaveStart", null, currentWave.ToString());
                SoundManager.play_effect("Sounds/bo_start");
                currentWave++;
                currentWaveText.text = currentWave.ToString();
                //Highlight the timer for short time;
                //StartCoroutine("HighlightTimer");
                //when all waves are send
                if (GetCurrentWaveCounter() == false)
                {
                    finished = true;
                    //send event about timer stop
                    EventManager.TriggerEvent("TimerEnd", null,null);
                    return;
                }
            }
            counter -= Time.fixedDeltaTime;
            if (currentTimeout>0f)
            {
                float timerr = counter / currentTimeout;
                timeBar.fillAmount = 1 - timerr;

                //timeBar.fillAmount = counter / currentTimeout;

            }
            else
            {
                timeBar.fillAmount = 0f;
            }
        }    
    }
    private bool GetCurrentWaveCounter()
    {
        bool res = false;
        if (waves.Count>currentWave)
        {
            counter = currentTimeout = waves[currentWave];
            res = true;
        }
        return res;
    }

    //private IEnumerator HighlightTimer()
    //{
    //    highlightedFX.SetActive(true);
    //    yield return new WaitForSeconds(highlightedTO);
    //    highlightedFX.SetActive(false);
    //}

    void OnDestroy()
    {
        StopAllCoroutines();
    }
}
