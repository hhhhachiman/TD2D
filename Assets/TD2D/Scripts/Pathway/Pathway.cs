using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 敌人移动路线
/// </summary>

[ExecuteInEditMode]
//编辑器模式运行
public class Pathway : MonoBehaviour
{
    //if-endif间的内容只能在编辑器中执行
#if UNITY_EDITOR
    private void Update()
    {
        Waypoint[] waypoints = GetComponentsInChildren<Waypoint>();
        if (waypoints.Length>1)
        {
            int idx;
            for (idx = 1;idx<waypoints.Length-1;idx++)
            {
                //根据位移点画移动线
                Debug.DrawLine(waypoints[idx - 1].transform.position, waypoints[idx].transform.position, new Color(0.7f, 0f, 0f));
            }
        }
    }

#endif
  /// <summary>
  /// 获得最近点的坐标和距离，里面的GetHashCode有些难理解
  /// </summary>
  /// <param name="position"></param>
  /// <returns></returns>
    public Waypoint GetNearestWayPoint(Vector3 position)
    {
        float minDistance = float.MaxValue;
        Waypoint nearestWaypoint = null;
        foreach (Waypoint waypoint in GetComponentsInChildren<Waypoint>())
        {
            //判断Waypoint组中取出点于当前点是否为同一点，通过GetHashCode方法判断唯一值
            if (waypoint.GetHashCode() != GetHashCode())
            {
                //计算距离
                Vector3 vect = position - waypoint.transform.position;
                float distance=vect.magnitude;
                //magnitude：Returns the length of this vector (Read Only)
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
    /// 首先判断当前点在队列中的位置，只要不是在队尾，则返回他后一个点的位置
    /// </summary>
    /// <param name="currentWaypoint"></param>
    /// <param name="loop"></param>
    /// <returns></returns>
    public Waypoint GetNextWayPoint(Waypoint currentWaypoint,bool loop)
    {
        Waypoint res = null;
        //获取curr在当前层级中的位置
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
                //在目标点和后续点间添加距离
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
