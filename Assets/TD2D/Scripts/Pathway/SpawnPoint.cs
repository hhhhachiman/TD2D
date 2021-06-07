
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy spanwner
/// </summary>
public class SpawnPoint : MonoBehaviour
{   //序列化存储
    [System.Serializable]
    
    public class Wave
    {
        //进攻延迟
        public float delayBeforeWave;
        //敌人队列
        public List<GameObject> enemies;
    }

    //enemies speed random basic
    public float speedRandomizer = 0.2f;
    //delay between enemies spawn in wave
    public float unitSpawnDelay = 0.8f;
    //wave list for the spawner
    public List<Wave> waves;
    [HideInInspector]
    public List<GameObject> randomEnemiesList = new List<GameObject>();
    //初始化路线
    private Pathway path;
   
    private List<GameObject> activeEnemies = new List<GameObject>();

    //所有敌人已孵化
    private bool finished = false;
    /// <summary>
    /// 唤醒实例
    /// </summary>
    void Awake()
    {
        path = GetComponentInParent<Pathway>();
        
        Debug.Assert(path != null, "Wrong initial parameters");
       

    }
   
    /// <summary>
    /// 产生事件
    /// </summary>
    void OnEnable()
    {
        EventManager.StartListening("UnitDie", UnitDie);
        EventManager.StartListening("WaveStart", WaveStart);
    }

    /// <summary>
    /// 关闭事件
    /// </summary>
    void OnDisable()
    {
        EventManager.StopListening("UnitDie", UnitDie);
        EventManager.StopListening("WaveStart", WaveStart);
    }

     void Update()
    {  
        if ( (finished==true)&&(activeEnemies.Count<=0))
        {
            EventManager.TriggerEvent("AllEnemiesAreDead", null, null);
            //孵化点关闭   
            gameObject.SetActive(false);
        }   
    }

    private IEnumerator RunWave(int waveIdx)
    {
        
       
        if (waves.Count>waveIdx)
        {   //yield return :记录上次执行的位置，下次从该位置开始执行
            //Debug.Log("runwave begin1");
            yield return new WaitForSeconds(waves[waveIdx].delayBeforeWave);
            //Debug.Log("runwave begin2");
            foreach (GameObject enemy in waves[waveIdx].enemies)
            {
                //Debug.Log("runwave begin3");
                GameObject prefab ;
                prefab = enemy;
                // If enemy prefab not specified - spawn random enemy
                //if (prefab == null && randomEnemiesList.Count > 0)
                if (prefab == null )
                {
                    if (waves[waveIdx].enemies.Count > 0)
                    {
                        // prefab = randomEnemiesList[Random.Range (0, randomEnemiesList.Count)];
                        GameObject gameObject1 = waves[waveIdx].enemies[Random.Range(0, waves[waveIdx].enemies.Count)];
                        prefab = gameObject1;
                    
                    }
                    Debug.LogError("Have no enemy prefab.Please specify enemies in Level Manager or in Spwan Point");
                }
                
               // Debug.Log("cerateEnemy");
                //创造敌人
                //GameObject newEnemy = Instantiate(prefab, transform.position, transform.rotation);
                GameObject newEnemy = GameObjectPool.instance.GetPool(prefab, transform.position);
                //set pathway
                newEnemy.GetComponent<AiStatePatrol>().path = path;
                NavAgent agent = newEnemy.GetComponent<NavAgent>();
                //set speed offset
                agent.speed = Random.Range(agent.speed * (1f - speedRandomizer), agent.speed * (1f + speedRandomizer));
                //add enemy to list
                activeEnemies.Add(newEnemy);
                //wait for delay before next enemy run
                yield return new WaitForSeconds(unitSpawnDelay);
            }
            if (waveIdx + 1 == waves.Count)
            {
                finished = true;
            }
        }
    }


    private void WaveStart(GameObject obj, string param)
    {
        int waveIdx;
        //TryParse(string, int)j将字符串转换成int输出
        int.TryParse(param, out waveIdx);
        //启动协程控制wave
        StartCoroutine("RunWave", waveIdx);

       //Debug.Log("start runwave"+  waveIdx);

    }

    private void UnitDie(GameObject obj,string param)
    {
        //if this is active enemy
        if (activeEnemies.Contains(obj) == true) 
        {
            //kill it 
            //Remove():移除队列中的元素
            activeEnemies.Remove(obj);

        }
    }

     void OnDestroy()
    {
        StopAllCoroutines();        
    }



}
