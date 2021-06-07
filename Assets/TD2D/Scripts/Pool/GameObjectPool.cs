using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameObjectPool : MonoBehaviour
{

    // Use this for initialization

    //单例模式，如果不知道的话可以百度一下，这里不解释了，因为水平有限
    public static GameObjectPool instance;

    //下面这句是用字典构造你的池子，字典里的String就是坑的名字，每一个坑对应一个GameObject列表
     Dictionary<string, List<GameObject>> pool = new Dictionary<string, List<GameObject>>() { };
    
    void Start()
    {
        instance = this;//单例模式
    }

    //从池子得到物体的方法，传递两个参数，你需要得到的物体，和你需要放置的位置
    //你所需的物体应该已经制作成预置物体
    public GameObject GetPool(GameObject go, Vector3 position)
    {
      
        string key = go.name ;//要去拿东西的坑名字

        GameObject rongqi; //你用来取物体的容器；


        //下面分三种情况来分析
        if (pool.ContainsKey(key) && pool[key].Count > 0)//如果坑存在，坑里有东西
        {
            Debug.Log("poolenemyclone");
            //直接拿走坑里面的第一个
            rongqi = pool[key][0];
            pool[key].RemoveAt(0);//把第一个位置释放；
        }
        else if (pool.ContainsKey(key) && pool[key].Count <= 0)//坑存在，坑里没东西
        {
            Debug.Log("empetypool -intopool");
            //那就直接初始化一个吧
            rongqi = Instantiate(go, position, Quaternion.identity) as GameObject;
            //IntoPool(go);
        }
        else  //没坑
        {
            Debug.Log("nopool");
            //不仅要初始化，还要把坑加上
            rongqi = Instantiate(go, position, Quaternion.identity) as GameObject;
            pool.Add(key, new List<GameObject>() { });
            //IntoPool(go);
            Debug.Log("create pool success");
        }

        //调整物体初始状态
        rongqi.SetActive(true);

        //这里我加了一个子物体也显示的代码，你可以不用加
        //foreach (Transform child in rongqi.transform)
        //{
        //    child.gameObject.SetActive(true);
        //}

        //位置初始化
        rongqi.transform.position = position;
        return rongqi;
    }

    //放入池子中的方法
    public void IntoPool(GameObject go)
    {
       
        //理论上我们的东西都是从坑里拿出来的，所以放物体进去的时候肯定有他的坑，可以直接放入，不用分情况了
        string key = go.name;
      
            pool[key].Add(go);
        
        //go.SetActive(false);
      
    }

    public bool GetPoolSize(GameObject go)
    {
        string key = go.name;
        bool poolfilled;
        if (pool.ContainsKey(key) && pool[key].Count<4)
        {
            poolfilled = false;
        }
        else
        {
            poolfilled = true;
        }
        return poolfilled;
    }
    
}