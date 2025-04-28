using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;

public class PPOAgent : Agent
{
    public SpawnLander spawner;
    public Bound bound;
    public RewardGiver rewardGiver;
    public LanderController lander;
    private void Start()
    {
        rewardGiver.addReward.AddListener(this.AddReward);
        rewardGiver.setReward.AddListener(this.SetReward);
    }
    public override void OnEpisodeBegin()
    {
        spawner.SetRandomPos(transform);
        lander.transform.rotation = Random.rotation;
        lander.rb.linearVelocity = Vector3.zero;
        lander.rb.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }
    Vector3 lastVel = Vector3.zero;
    Vector3 lastAngularVel = Vector3.zero;
    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 velocity = lander.rb.linearVelocity;
        Vector3 worldRotation = lander.transform.eulerAngles;
        Vector3 angularVel = lander.rb.angularVelocity;
        Vector3 worldPos = lander.transform.position;   
        float heightFromTerrain = lander.HeightFromTerrain;
        sensor.AddObservation(velocity);
        sensor.AddObservation(worldRotation);
        sensor.AddObservation(worldPos);
        sensor.AddObservation(angularVel);
        sensor.AddObservation(lastVel);
        sensor.AddObservation(lastAngularVel);
        sensor.AddObservation(transform.position.y);

        lastVel = velocity;
        lastAngularVel = angularVel;
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        base.Heuristic(actionsOut);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        var continuousActions = actions.ContinuousActions;

        float[] thrusterAngle = new float[6];
        float[] thruster = new float[3];

        lander.SetThrusterAngle(thrusterAngle);
        lander.SetThrusterThrottle(thruster);

        for (int i = 0; i < 6; i++)
            thrusterAngle[i] = continuousActions[i];
        for (int i = 0; i < 3; i++)
            thruster[i] = continuousActions[6 + i];
        rewardGiver.Step();
        if (!bound.insideBound(transform.position) || lander.transform.position.y < 2)
        {
            EndEpisode();
        }
    }
}
