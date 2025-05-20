using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryCollector : MonoBehaviour
{
    // Start is called before the first frame update

    public List<Trajector.State> states = new List<Trajector.State>();
    public LanderController lander;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Trajector.State state =new Trajector.State();
        state.throttle = new float[3];
        lander.throttle.CopyTo(state.throttle,0);
        state.thrusterAngle = new float[6];
        lander.angle.CopyTo(state.thrusterAngle,0);
        state.rotation = transform.rotation;
        state.pos = transform.position;
        states.Add(state); 
    }
}
