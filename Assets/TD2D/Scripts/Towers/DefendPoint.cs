using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendPoint : MonoBehaviour
{
    //prefab for defend point
    public GameObject defendPointPrefab;
    //list with defend places for this defend point
    private List<Transform> defendPlaces = new List<Transform>();

    void Awake()
    {
        Debug.Assert(defendPointPrefab, "Wrong initial parameters");
        //get defend places from defend point prefab and place it on scene
        foreach (Transform defendPlace in defendPointPrefab.transform)
        {
            Instantiate(defendPlace.gameObject, transform);
            //Debug.Log("start defendplace");
        }
        //create defend places list
        foreach (Transform child in transform)
        {
            defendPlaces.Add(child);
        }
    }
    public List<Transform> GetDefendPoints()
    {
        return defendPlaces;
    }

    //public  List<Transform> UpdateDefendPoints(Transform changePos)
    //{
    //    = changePos;   
    //    return defendPlaces;
    //}
}
