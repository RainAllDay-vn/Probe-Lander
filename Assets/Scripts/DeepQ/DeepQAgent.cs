using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class DeepQAgent : PPOAgent
{
    public Transform landPadTransform;
    public override void OnEpisodeBegin()
    {
        float[] thrusterAngle = { 0, 0, 0, 0, 0, 0 };
        float[] thruster = { 0, 0, 0 };
        lander.SetThrusterAngle(thrusterAngle);
        lander.SetThrusterThrottle(thruster);
        spawner.SetRandomPos(transform);
        lander.transform.rotation = Quaternion.Euler(Random.Range(-max_rand_angle, max_rand_angle), Random.Range(-max_rand_angle, max_rand_angle), Random.Range(-max_rand_angle, max_rand_angle));
        lander.rb.velocity = Vector3.zero;
        lander.rb.angularVelocity = Vector3.zero;
        landPadTransform.position = landingPad;
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        // Retrieve discrete actions
        var discreteActions = actions.DiscreteActions;

        // Process thruster angles (branches 0 to 5 correspond to 6 thrusters)
        float[] thrusterAngles = new float[6];
        for (int i = 0; i < 6; i++)
        {
            int actionIndex = discreteActions[i]; // Action for each thruster angle (0, 1, 2)
            // Map the discrete action (0, 1, 2) to angle (-1, 0, 1)
            thrusterAngles[i] = actionIndex == 0 ? -1f : (actionIndex == 1 ? 0f : 1f);
        }

        // Process thruster throttle (branches 6 and 7 correspond to throttle for 3 thrusters)
        float[] thrusterThrottle = new float[3];
        for (int i = 0; i < 3; i++)
        {
            int actionIndex = discreteActions[6 + i]; // Action for each throttle (0, 1)
            // Map the discrete action (0, 1) to throttle (0, 1)
            thrusterThrottle[i] = actionIndex == 0 ? 0f : 1f;
        }

        // Apply the thruster settings
        lander.SetThrusterAngle(thrusterAngles);
        lander.SetThrusterThrottle(thrusterThrottle);

        // Call step in reward giver
        rewardGiver.Step();
    }
}
