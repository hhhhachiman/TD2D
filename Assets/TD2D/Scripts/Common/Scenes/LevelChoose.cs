using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelChoose : MonoBehaviour
{   //Scene to exit
    public string exitSceneName;
    //choosen level
    public GameObject currentLevel;
    //all levels
    public List<GameObject> levelsPrefabs = new List<GameObject>();
    //visual dispalying for number of levels
    public Transform togglesFolder;
    //incative toggle prefab
    public Toggle inactiveTogglePrefab;
    //active toggle prefab
    public Toggle activeTogglePrefab;
    public Button nextLevelButton;
    public Button prevLevelButton;

    //Index of last allowed level for choosing
    private int maxActiveLevelIdx;
    //index of current dispalyed level
    private int currentDisplayedLevelIdx;
    //list with active toggles
    private List<Toggle> activeToggles = new List<Toggle>();
    private void Awake()
    {
        maxActiveLevelIdx = -1;
        Debug.Assert(currentLevel & togglesFolder && activeTogglePrefab && inactiveTogglePrefab && nextLevelButton && prevLevelButton,
            "Wrong initial Settings");
    }

    void Start()
    {
        int hitIdx = -1;
        int levelsCount = DataManager.instance.progress.openedLevels.Count;
        if (levelsCount>0)
        {
            //get name of last opened level from stored data
            string openedLevelName = DataManager.instance.progress.openedLevels[levelsCount - 1];
            int idx;
            for (idx = 0;idx<levelsPrefabs.Count; ++idx)
            {
                //try to find last opend level in levels list
                if (levelsPrefabs[idx].name==openedLevelName)
                {
                    hitIdx = idx;
                    break;
                }
            }
        }

        //level found
        if (hitIdx>=0)
        {
            if (levelsPrefabs.Count>hitIdx+1)
            {
                maxActiveLevelIdx = hitIdx + 1;
            }
            else
            {
                maxActiveLevelIdx = hitIdx;
            }
        }
        else
        {
            if (levelsPrefabs.Count>0)
            {
                maxActiveLevelIdx = 0;
            }
            else
            {
                Debug.LogError("Have no levels prefabs!");
            }
        }
        if (maxActiveLevelIdx>=0)
        {
            DisplayToggles();
            DisplayLevel(maxActiveLevelIdx);
        }
    }

    private void DisplayLevel(int levelIdx)
    {
        Transform parentOfLevel = currentLevel.transform.parent;
        Vector3 levelPosition = currentLevel.transform.position;
        Quaternion levelRotation = currentLevel.transform.rotation;
        Destroy(currentLevel);
      
        currentLevel = Instantiate(levelsPrefabs[levelIdx],parentOfLevel);
        currentLevel.name = levelsPrefabs[levelIdx].name;
        currentLevel.transform.position = levelPosition;
        currentLevel.transform.rotation = levelRotation;
        currentDisplayedLevelIdx = levelIdx;
        foreach(Toggle toggle in activeToggles)
        {
            toggle.isOn = false;
        }
        activeToggles[levelIdx].isOn = true;
        UpdateButtonsVisible(levelIdx);
    }

    private void UpdateButtonsVisible(int levelIdx)
    {   //按钮是否可用
        prevLevelButton.interactable = levelIdx > 0 ? true : false;
        nextLevelButton.interactable = levelIdx < maxActiveLevelIdx ? true : false;
    }

    private void DisplayToggles()
    {
        foreach (Toggle toggle in togglesFolder.GetComponentsInChildren<Toggle>())
        {
            Destroy(toggle.gameObject);
        }
        int cnt;
        for (cnt = 0; cnt < maxActiveLevelIdx+1; cnt++)
        {
            GameObject toggle = Instantiate(activeTogglePrefab.gameObject, togglesFolder);
            activeToggles.Add(toggle.GetComponent<Toggle>());
        }
        if (maxActiveLevelIdx<levelsPrefabs.Count-1)
        {
            Instantiate(inactiveTogglePrefab.gameObject, togglesFolder);
        }
    }

    public void DisplayNextLevel()
    {
        if (currentDisplayedLevelIdx<maxActiveLevelIdx)
        {
            DisplayLevel(currentDisplayedLevelIdx + 1);
        }
    }

    public void DisplayPrevLevel()
    {
        if (currentDisplayedLevelIdx>0)
        {
            DisplayLevel(currentDisplayedLevelIdx - 1);
        }
    }
    public void Exit()
    {
        SceneManager.LoadScene(exitSceneName);
        SoundManager.play_effect("Sounds/loaderClose");
    }

    public void GoToLevel()
    {
        SceneManager.LoadScene(currentLevel.name);
        SoundManager.play_effect("Sounds/loaderClose");
        Debug.Log(currentLevel.name);
    }
}
