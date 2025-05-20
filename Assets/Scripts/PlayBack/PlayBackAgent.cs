using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBackAgent : MonoBehaviour
{
    public Trajector trajectory;
    public int time = 0;
    public LanderController controller;
    public GameObject[] thrusterParticles;
    public float frame_time = 0.2f;
    private void Start()
    {
        InvokeRepeating("Step", 0, frame_time);
    }
    private void Step()
    {
        if (time >= trajectory.states.Count - 1)
        {
            for (int i = 0; i < thrusterParticles.Length; i++)
            {
                thrusterParticles[i].SetActive(false);
            }
        }
        else
        {
            controller.SetThrusterAngle(trajectory.states[time].thrusterAngle);
            controller.SetThrusterThrottle(trajectory.states[time].throttle);
            controller.transform.position = trajectory.states[time].pos;
            controller.transform.rotation = trajectory.states[time].rotation;
            for (int i = 0; i < thrusterParticles.Length; i++)
            {
                if (trajectory.states[time].throttle[i] > 0.3f)
                {
                    thrusterParticles[i].SetActive(true);
                }
                else
                {
                    thrusterParticles[i].SetActive(false);
                }
            }

            time++;
        }
    }
}
