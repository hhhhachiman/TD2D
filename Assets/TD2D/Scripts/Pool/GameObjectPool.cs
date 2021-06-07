using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameObjectPool : MonoBehaviour
{

    // Use this for initialization

    //����ģʽ�������֪���Ļ����԰ٶ�һ�£����ﲻ�����ˣ���Ϊˮƽ����
    public static GameObjectPool instance;

    //������������ֵ乹����ĳ��ӣ��ֵ����String���ǿӵ����֣�ÿһ���Ӷ�Ӧһ��GameObject�б�
     Dictionary<string, List<GameObject>> pool = new Dictionary<string, List<GameObject>>() { };
    
    void Start()
    {
        instance = this;//����ģʽ
    }

    //�ӳ��ӵõ�����ķ�����������������������Ҫ�õ������壬������Ҫ���õ�λ��
    //�����������Ӧ���Ѿ�������Ԥ������
    public GameObject GetPool(GameObject go, Vector3 position)
    {
      
        string key = go.name ;//Ҫȥ�ö����Ŀ�����

        GameObject rongqi; //������ȡ�����������


        //������������������
        if (pool.ContainsKey(key) && pool[key].Count > 0)//����Ӵ��ڣ������ж���
        {
            Debug.Log("poolenemyclone");
            //ֱ�����߿�����ĵ�һ��
            rongqi = pool[key][0];
            pool[key].RemoveAt(0);//�ѵ�һ��λ���ͷţ�
        }
        else if (pool.ContainsKey(key) && pool[key].Count <= 0)//�Ӵ��ڣ�����û����
        {
            Debug.Log("empetypool -intopool");
            //�Ǿ�ֱ�ӳ�ʼ��һ����
            rongqi = Instantiate(go, position, Quaternion.identity) as GameObject;
            //IntoPool(go);
        }
        else  //û��
        {
            Debug.Log("nopool");
            //����Ҫ��ʼ������Ҫ�ѿӼ���
            rongqi = Instantiate(go, position, Quaternion.identity) as GameObject;
            pool.Add(key, new List<GameObject>() { });
            //IntoPool(go);
            Debug.Log("create pool success");
        }

        //���������ʼ״̬
        rongqi.SetActive(true);

        //�����Ҽ���һ��������Ҳ��ʾ�Ĵ��룬����Բ��ü�
        //foreach (Transform child in rongqi.transform)
        //{
        //    child.gameObject.SetActive(true);
        //}

        //λ�ó�ʼ��
        rongqi.transform.position = position;
        return rongqi;
    }

    //��������еķ���
    public void IntoPool(GameObject go)
    {
       
        //���������ǵĶ������Ǵӿ����ó����ģ����Է������ȥ��ʱ��϶������Ŀӣ�����ֱ�ӷ��룬���÷������
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