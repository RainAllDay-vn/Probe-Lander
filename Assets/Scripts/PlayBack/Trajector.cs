using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(menuName ="Trajectory")]
public class Trajector : ScriptableObject
{
    [Serializable]
    public struct State
    {
        public Vector3 pos;
        public Quaternion rotation;
        public float[] thrusterAngle;
        public float[] throttle;

    }
    public List<State> states; 
    public float reward = -10;
}
