using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightScheduler : MonoBehaviour
{
    public static HeightScheduler instance;
    public SpawnLander spawner;
    public int max_count = 5;
    public int count = 5;
    public float heighIncrease = 2;
    void Start()
    {
        instance  = this;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
