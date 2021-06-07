using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UiManager : MonoBehaviour
{
    // This scene will loaded after whis level exit
    public string exitSceneName;
    //startScreen canvas
    public GameObject startScreen;
    //pause menu canvas
    public GameObject pauseMenu;
    // Defeat menu canvas
    public GameObject defeatMenu;
    // Victory menu canvas
    public GameObject victoryMenu;
    // Level interface
    public GameObject levelUI;
    // Avaliable gold amount
    public Text goldAmount;
    // Capture attempts before defeat
    public Text defeatAttempts;
    // Victory and defeat menu display delay
    public float menuDisplayDelay = 1f;

    //camera controll component
    private CameraControl cameraControl;
    //camera is draging now
    private bool cameraIsDragged;
    //origin point of camera drag start
    private Vector3 dragOrigin = Vector3.zero;
    // is game paused?
    private bool paused;
    void Awake()
    {
        cameraControl = FindObjectOfType<CameraControl>();
        Debug.Assert(cameraControl && startScreen && pauseMenu && defeatMenu && victoryMenu && levelUI && defeatAttempts && goldAmount, "Wrong initial parameters");
    }


    void OnEnable()
    {
        EventManager.StartListening("UnitKilled", UnitKilled);
        EventManager.StartListening("ButtonPressed", ButtonPressed);
        EventManager.StartListening("SellTower", SellTower);
    }
    void OnDisable()
    {
        EventManager.StopListening("UnitKilled", UnitKilled);
        EventManager.StopListening("ButtonPressed", ButtonPressed);
        EventManager.StopListening("SellTower", SellTower);
    }
    void Start()
    {
        PauseGame(true);
        SoundManager.init();
    }

    void Update()
    {
        if (paused == false)
        {
            //user press mouse button
            if (Input.GetMouseButtonDown(0) == true)
            {
                //check if pointer over UI components
                GameObject hittedObj = null;
                PointerEventData pointerData = new PointerEventData(EventSystem.current);
                pointerData.position = Input.mousePosition;
                //射线检测
                List<RaycastResult> results = new List<RaycastResult>();
                //投射一条光线并返回所有碰撞，也就是投射光线并返回一个RaycastHit[]结构体。
                EventSystem.current.RaycastAll(pointerData, results);
                if (results.Count > 0)
                {
                    foreach (RaycastResult res in results)
                    {
                        if (res.gameObject.CompareTag("ActionIcon"))
                        {
                            hittedObj = res.gameObject;
                            break;
                        }
                    }
                    //send message with user click on ui component
                    EventManager.TriggerEvent("UserUiClick", hittedObj, null);
                }
                else//no ui components on pointer
                {
                    //check if pointer over colliders
                    RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward);
                    foreach (RaycastHit2D hit in hits)
                    {
                        //if this is allowed collider
                        if (hit.collider.gameObject.CompareTag("Tower")
                            || hit.collider.gameObject.CompareTag("Enemy")
                            || hit.collider.gameObject.CompareTag("Defender")
                           )
                           
                        {
                            hittedObj = hit.collider.gameObject;
                            break;
                        }
                    }
                    //send message with user click data on game space
                    EventManager.TriggerEvent("UserClick", hittedObj, null);
                }
                //if there is no hitted object,then start camera drag
                if (hittedObj == null)
                {
                    cameraIsDragged = true;
                    dragOrigin = Input.mousePosition;
                }
            }
            if (Input.GetMouseButtonUp(0) == true)
            {
                //stop drag camera on mouse release
                cameraIsDragged = false;
            }
            if (cameraIsDragged == true)
            {//实现坐标点position从屏幕坐标系向摄像机视口的单位化坐标系转换。
                Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
                cameraControl.MoveX(-pos.x);
                cameraControl.MoveY(-pos.y);
            }
        }
    }

    public Vector3 GetMousePoisition()
    {
        Vector3 flagposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 uiPos = new Vector3(flagposition.x, Screen.height - flagposition.y, 0);
        return uiPos;
    }
    private void LoadScene(string sceneName)
    {
        EventManager.TriggerEvent("SceneQuit", null, null);
        SceneManager.LoadScene(sceneName);
        SoundManager.play_effect("Sounds/loaderOpen");
    }

    private void ResumeGame()
    {
        GoToLevel();
        PauseGame(false);
    }

    private void ExitFromLevel()
    {
        LoadScene(exitSceneName);
    }

    private void PauseGame(bool pause)
    {
        paused = pause;
        //stop the time on pause
        Time.timeScale = pause ? 0f : 1f;
        EventManager.TriggerEvent("GamePaused", null, pause.ToString());
    }

    private void GoToPauseMenu()
    {
        PauseGame(true);
        CloseAllUI();
        pauseMenu.SetActive(true);
    }
    private void GoToLevel()
    {
        
        CloseAllUI();
        levelUI.SetActive(true);
        PauseGame(false);
        SoundManager.play_effect("Sounds/loaderOpen");
    }

    private void CloseAllUI()
    {
        startScreen.SetActive(false);
        pauseMenu.SetActive(false);
        defeatMenu.SetActive(false);
        victoryMenu.SetActive(false);
    }

    private void UnitKilled(GameObject obj, string param)
    {
        if (obj.CompareTag("Enemy"))
        {
            Price price = obj.GetComponent<Price>();
            if (price!=null)
            {
                //add gold for enemy kill
                AddGold(price.price);

            }

        }
    }

    private void SellTower(GameObject obj,string param)
    {
        Price price = obj.GetComponent<Price>();
        if (price!=null)
        {
            AddGold(price.price);
            SoundManager.play_effect("Sounds/sell_tower");
            //AudioManager.Instance.Play("sell_tower");
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void GoToDefeatMenu()
    {
        StartCoroutine("DefeatCoroutine");
    }

    private IEnumerator DefeatCoroutine()
    {
        yield return new WaitForSeconds(menuDisplayDelay);
        PauseGame(true);
        CloseAllUI();
        defeatMenu.SetActive(true);
        SoundManager.play_effect("Sounds/gameover");
       // AudioManager.Instance.Play("gameover");
    }

    public void GoToVictoryMenu()
    {
        Debug.Log("gotovictory");
        StartCoroutine("VictoryCoroutine");
    }

    private IEnumerator VictoryCoroutine()
    {
        yield return new WaitForSeconds(menuDisplayDelay);
        PauseGame(true);
        CloseAllUI();
        //game progress autosaving
        //get the name of the completed
        Debug.Log("victory");
        DataManager.instance.progress.lastCompetedLevel = SceneManager.GetActiveScene().name;
        //check if this level have no completed before;
        bool hit = false;
        foreach (string level in DataManager.instance.progress.openedLevels)
        {
            if (level == SceneManager.GetActiveScene().name)
            {
                hit = true;
                break;
            }
        }
        if (hit == false)
        {
            DataManager.instance.progress.openedLevels.Add(SceneManager.GetActiveScene().name);
        }
        //save game progess
        DataManager.instance.SaveGameProgess();
        victoryMenu.SetActive(true);
        SoundManager.play_effect("Sounds/victory");
       // AudioManager.Instance.Play("victory");
    }

    private void RestartLevel()
    {
        LoadScene(SceneManager.GetActiveScene().name);
        SoundManager.play_effect("Sounds/loaderClose");
    }
    private int GetGold()
    {
        int gold;
        int.TryParse(goldAmount.text, out gold);
        return gold;
    }

    /// <summary>
    /// Sets gold amount.
    /// </summary>
    /// <param name="gold">Gold.</param>
	public void SetGold(int gold)
    {
        goldAmount.text = gold.ToString();
    }

    /// <summary>
    /// Adds the gold.
    /// </summary>
    /// <param name="gold">Gold.</param>
    private void AddGold(int gold)
    {
        SetGold(GetGold() + gold);
    }

    /// <summary>
    /// Spends the gold if it is.
    /// </summary>
    /// <returns><c>true</c>, if gold was spent, <c>false</c> otherwise.</returns>
    /// <param name="cost">Cost.</param>
    public bool SpendGold(int cost)
    {
        bool res = false;
        int currentGold = GetGold();
        if (currentGold >= cost)
        {
            SetGold(currentGold - cost);
            res = true;
        }
        return res;
    }

    /// <summary>
    /// Sets the defeat attempts.
    /// </summary>
    /// <param name="attempts">Attempts.</param>
    public void SetDefeatAttempts(int attempts)
    {
        defeatAttempts.text = attempts.ToString();
    }

    private void ButtonPressed(GameObject obj, string param)
    {
        switch (param)
        {
            case "Pause":
                GoToPauseMenu();
                break;
            case "Resume":
                ResumeGame();
                break;
            case "Back":
                ExitFromLevel();
                break;
            case "Restart":
                RestartLevel();
                break;
        }
    }

}
