using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �����ƶ�·��
/// </summary>

[ExecuteInEditMode]
//�༭��ģʽ����
public class Pathway : MonoBehaviour
{
    //if-endif�������ֻ���ڱ༭����ִ��
#if UNITY_EDITOR
    private void Update()
    {
        Waypoint[] waypoints = GetComponentsInChildren<Waypoint>();
        if (waypoints.Length>1)
        {
            int idx;
            for (idx = 1;idx<waypoints.Length-1;idx++)
            {
                //����λ�Ƶ㻭�ƶ���
                Debug.DrawLine(waypoints[idx - 1].transform.position, waypoints[idx].transform.position, new Color(0.7f, 0f, 0f));
            }
        }
    }

#endif
  /// <summary>
  /// �������������;��룬�����GetHashCode��Щ�����
  /// </summary>
  /// <param name="position"></param>
  /// <returns></returns>
    public Waypoint GetNearestWayPoint(Vector3 position)
    {
        float minDistance = float.MaxValue;
        Waypoint nearestWaypoint = null;
        foreach (Waypoint waypoint in GetComponentsInChildren<Waypoint>())
        {
            //�ж�Waypoint����ȡ�����ڵ�ǰ���Ƿ�Ϊͬһ�㣬ͨ��GetHashCode�����ж�Ψһֵ
            if (waypoint.GetHashCode() != GetHashCode())
            {
                //�������
                Vector3 vect = position - waypoint.transform.position;
                float distance=vect.magnitude;
                //magnitude��Returns the length of this vector (Read Only)
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestWaypoint = waypoint;
                }
            }

        }
        return nearestWaypoint;
    }

    /// <summary>
    /// �����жϵ�ǰ���ڶ����е�λ�ã�ֻҪ�����ڶ�β���򷵻�����һ�����λ��
    /// </summary>
    /// <param name="currentWaypoint"></param>
    /// <param name="loop"></param>
    /// <returns></returns>
    public Waypoint GetNextWayPoint(Waypoint currentWaypoint,bool loop)
    {
        Waypoint res = null;
        //��ȡcurr�ڵ�ǰ�㼶�е�λ��
        int idx = currentWaypoint.transform.GetSiblingIndex();
        if (idx<(transform.childCount-1))
        {
            idx += 1;
        }
        else
        {
            idx = 0;
        }
        if (!(loop==false&&idx==0))
        {
            res = transform.GetChild(idx).GetComponent<Waypoint>();
        }
        return res;
    }

    public float GetPathDistance(Waypoint fromWaypoint)
    {
        Waypoint[] waypoints = GetComponentsInChildren<Waypoint>();
        float pathDistance = 0f;
        bool hitted = false;
        int idx;
        for ( idx = 0; idx < waypoints.Length;++idx)
        {
            if (hitted ==true)
            {
                //��Ŀ���ͺ��������Ӿ���
                Vector2 distance = waypoints[idx].transform.position - waypoints[idx - 1].transform.position;
                pathDistance += distance.magnitude;
            }
            if (waypoints[idx]==fromWaypoint)
            {
                hitted = true;
            }
        }
        
        return pathDistance;
    }
}
