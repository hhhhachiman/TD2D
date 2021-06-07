using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendersSpawner : MonoBehaviour
{
    //cooldown for between spawns
    public float cooldown = 10f;
    //max number of spawned objects in buffer
    public int maxNum = 2;
    //spawned object prefab
    public GameObject prefab;
    //position for spawning
    public Transform spawnPoint;

    //denfend points for this tower
    private DefendPoint defPoint;
    //counter for cool down calculation
    private float cooldownCounter;
    //buffer with spawned objects
    private Dictionary<GameObject, Transform> defendersList = new Dictionary<GameObject, Transform>();


    void OnEnable()
    {
        EventManager.StartListening("UnitDie", UnitDie);
       // EventManager.StartListening("UnitDestory", UnitDestory);

    }
    void OnDisable()
    {
        EventManager.StopListening("UnitDie", UnitDie);
        //EventManager.StopListening("UnitDestory", UnitDestory);

    }


    void Start()
    {
        Debug.Assert(spawnPoint, "wrong initial parameters");
        BuildingPlace buildingPlace = GetComponentInParent<BuildingPlace>();
        defPoint = buildingPlace.GetComponentInChildren<DefendPoint>();
        cooldownCounter = cooldown;
        //upgrade all existing defenders on tower building
        foreach (Transform point in defPoint.GetDefendPoints())
        {
            //if defend point already has defender
            AiBehavior defender = point.GetComponentInChildren<AiBehavior>();
            if (defender != null)
            {
                //spawn new defender in the same place
                Spawn(defender.transform, point);
                Debug.Log("startSpawn");
                //destroy old defender
                Debug.Log("defender"+defender.gameObject);
                Destroy(defender.gameObject);
            }
        }
    }

    void FixedUpdate()
    {
        cooldownCounter += Time.fixedDeltaTime;
        if (cooldownCounter>=cooldown)
        {
            //try to spawn new object on cooldown
            if (TryToSpawn()==true)
            {
                cooldownCounter = 0f;
            }
            else
            {
                cooldownCounter = cooldown;
            }
        }

     

    }
    private Transform GetFreeDefendPosition()
    {
        Transform res = null;
        List<Transform> points = defPoint.GetDefendPoints();
        foreach(Transform point in points)
        {   //containsValue:to make a judge for the object existing
            if (defendersList.ContainsValue(point)==false)
            {
                res = point;
                break;
            }
            
        }
        return res;
    }
    private bool TryToSpawn()
    {
        bool res = false;
        //if spawned objects number less than max allowed number
        if ((prefab!=null)&&(defendersList.Count< maxNum))
        {
            Transform destination = GetFreeDefendPosition();
            if (destination!=null)
            {
                //Spawn new defender
                Spawn(spawnPoint, destination);
                res = true;
            }
        }
        return res;
    }

    private void Spawn(Transform position, Transform destination)
    {
       // Debug.Log("trying spawn");
        GameObject obj = Instantiate<GameObject>(prefab, position.position, position.rotation);
        obj.transform.SetParent(destination);
        obj.GetComponent<AiStateMove>().destination= destination;
        defendersList.Add(obj, destination);
        //Debug.Log("finishSpawn");
    }

    private void UnitDie(GameObject obj, string param)
    {
        if (defendersList.ContainsKey(obj)==true)
        {
            //remove it from buffer
            defendersList.Remove(obj);
        }
    }

 

    private void OnDestroy()
    {
        
        Debug.Log("defendersdestory");
        if (GetComponentInParent<BuildingPlace>()!=null || GetComponentInChildren<DefendPoint>()!=null)
        {
            BuildingPlace buildingPlace = GetComponentInParent<BuildingPlace>();
            defPoint = buildingPlace.GetComponentInChildren<DefendPoint>();
            //upgrade all existing defenders on tower building

            foreach (Transform point in defPoint.GetDefendPoints())
            {
                //if defend point already has defender
                AiBehavior defender = point.GetComponentInChildren<AiBehavior>();
                if (defender != null)
                {
                    //destroy old defender
                    Destroy(defender.gameObject);
                }
            }
        }
        
    }
}
