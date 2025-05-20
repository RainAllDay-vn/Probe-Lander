using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkingReward : RewardGiver
{
    Vector3 pos2d;
    float Normalize(float val)
    {
        return val / (Mathf.Abs(val) + 1f);
    }

    public override void Step()
    {
        Vector3 landPadPos = Vector3.zero;
        pos2d = transform.position;
        pos2d.y = 0;
        Rigidbody rb = lander.rb;

        float distanceFromLandPad = Vector3.Distance(pos2d, landPadPos);
        float speed = rb.velocity.magnitude;
        float y_speed = Mathf.Abs(rb.velocity.y);
        float angularSpeed = rb.angularVelocity.magnitude;
        float angleAlignment = Vector3.Dot(Vector3.up, lander.transform.up); // 1 when upright
        float height = lander.transform.position.y;

        float angularPenalty = Normalize(-angularSpeed) * 0.3f;
        //float heightPenalty = Normalize(-Mathf.Max(0f, height - 2f)) * 0.2f;
        float landPadDistancePenalty = Normalize(-distanceFromLandPad * 0.1f) * 0.7f;
        float y_speed_penalty = Normalize(-y_speed * .1f) * 0.6f;

        float reward = angleAlignment + angularPenalty + y_speed_penalty + landPadDistancePenalty;

        AddReward(reward);

        for (int i = 0; i < 3; i++)
        {
            if (lander.throttle[i] > 0)
                AddReward(-lander.throttle[i] * 0.03f);
        }
    }

    public override void OnEndEpisode()
    {
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.relativeVelocity);
        float yVel = Mathf.Abs(collision.relativeVelocity.y);
        float angVel = lander.rb.angularVelocity.magnitude;

        if (Mathf.Abs(yVel) < 1.5f && angVel < 3 && lander.LegTouched >= 2)
        {
            float bonus = 10 + lander.LegTouched * 20;
            bonus += Normalize(10 - pos2d.magnitude) * 10;
            if (lander.LegTouched == 4)
            {
                bonus += 100;
                AddReward(bonus);
                EndEpisode();
            }

            AddReward(bonus);
            Debug.Log($"Win y vel {yVel} {pos2d.magnitude} leg toched {lander.LegTouched}");
        }
        else
        {
            AddReward(-10f - Mathf.Abs(lander.rb.velocity.y)); // Phạt nặng nếu va chạm xấu
            EndEpisode();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        float angle = Vector3.Angle(Vector3.up, lander.transform.up);
        if (Mathf.Abs(angle) > 90f)
        {
            AddReward(-20f);
            EndEpisode();
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Ground")
        {
            AddReward(-20);
        }
    }
}
