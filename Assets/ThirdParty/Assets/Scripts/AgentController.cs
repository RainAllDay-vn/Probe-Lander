﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

[RequireComponent(typeof(RocketController))]
public class AgentController : Agent
{
    public EnvironmentParameters m_ResetParams;
    private RocketController rc;

    public bool episodeFinished;

    void Start()
    {
        rc = GetComponent<RocketController>();
    }

    public override void OnEpisodeBegin()
    {
        m_ResetParams = Academy.Instance.EnvironmentParameters;

        rc.height = m_ResetParams.GetWithDefault("init_height", 550);
        rc.init_angle_roll = m_ResetParams.GetWithDefault("init_angle_roll", 5);
        rc.init_angle_pitch = m_ResetParams.GetWithDefault("init_angle_pitch", 90);
        rc.init_xoffset = m_ResetParams.GetWithDefault("init_xoffset", 15);
        rc.init_zoffset = m_ResetParams.GetWithDefault("init_zoffset", 90);
        rc.init_zspeed = m_ResetParams.GetWithDefault("init_zspeed", 1);
        rc.angle_tvc = m_ResetParams.GetWithDefault("angle_tvc", 7);
        rc.thrust_engine = m_ResetParams.GetWithDefault("thrust_engine", 5000);
        rc.thrust_rcs = m_ResetParams.GetWithDefault("thrust_rcs", 0.2f);
        rc.collision_speed = m_ResetParams.GetWithDefault("collision_speed", 5);

        episodeFinished = false;
        rc.ResetPosition();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 rocketPosition = rc.GetPosition();
        Vector3 rocketVelocity = rc.GetVelocity();
        Vector3 rocketAngularVelocity = rc.GetAngularVelocity();
        float rollIndicator = rc.GetRollRotation();
        float pitchIndicator = rc.GetPitchRotation();
        float upsideDownIndicator = rc.GetUpsideDownRotation();

        sensor.AddObservation(rocketPosition.x);
        sensor.AddObservation(rocketPosition.y);
        sensor.AddObservation(rocketPosition.z);

        sensor.AddObservation(rocketVelocity.x);
        sensor.AddObservation(rocketVelocity.y);
        sensor.AddObservation(rocketVelocity.z);

        sensor.AddObservation(rollIndicator);
        sensor.AddObservation(upsideDownIndicator);
        sensor.AddObservation(pitchIndicator);

        sensor.AddObservation(rocketAngularVelocity.x);
        sensor.AddObservation(rocketAngularVelocity.z);

        /*
        string[] returnable = new string[11];
        returnable[0] = rc.GetPosition().x.ToString();
        returnable[1] = rc.GetPosition().y.ToString();
        returnable[2] = rc.GetPosition().z.ToString();
        returnable[3] = rc.GetVelocity().x.ToString();
        returnable[4] = rc.GetVelocity().y.ToString();
        returnable[5] = rc.GetVelocity().z.ToString();
        returnable[6] = rc.GetXRotation().ToString();
        returnable[7] = rc.GetYRotation().ToString();
        returnable[8] = rc.GetZRotation().ToString();
        returnable[9] = rc.GetAngularVelocity().x.ToString();
        returnable[10] = rc.GetAngularVelocity().z.ToString();
        CSVManager.AppendToReport(returnable);
        */
    }

    public override void OnActionReceived(ActionBuffers vectorAction)
    {
        rc.ModifyEngineState((int)vectorAction.ContinuousActions[0], (int)vectorAction.ContinuousActions[1], (int)vectorAction.ContinuousActions[2]);
        rc.ModifyRollRCSState((int)vectorAction.ContinuousActions[3]);
        rc.ModifyPitchRCSState((int)vectorAction.ContinuousActions[4]);
    }

    public void EndEpisode(float reward)
    {
        AddReward(reward);

        episodeFinished = true;
        StartCoroutine(WaitCoroutine());
    }

    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        EndEpisode();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // 0: MOTEUR (OFF/ON), 1: TVC ROLL (OFF, -5°, +5°), 2: TVC PITCH (OFF, -5°, +5°), 3: RCS ROLL (OFF, POS, NEG), 4 : RCS PITCH (OFF, POS, NEG)

    }
}
