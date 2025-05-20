using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DenseReward : RewardGiver
{
    bool _done = false;
    bool _hasTouchedDown = false;
    float dt1 = 0;
    float vt1 = 0;
    float wt1 = 0;
    float landReward = 0;
    public override void Step()
    {
        dt1 = transform.position.magnitude;
        vt1 = lander.rb.velocity.magnitude;
        wt1 = lander.rb.angularVelocity.magnitude;
        landReward = 0;
    }
    private void FixedUpdate()
    {
        float d= transform.position.magnitude;
        float v = lander.rb.velocity.magnitude;
        float w = lander.rb.angularVelocity.magnitude;
        AddReward(-100 * (d - dt1) - 100 * (v - vt1) - 100 * (w - wt1) + landReward);
        dt1 =d;
        vt1 = v;
        wt1 = w;
        if(lander.rb.angularVelocity.y > 10)
        {
            AddReward(-100);
            EndEpisode();
        }
        if(lander.rb.velocity.y > 2)
        {
            AddReward(-100);
            EndEpisode();
        }
    }

    public override void OnEndEpisode()
    {
        _done = false;
        _hasTouchedDown = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_done) return;
        if (collision.gameObject.name == "AgentRocket") return;
        if (collision.relativeVelocity.magnitude > 3)
        {
            AddReward(-50-collision.relativeVelocity.magnitude);
            EndEpisode();
            return;
        }
        // mark that we’ve touched ground at least once
        _hasTouchedDown = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (_done || !_hasTouchedDown) return;

        Vector2 pos2d = new Vector2(transform.position.x, transform.position.z);
        // check leg count
        if (lander.LegTouched >= 4)
        {
            HeightScheduler.instance.count--;
            if (HeightScheduler.instance.count == 0)
            {
                HeightScheduler.instance.count = HeightScheduler.instance.max_count;
                HeightScheduler.instance.spawner.yLevel += HeightScheduler.instance.heighIncrease;
            }
            // full success
            if (pos2d.magnitude < 10)
            {
                Debug.Log("Win");
                AddReward(100);
            }


            _done = true;
            EndEpisode();

        }
        // else: fewer than 4 legs, keep waiting
    }
}
