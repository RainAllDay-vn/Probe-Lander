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
    public float vel_weight = 0.2f, pos_weight = 0.1f, landPadSpawnRadius = 1;
    public float max_rand_angle = 15;
    public bool inPlayBack = false;
    private void Start()
    {
        rewardGiver.addReward.AddListener(this.AddReward);
        rewardGiver.setReward.AddListener(this.SetReward);
        rewardGiver.endEpisode.AddListener(this.EndEpisode);
    }
    public Vector3 landingPad = Vector3.zero;
    public override void OnEpisodeBegin()
    {
        float[] thrusterAngle = { 0,0,0,0,0,0};
        float[] thruster = { 0, 0, 0};
        lander.SetThrusterAngle(thrusterAngle);
        lander.SetThrusterThrottle(thruster);
        spawner.SetRandomPos(transform);
        lander.transform.rotation = Quaternion.Euler(Random.Range(-max_rand_angle,max_rand_angle), Random.Range(-max_rand_angle, max_rand_angle), Random.Range(-max_rand_angle, max_rand_angle));
        lander.rb.velocity = Vector3.zero;
        lander.rb.angularVelocity = Vector3.zero;
        //landingPad = spawner.sampleRandomPos(transform.position, landPadSpawnRadius);
        //landingPad.y = 0;
    }
    Vector3 lastVel = Vector3.zero;
    Vector3 lastAngularVel = Vector3.zero;

    protected Vector3 normalize(Vector3 v)
    {
        Vector3 abs = Vector3.Max(v, -v);
        v.Scale(new Vector3(1 / (abs.x + 1), 1 / (abs.y + 1), 1 / (abs.z + 1)));
        return v;
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 velocity = lander.rb.velocity;
        Vector3 worldRotation = lander.transform.eulerAngles;
        Vector3 angularVel = lander.rb.angularVelocity;
        Vector3 worldPos = lander.transform.position;
        Vector3 landPadPos = landingPad;
        velocity = normalize(velocity * vel_weight);
        worldRotation = worldRotation / 360;
        worldPos = normalize(worldPos * vel_weight);
        angularVel = normalize(angularVel * vel_weight);
        landPadPos = normalize(landPadPos * pos_weight);
        sensor.AddObservation(velocity);
        sensor.AddObservation(worldRotation/360);
        sensor.AddObservation(worldPos);
        sensor.AddObservation(angularVel);
        sensor.AddObservation(landPadPos);
        sensor.AddObservation(lastVel);
        sensor.AddObservation(lastAngularVel);
        lastVel = velocity;
        lastAngularVel = angularVel;
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        var continuousActions = actions.ContinuousActions;

        float[] thrusterAngle = new float[6];
        float[] thruster = new float[3];
      
        for (int i = 0; i < 6; i++)
            thrusterAngle[i] = continuousActions[i];
        for (int i = 0; i < 3; i++)
            thruster[i] = continuousActions[6 + i];
        for (int i = 0; i < thruster.Length; i++)
        {
            thruster[i] = thruster[i] / 2 + 0.5f;
        }
        lander.SetThrusterAngle(thrusterAngle);
        lander.SetThrusterThrottle(thruster);
        rewardGiver.Step();
    }
    private void FixedUpdate()
    {
        if (inPlayBack)
            return;
        if (!bound.insideBound(transform.position))
        {
           
            EndEpisode();
        }
        if(lander.rb.angularVelocity.magnitude>30)
        {
            AddReward(-100);
            EndEpisode();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(landingPad, Vector3.up,Color.blue);
        Debug.DrawLine(landingPad, transform.position);
        for (int i = 0; i < lander.thrusters.Length; i++) {
            Transform thruster = lander.thrusters[i];
            Debug.DrawRay(thruster.position, -thruster.up * lander.throttle[i]);
        }
    }
}
