
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour
{

    public List<GameObject> ObjectPoolList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        //批量将对象压入对象池
        foreach (GameObject obj in ObjectPoolList)
        {
           

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
