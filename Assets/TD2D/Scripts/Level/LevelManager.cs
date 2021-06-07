using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //List with allowed randomly generated enemy for this level
    public List<GameObject> allowedEnemies = new List<GameObject>();
    //ui scene loda on level start
    public string levelUiSceneName;
    //initial gold amount for this level
    public int goldAmount = 20;
    //how many times enemies can reach captuer point before defeat
    public int defeatAttempts = 1;
    //user interface manager
    private UiManager uiManager;
    //Numbers of enemy spawners in this level;
    private int spawnNumbers;
    private int beforeLooseCounter;
    void Awake()
    {
        //Load UI scence
        SceneManager.LoadScene(levelUiSceneName,LoadSceneMode.Additive);
    }
    void Start()

    {
        uiManager = FindObjectOfType<UiManager>();
        SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();
        spawnNumbers = spawnPoints.Length;
        if (spawnNumbers<=0)
        {
            Debug.LogError("Have no spawners");
        }
        //set random enemies list for each spawner
        foreach (SpawnPoint spawnPoint in spawnPoints)
        {
            //spawnPoint.randomEnemiesList = allowedEnemies;
        }
        Debug.Assert(uiManager, "Wrong initial parameters");
       uiManager.SetGold(goldAmount);
        beforeLooseCounter = defeatAttempts;
        uiManager.SetDefeatAttempts(beforeLooseCounter);
    }

    // Update is called once per frame
     void OnEnable()
    {
        EventManager.StartListening("Captured", Captured);
        EventManager.StartListening("AllEnemiesAreDead", AllEnemiesAreDead);

            
    }

    private void AllEnemiesAreDead(GameObject obj, string param)
    {
        //Debug.Log("allenedead");
        spawnNumbers--;
        if (spawnNumbers<=0)
        {
            uiManager.GoToVictoryMenu();
        }
    }

    private void Captured(GameObject obj,string param)
    {
        if (beforeLooseCounter>0)
        {
            beforeLooseCounter--;
            uiManager.SetDefeatAttempts(beforeLooseCounter);
            if (beforeLooseCounter<=0)
            {
                //defeat
                uiManager.GoToDefeatMenu();
            }
        }
    }
}
